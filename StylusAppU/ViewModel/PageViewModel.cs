﻿using System;
using StylusAppU.Data.Data;
using Utils.ViewModel;
using Windows.Storage;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Media.Imaging;
using StylusAppU.Data.Serialization;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace StylusAppU.ViewModel
{
    public class PageViewModel : ViewModelBase
    {
        private Page _page;
        private Brush _backgroundBrush;
        private NotebookSerializer _notebookSerializer;

        public PageViewModel(Page page, NotebookSerializer notebookSerializer)
        {
            _page = page;
            _notebookSerializer = notebookSerializer;
            //LoadBackground();
        }

        public double Width
        {
            get { return _page.Width; }
            set
            {
                _page.Width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get { return _page.Height; }
            set
            {
                _page.Height = value;
                OnPropertyChanged();
            }
        }

        public Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
            set
            {
                _backgroundBrush = value;
                OnPropertyChanged();
            }
        }

        public InkStrokeContainer StrokeContainer
        {
            get { return _page.StrokeContainer; }
            set
            {
                _page.StrokeContainer = value;
                OnPropertyChanged();
            }
        }
    }
}
