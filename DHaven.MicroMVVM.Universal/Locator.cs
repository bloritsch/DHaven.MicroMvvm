#region Copyright 2016 D-Haven.org

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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using DHaven.MicroMvvm;
using DHaven.MicroMvvm.Dialog;

namespace DHaven.MicroMVVM.Universal
{
    public class Locator
    {
        private const string ViewModel = "ViewModel";
        private const string View = "View";

        private static readonly Type[] FrameworkTypes =
        {
            typeof(IViewModel),
            typeof(IValueConverter),
            typeof(ICommand),
            typeof(UserControl)
        };

        private static readonly Dictionary<string, Type> DiscoveredTypes = new Dictionary<string, Type>();

        static Locator()
        {
            foreach (var assembly in GetAssemblyList().Result)
            {
                ScanForFrameworkTypes(assembly);
            }
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
                    {
                        shortClassName = viewModelType.Name.Substring(0, viewModelType.Name.Length - ViewModel.Length)
                                         + View;
                    }
                }

                if (dataContext is UserControl) // In UWP, Page is a UserControl
                {
                    var viewType = dataContext.GetType();

                    if (viewType.Name.EndsWith(View))
                    {
                        shortClassName = viewType.Name.Substring(0, viewType.Name.Length - View.Length) + ViewModel;
                    }
                }

                if (dataContext is Message)
                {
                    shortClassName = dataContext.GetType().Name + View;
                }

                return CreateObjectFor(shortClassName ?? dataContext?.GetType().Name);
            }
        }

        /// <summary>
        /// This is a hacky workaround since we can't use Assembly.GetReferencedResources()
        /// to scan all relevant assemblies.
        /// </summary>
        /// <returns></returns>
        private static async Task<IEnumerable<Assembly>> GetAssemblyList()
        {
            var assemblies = new List<Assembly>();

            var files = await Package.Current.InstalledLocation.GetFilesAsync();
            if (files == null)
            {
                return assemblies;
            }

            foreach (var file in files.Where(file =>
                file.FileType.Equals(".dll", StringComparison.OrdinalIgnoreCase)
                || file.FileType.Equals(".exe", StringComparison.OrdinalIgnoreCase)))
            {
                try
                {
                    assemblies.Add(Assembly.Load(new AssemblyName(file.DisplayName)));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return assemblies;
        }

        private static void ScanForFrameworkTypes(Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes.Where(IsFrameworkType))
            {
                DiscoveredTypes.Add(type.Name, type.AsType());
            }
        }

        private static bool IsFrameworkType(TypeInfo type)
        {
            return FrameworkTypes.Any(ft => ft.IsAssignableFrom(type.AsType()));
        }

        private static object CreateObjectFor(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return null;
            }

            Type targetType;
            if (DiscoveredTypes.TryGetValue(className, out targetType))
            {
                try
                {
                    var constructor = targetType.GetConstructor(new Type[0]);
                    return constructor?.Invoke(new object[0]);
                }
                catch
                {
                    // Ignore.  Couldn't create, so we swallow the exception and return null.
                }
            }

            return null;
        }
    }
}