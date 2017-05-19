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

using Xunit;

namespace DHaven.MicroMvvm.Test
{
    public class GivenViewModel
    {
        private class TestViewModel<T> : ViewModel<T>
        {
            public TestViewModel() : base(default(T))
            {
            }

            public TestViewModel(T model) : base(model)
            {
            }
        }

        [Fact]
        public void ShouldAllowInitialObjectInConstructor()
        {
            const string model = "A silly string, not practical, but good enough";
            var viewModel = new TestViewModel<string>(model);

            Assert.Equal(model, viewModel.Model);
        }

        [Fact]
        public void ShouldNotifyModelChanged()
        {
            var modelChanged = false;
            var viewModel = new TestViewModel<int>(2016);
            viewModel.PropertyChanged += (source, args) => modelChanged = args.PropertyName == nameof(viewModel.Model);

            viewModel.Model = 288;

            Assert.True(modelChanged);
        }

        [Fact]
        public void ShouldNotRequireModelInConstructor()
        {
            var viewModel = new TestViewModel<GivenObservableObject.TestObservable>();

            Assert.Null(viewModel.Model);
        }
    }
}