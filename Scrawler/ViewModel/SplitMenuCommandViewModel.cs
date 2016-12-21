using System;

namespace Scrawler.ViewModel
{
    public class SplitMenuCommandViewModel : CommandViewModel
    {
        private string _iconPath;
        private bool _isExpanded;

        public SplitMenuCommandViewModel(
            string label,
            string iconPath,
            Action<object> execute) 
            : base(label, execute)
        {
            _iconPath = iconPath;
            _isExpanded = false;
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
    }
}
