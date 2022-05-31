using Copy_Commander.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Copy_Commander.View_Model
{
    internal class DirectoryListViewModel : PropertyChangedNotify
    {
        private List<string> _drives;

        private DirectoryListModel _dir;

        private DirectoryItemModel _selectedItem;

        private ICommand _loadDrivesCommand;
        private ICommand _updateCommand;
        private ICommand _moveCommand;

        public DirectoryListViewModel()
        {
            _dir = new DirectoryListModel();
            LoadDrives();
            FullPath = Drives[0];
        }

        #region Properties
        public List<string> Drives
        {
            get { return _drives; }
            set
            {
                if (_drives != value)
                {
                    _drives = value;
                    OnPropertyChanged("Drives");
                }
            }
        }

        public string FullPath
        {
            get { return _dir.Path; }
            set
            {
                if (value != _dir.Path)
                {
                    _dir.Path = value;
                    OnPropertyChanged("FullPath");
                    OnPropertyChanged("RelativePath");
                    Update();
                }
            }
        }

        public string CurrentDrive
        {
            get { return FullPath.Split('\\')[0] + '\\'; }
            set
            {
                if (value != FullPath)
                {
                    _dir.Path = value;
                    OnPropertyChanged("CurrentDrive");
                }
            }
        }

        public string RelativePath
        {
            get { return String.Join('\\', _dir.Path.Split('\\').Skip(2)); }
        }

        public List<DirectoryItemModel> ItemsList
        {
            get { return _dir.Items; }
            set
            {
                if (_dir.Items != value)
                {
                    _dir.Items = value;
                    OnPropertyChanged("ItemsList");
                }
            }
        }

        public DirectoryItemModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        #region ICommands
        public ICommand LoadDrivesCommand
        {
            get
            {
                if (_loadDrivesCommand == null)
                    _loadDrivesCommand = new RelayCommand(param => LoadDrives());
                return _loadDrivesCommand;
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                    _updateCommand = new RelayCommand(param => Update());
                return _updateCommand;
            }
        }

        public ICommand MoveCommand
        {
            get
            {
                if (_moveCommand == null)
                    _moveCommand = new RelayCommand(param => Move());
                return _moveCommand;
            }
        }
        #endregion
        #endregion

        #region Functions
        private void LoadDrives()
        {
            Drives = Directory.GetLogicalDrives().ToList();
        }

        private void Update()
        {
            List<DirectoryItemModel> L = new List<DirectoryItemModel>();
            if (FullPath != CurrentDrive)
            {
                DirectoryItemModel R = new DirectoryItemModel();
                R.Name = "...";
                R.Type = "";
                L.Add(R);
            }
            try
            {
                foreach (string name in Directory.GetDirectories(FullPath))
                {
                    DirectoryItemModel I = new DirectoryItemModel();
                    I.Name = name.Split('\\').Last();
                    if (I.Name[0] == '$')
                        I.Type = "HD";
                    else
                        I.Type = "D";
                    L.Add(I);
                }
            }
            catch { }

            try
            {
                foreach (string name in Directory.GetFiles(FullPath))
                {
                    DirectoryItemModel I = new DirectoryItemModel();
                    I.Name = name.Split('\\').Last();
                    if (I.Name.Split('.').Last() == "sys")
                        I.Type = "SF";
                    else
                        I.Type = "F";
                    L.Add(I);
                }
            }
            catch { }

            ItemsList = L;
        }

        private void Move()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.Type == "")
                    FullPath = String.Join('\\', _dir.Path.Split('\\').SkipLast(1));
                else if ((SelectedItem.Type == "D") || (SelectedItem.Type == "HD"))
                    FullPath = _dir.Path + '\\' + SelectedItem.Name;
                Update();
            }
        }
        #endregion

    }
}
