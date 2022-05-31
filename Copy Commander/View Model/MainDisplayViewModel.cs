using Copy_Commander.Model;
using System.Windows.Input;
using System.IO;

namespace Copy_Commander.View_Model
{
    internal class MainDisplayViewModel : PropertyChangedNotify
    {
        private DirectoryListViewModel _leftListViewModel;
        private DirectoryListViewModel _rightListViewModel;
        private string _copyDirection;
        private bool _allowCopy;
        private DirectoryItemModel _lastSelected;
        private bool _lastSelectedLeft;

        private ICommand _copyCommand;
        private ICommand _displayFilesHelpCommand;
        private ICommand _displayOpHelpCommand;
        private ICommand _setLeftPriorityCommand;
        private ICommand _setRightPriorityCommand;
        private ICommand _disallowCopyCommand;

        #region Properties
        public DirectoryListViewModel LeftListViewModel
        {
            get
            {
                if (_leftListViewModel == null)
                    LeftListViewModel = new DirectoryListViewModel();
                return _leftListViewModel;
            }
            set
            {
                if (_leftListViewModel != value)
                {
                    _leftListViewModel = value;
                    OnPropertyChanged("LeftListModel");
                }
            }
        }

        public DirectoryListViewModel RightListViewModel
        {
            get
            {
                if (_rightListViewModel == null)
                    RightListViewModel = new DirectoryListViewModel();
                return _rightListViewModel;
            }
            set
            {
                if (value != _rightListViewModel)
                {
                    _rightListViewModel = value;
                    OnPropertyChanged("RightListModel");
                }
            }
        }

        public string CopyDirection
        {
            get
            {
                if (_copyDirection == null)
                    _copyDirection = "X";
                return _copyDirection;
            }
            set
            {
                if (value != _copyDirection)
                {
                    _copyDirection = value;
                    OnPropertyChanged("CopyDirection");
                }
            }
        }

        public bool AllowCopy
        {
            get
            {
                if (_allowCopy == null)
                    AllowCopy = false;
                return _allowCopy;
            }
            set
            {
                if (value != AllowCopy)
                {
                    _allowCopy = value;
                    OnPropertyChanged("AllowCopy");
                }
            }
        }

        #region ICommands
        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand == null)
                    _copyCommand = new RelayCommand(param => Copy());
                return _copyCommand;
            }
        }

        public ICommand DisplayFilesHelpCommand
        {
            get
            {
                if (_displayFilesHelpCommand == null)
                    _displayFilesHelpCommand = new RelayCommand(param => DisplayHelp("HD\tUkryty Folder\nD\tFolder\nF\tPlik\nSF\tPlik Systemowy", "Wyjaśnienie Plików"));
                return _displayFilesHelpCommand;
            }
        }
        public ICommand DisplayOpHelpCommand
        {
            get
            {
                if (_displayOpHelpCommand == null)
                    _displayOpHelpCommand = new RelayCommand(param => DisplayHelp("Strzałka na środkowym przycisku wskazuje kierunkek kopii.\n\nW wypadku braku możliwości wykonanie kopii, zostanie wyświetlona wiadomość.", "Wyjaśnienie Operacji"));
                return _displayOpHelpCommand;
            }
        }

        public ICommand SetLeftPriorityCommand
        {
            get
            {
                if (_setLeftPriorityCommand == null)
                    _setLeftPriorityCommand = new RelayCommand(param => SetPriority(true));
                return _setLeftPriorityCommand;
            }
        }

        public ICommand SetRightPriorityCommand
        {
            get
            {
                if (_setRightPriorityCommand == null)
                    _setRightPriorityCommand = new RelayCommand(param => SetPriority(false));
                return _setRightPriorityCommand;
            }
        }

        public ICommand DisallowCopyCommand
        {
            get
            {
                if (_disallowCopyCommand == null)
                    _disallowCopyCommand = new RelayCommand(param => DisallowCopy());
                return _disallowCopyCommand;
            }
        }
        #endregion
#endregion


        private void Copy()
        {
            if (_lastSelected.Type == "" || _lastSelected.Type == null)
            {
                DisplayHelp("Wrong item selection", "Copy source error");
                return;
            }
            string _sourcePath;
            string _destPath;
            if (_lastSelectedLeft)
            {
                _sourcePath = LeftListViewModel.FullPath + '\\' + _lastSelected.Name;
                _destPath = RightListViewModel.FullPath + "\\" + _lastSelected.Name;
            }
            else
            {
                _sourcePath = RightListViewModel.FullPath + '\\' + _lastSelected.Name;
                _destPath= LeftListViewModel.FullPath + "\\" + _lastSelected.Name;
            }

            if (_lastSelected.Type == "D" || _lastSelected.Type == "HD")
            {
                DisplayHelp("Directory copying not supported in current release", "Copy Error");
            }
            else
            {
                try
                {
                    File.Copy(_sourcePath, _destPath, true);
                }
                catch
                {
                    DisplayHelp("Access Denied", "Copy Error");
                    return;
                }
            }

        }

        private void DisplayHelp(string text, string title)
        {
            System.Windows.MessageBox.Show(text, title);
        }

        private void SetPriority(bool left) // T - Left LB, F - Right LB
        {
            if (left)
            {
                if (LeftListViewModel.SelectedItem != null)
                {
                    if (LeftListViewModel.SelectedItem.Type == "")
                    {
                        CopyDirection = "X";
                        AllowCopy = false;
                    }
                    else
                    {
                        CopyDirection = ">>>";
                        AllowCopy = true;
                        _lastSelected = LeftListViewModel.SelectedItem;
                    }
                }
            }
            else
            {
                if (RightListViewModel.SelectedItem != null)
                {
                    if (RightListViewModel.SelectedItem.Type == "")
                    {
                        CopyDirection = "X";
                        AllowCopy = false;
                    }
                    else
                    {
                        CopyDirection = "<<<";
                        AllowCopy = true;
                        _lastSelected = RightListViewModel.SelectedItem;
                    }
                }
            }
        }

        private void DisallowCopy()
        {
            if (_lastSelected.Type == "D" || _lastSelected.Type == "HD" || _lastSelected.Type == "")
            {
                CopyDirection = "X";
                AllowCopy = false;
            }
        }
    }
}
