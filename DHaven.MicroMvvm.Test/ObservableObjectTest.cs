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
using NUnit.Framework;

namespace DHaven.MicroMvvm.Test
{
    [TestFixture]
    public class ObservableObjectTest
    {
        public class TestObservable : ObservableObject
        {
            public string Name
            {
                get { return GetValue<string>(nameof(Name)); }
                set { SetValue(nameof(Name), value); }
            }

            public double X
            {
                get { return GetValue<int>(nameof(X)); }
                set { SetValue(nameof(X), value); }
            }
        }

        [Test]
        public void ObservableObjectPropertiesBehaveNormally()
        {
            var testObject = new TestObservable();

            Assert.That(testObject.Name, Is.Null);

            testObject.Name = "Test String";

            Assert.That(testObject.Name, Is.EqualTo("Test String"));
        }

        [Test]
        public void ObservableObjectPropertiesRaisePropertyChanged()
        {
            var propertiesChanged = new List<string>();

            var testObject = new TestObservable();
            testObject.PropertyChanged += (source, args) => propertiesChanged.Add(args.PropertyName);

            testObject.Name = "Another String";
            testObject.X = Math.PI;

            Assert.That(propertiesChanged.Count, Is.EqualTo(2));
            Assert.That(propertiesChanged, Contains.Item(nameof(testObject.Name)));
            Assert.That(propertiesChanged, Contains.Item(nameof(testObject.X)));
        }
    }
}