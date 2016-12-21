﻿using System;
using System.ComponentModel;
using Scrawler.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Scrawler
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            DataContext = new MainViewModel();
            this.InitializeComponent();
        }

        public MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
    }
}
