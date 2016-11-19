using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Windows.UI.Xaml;
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
            typeof(UserControl),
            typeof(Page)
        };

        private static readonly Dictionary<string, Type> DiscoveredTypes = new Dictionary<string, Type>();

        static Locator()
        {
            var searchedAssemblies = new List<string>();
            var startingAssembly = Application.Current.GetType().GetTypeInfo().Assembly;

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
                    {
                        shortClassName = viewModelType.Name.Substring(0, viewModelType.Name.Length - ViewModel.Length) + View;
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

        private static void ScanForFrameworkTypes(Assembly assembly, ref List<string> searchedAssemblies)
        {
            var assemblyName = assembly.GetName().ToString();
            searchedAssemblies.Add(assemblyName);

            foreach (var type in assembly.DefinedTypes.Where(IsFrameworkType))
            {
                DiscoveredTypes.Add(type.Name, type.AsType());
            }

            foreach (var referencedName in assembly.GetReferencedAssemblies())
            {
                if (!searchedAssemblies.Contains(referencedName.ToString()))
                {
                    var dependency = Assembly.Load(referencedName);
                    ScanForFrameworkTypes(dependency, ref searchedAssemblies);
                }
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
