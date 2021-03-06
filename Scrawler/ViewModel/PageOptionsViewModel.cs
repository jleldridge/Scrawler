﻿using System;
using Microsoft.Graphics.Canvas;
using Scrawler.Data.Data;
using Scrawler.Data.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Utils.Commands;
using Utils.ViewModel;
using Windows.Storage.Pickers;
using System.ComponentModel;

namespace Scrawler.ViewModel
{
    public class PageOptionsViewModel : ViewModelBase
    {
        private double _height, _width;
        private BackgroundType _selectedType;
        private BackgroundViewModelBase _backgroundDataViewModel;
        private NotebookViewModel _notebook;
        private ICommand _saveBackgroundCommand;
        private ICommand _setBackgroundCommand;

        public PageOptionsViewModel(PageViewModel page, NotebookViewModel notebook)
        {
            Width = page.Width;
            Height = page.Height;
            var backgroundData = page.BackgroundViewModel.BackgroundData.GetDeepCopy();
            _notebook = notebook;
            
            if (backgroundData is SolidBackground)
            {
                _backgroundDataViewModel = new SolidBackgroundViewModel(backgroundData as SolidBackground);
                _selectedType = BackgroundType.Solid;
            }
            else if (backgroundData is GridLineBackground)
            {
                _backgroundDataViewModel = new GridLineBackgroundViewModel(backgroundData as GridLineBackground);
                _selectedType = BackgroundType.Grid;
            }
            else if (backgroundData is ImageBackground)
            {
                _backgroundDataViewModel = new ImageBackgroundViewModel(backgroundData as ImageBackground);
                _selectedType = BackgroundType.Image;
            }
        }

        public List<BackgroundType> BackgroundTypes
        {
            get
            {
                return new List<BackgroundType>()
                {
                    BackgroundType.Solid,
                    BackgroundType.Grid,
                    BackgroundType.Image
                };
            }
        }

        public BackgroundType SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (value != _selectedType)
                {    
                    _selectedType = value;
                    OnPropertyChanged();

                    switch (_selectedType)
                    {
                        case BackgroundType.Solid:
                            BackgroundDataViewModel = new SolidBackgroundViewModel(new SolidBackground());
                            break;
                        case BackgroundType.Grid:
                            BackgroundDataViewModel = new GridLineBackgroundViewModel(new GridLineBackground());
                            break;
                        case BackgroundType.Image:
                            BackgroundDataViewModel = new ImageBackgroundViewModel(new ImageBackground());
                            break;
                    }
                }
            }
        }

        public BackgroundViewModelBase BackgroundDataViewModel
        {
            get { return _backgroundDataViewModel; }
            set
            {
                _backgroundDataViewModel = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public List<BackgroundBase> SavedBackgrounds
        {
            get { return _notebook.SavedPageBackgrounds; }
            set 
            { 
                _notebook.SavedPageBackgrounds = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveBackgroundCommand
        {
            get { return _saveBackgroundCommand ?? (_saveBackgroundCommand = new RelayCommand(_ => SaveBackground())); }
        }

        public ICommand SetBackgroundCommand
        {
            get { return _setBackgroundCommand ?? (_setBackgroundCommand = new RelayCommand(SetBackground)); }
        }

        public void SaveBackground()
        {
            var backgroundToBeSaved = BackgroundDataViewModel.BackgroundData.GetDeepCopy();
            SavedBackgrounds.Add(backgroundToBeSaved);
            SavedBackgrounds = new List<BackgroundBase>(SavedBackgrounds);
        }

        public void SetBackground(object param)
        {
            var backgroundData = param as BackgroundBase;
            if (backgroundData != null)
            {
                var newBackgroundData = backgroundData.GetDeepCopy();
                if (newBackgroundData is SolidBackground)
                {
                    _selectedType = BackgroundType.Solid;
                    BackgroundDataViewModel = new SolidBackgroundViewModel(newBackgroundData as SolidBackground);
                }
                else if (newBackgroundData is GridLineBackground)
                {
                    _selectedType = BackgroundType.Grid;
                    BackgroundDataViewModel = new GridLineBackgroundViewModel(newBackgroundData as GridLineBackground);
                }
                else if (newBackgroundData is ImageBackground)
                {
                    _selectedType = BackgroundType.Image;
                    BackgroundDataViewModel = new ImageBackgroundViewModel(newBackgroundData as ImageBackground);
                }
                OnPropertyChanged("SelectedType");
            }
        }

        public async Task LoadImageForBackground()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();
            if (file == null) return;

            // Ensure the stream is disposed once the image is loaded
            using (var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                var device = CanvasDevice.GetSharedDevice();
                var image = await CanvasBitmap.LoadAsync(device, fileStream);
                var backgroundData = new ImageBackground();
                backgroundData.Image = image;

                // make sure we have image as our background type
                _selectedType = BackgroundType.Image;
                OnPropertyChanged("SelectedType");

                BackgroundDataViewModel = new ImageBackgroundViewModel(backgroundData);
            }
        }

        public void SetPageToImageSize()
        {
            var imageBackground = BackgroundDataViewModel.BackgroundData as ImageBackground;
            if (imageBackground != null)
            {
                Width = imageBackground.Image.SizeInPixels.Width;
                Height = imageBackground.Image.SizeInPixels.Height;
            }
        }
    }

    public enum BackgroundType
    {
        Solid,
        Grid,
        Image
    }
}
