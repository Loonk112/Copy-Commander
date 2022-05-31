using System.Collections.Generic;

namespace Copy_Commander.Model
{
    internal class DirectoryListModel : PropertyChangedNotify
    {
        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged("Path");
                }
            }
        }

        private List<DirectoryItemModel> _items;

        public List<DirectoryItemModel> Items
        {
            get { return _items; }
            set
            {
                if (value != _items)
                {
                    _items = value;
                    OnPropertyChanged("Items");
                }
            }
        }
    }
}
