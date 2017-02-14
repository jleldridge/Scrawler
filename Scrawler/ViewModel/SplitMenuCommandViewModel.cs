using System;
using Windows.UI.Xaml.Data;

namespace Scrawler.ViewModel
{
    public class SplitMenuCommandViewModel : CommandViewModel
    {
        private string _iconPath;
        private bool _isExpanded;
        private bool _visible;

        public SplitMenuCommandViewModel(
            string label,
            string iconPath,
            Action<object> execute,
            bool startsVisible = true) 
            : base(label, execute)
        {
            _iconPath = iconPath;
            _isExpanded = false;
            Visible = startsVisible;
        }

        public string IconPath
        {
            get { return _iconPath; }
            set
            {
                _iconPath = value;
                OnPropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnPropertyChanged();
            }
        }
    }
}
