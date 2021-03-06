﻿using Scrawler.ViewModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Scrawler.View
{
    public sealed partial class PageGridView : UserControl
    {
        public PageGridView()
        {
            this.InitializeComponent();
            DataContextChanged += PageGridView_DataContextChanged;
            SizeChanged += PageGridView_SizeChanged;
        }

        private void PageGridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var vm = DataContext as PageGridViewModel;
            if (vm != null)
            {
                SetPageSizes(vm);
            }
        }

        private void PageGridView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var vm = DataContext as PageGridViewModel;
            if (vm != null)
            {
                SetPageSizes(vm);
            }
        }

        private void SetPageSizes(PageGridViewModel vm)
        {
            double itemWidth = Math.Max((ActualWidth / 5) - 10, 50);
            double itemHeight = itemWidth;
            foreach (var page in vm.Pages)
            {
                page.Width = itemWidth;
                page.Height = itemHeight;
            }
        }
    }
}
