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

using System;
using System.Collections.Generic;
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