using Xunit;

namespace DHaven.MicroMvvm.Test
{
    public class GivenViewModel
    {
        private class TestViewModel<T> : ViewModel<T>
        {
            public TestViewModel() : base(default(T)) { }

            public TestViewModel(T model) : base(model) { }
        }

        [Fact]
        public void ShouldNotRequireModelInConstructor()
        {
            var viewModel = new TestViewModel<GivenObservableObject.TestObservable>();

            Assert.Null(viewModel.Model);
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
    }
}
