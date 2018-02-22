#region Copyright 2017 D-Haven.org

// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using DHaven.MicroMvvm.Dialog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DHaven.MicroMvvm.Wpf
{
    public class Locator
    {
        private const string ViewModel = "ViewModel";
        private const string View = "View";

        private static readonly Type[] FrameworkTypes =
        {
            typeof(IViewModel),
            typeof(IValueConverter),
            typeof(IMultiValueConverter),
            typeof(ICommand),
            typeof(UserControl),
            typeof(Page)
        };

        private static readonly Dictionary<string, Type> DiscoveredTypes = new Dictionary<string, Type>();

        public static IList<AssemblyName> UnscannableAssemblies { get; } = new List<AssemblyName>();

        static Locator()
        {
            var searchedAssemblies = new List<string>();
            var startingAssembly = Application.Current.GetType().Assembly;

            ScanForFrameworkTypes(startingAssembly, ref searchedAssemblies);

            searchedAssemblies.Clear();
        }

        public object this[object dataContext]
        {
            get
            {
                var shortClassName = dataContext as string;

                if (dataContext is IViewModel)
                {
                    var viewModelType = dataContext.GetType();

                    if (viewModelType.Name.EndsWith(ViewModel))
                        shortClassName = viewModelType.Name.Substring(0, viewModelType.Name.Length - ViewModel.Length) +
                                         View;
                }

                if (dataContext is UserControl || dataContext is Page)
                {
                    var viewType = dataContext.GetType();

                    if (viewType.Name.EndsWith(View))
                        shortClassName = viewType.Name.Substring(0, viewType.Name.Length - View.Length) + ViewModel;
                }

                if (dataContext is Message)
                    shortClassName = dataContext.GetType().Name + View;

                return CreateObjectFor(shortClassName ?? dataContext?.GetType().Name);
            }
        }

        private static void ScanForFrameworkTypes(Assembly assembly, ref List<string> searchedAssemblies)
        {
            if (searchedAssemblies.Contains(assembly.FullName))
            {
                return;
            }

            Debug.WriteLine($"Scanning {assembly.FullName}");
            searchedAssemblies.Add(assembly.FullName);

            foreach (var type in assembly.DefinedTypes.Where(IsFrameworkType))
            {
                try
                {
                    Debug.WriteLine($"Adding type {type.FullName}");
                    DiscoveredTypes.Add(type.Name, type);
                }
                catch (Exception e)
                {
                    var discoveredType = DiscoveredTypes[type.Name];
                    // Add some context so that when it is caught we can see what's going on.
                    throw new DHavenInitializationException($"\"{type.Name}\" is mapped to [{discoveredType.FullName}@{discoveredType.Assembly.FullName}]," +
                        $" instead of [{type.FullName}@{type.Assembly.FullName}]", e);
                }
            }

            foreach (var referencedName in assembly.GetReferencedAssemblies())
            {
                if (!searchedAssemblies.Contains(referencedName.ToString()))
                {
                    try
                    {
                        var dependency = Assembly.Load(referencedName);
                        ScanForFrameworkTypes(dependency, ref searchedAssemblies);
                    }
                    catch(Exception e)
                    {
                        // Load error, skip the assembly
                        UnscannableAssemblies.Add(referencedName);
                    }
                }
            }
        }

        private static bool IsFrameworkType(TypeInfo type)
        {
            return FrameworkTypes.Any(ft => ft.IsAssignableFrom(type));
        }

        private static object CreateObjectFor(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return null;

            Type targetType;
            if (DiscoveredTypes.TryGetValue(className, out targetType))
                try
                {
                    var constructor = targetType.GetConstructor(new Type[0]);
                    return constructor?.Invoke(new object[0]);
                }
                catch
                {
                    // Ignore.  Couldn't create, so we swallow the exception and return null.
                }

            return null;
        }
    }
}