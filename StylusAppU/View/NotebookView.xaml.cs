using System;
using System.ComponentModel;
using StylusAppU.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace StylusAppU.View
{
    public sealed partial class NotebookView : UserControl
    {
        public NotebookView()
        {
            this.InitializeComponent();
            DataContextChanged += NotebookView_DataContextChanged;
        }

        public NotebookViewModel ViewModel
        {
            get { return DataContext as NotebookViewModel; }
        }

        private void NotebookView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs e)
        {
            var vm = e.NewValue as NotebookViewModel;
            if (vm != null)
            {
                vm.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Zoom":
                    if (ViewModel != null && PageScroller.ZoomFactor != ViewModel.Zoom / 100f)
                    {
                        PageScroller.ChangeView(null, null, ViewModel.Zoom / 100f);
                    }
                    break;
            }
        }

        private void PageScroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (ViewModel.Zoom != PageScroller.ZoomFactor)
            {
                ViewModel.Zoom = PageScroller.ZoomFactor * 100;
            }
        }
    }
}
