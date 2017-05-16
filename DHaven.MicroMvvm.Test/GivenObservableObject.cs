using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DHaven.MicroMvvm.Test
{
    public class GivenObservableObject
    {
        public class TestObservable : ObservableObject
        {
            public string Name
            {
                get => GetValue<string>(nameof(Name));
                set => SetValue(nameof(Name), value);
            }

            public double X
            {
                get => GetValue<int>(nameof(X));
                set => SetValue(nameof(X), value);
            }
        }

        [Fact]
        public void PropertiesShouldBehaveNormally()
        {
            var testObject = new TestObservable();

            Assert.Null(testObject.Name);

            testObject.Name = "Test String";

            Assert.Equal("Test String", testObject.Name);
        }

        [Fact]
        public void PropertiesShouldRaisePropertyChanged()
        {
            var propertiesChanged = new List<string>();
            var testObject = new TestObservable();
            testObject.PropertyChanged += (source, args) => propertiesChanged.Add(args.PropertyName);

            testObject.Name = "Another String";
            testObject.X = Math.PI;

            Assert.Equal(2, propertiesChanged.Count);
            Assert.Contains(nameof(testObject.Name), propertiesChanged);
            Assert.Contains(nameof(testObject.X), propertiesChanged);
        }
    }
}
