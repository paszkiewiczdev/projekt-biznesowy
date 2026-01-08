using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;

namespace MVVMFirma.ViewModels.Abstract
{
    public abstract class WszystkieViewModel<T> : DataBaseViewModel
    {
        #region Fields

        private ObservableCollection<T> _List;
        private List<T> _allItems;

        private BaseCommand _LoadCommand;
        private BaseCommand _ApplyFilterCommand;
        private BaseCommand _ClearFilterCommand;

        private string _FilterText;
        private string _SelectedSortField;
        private bool _SortDescending;
        private ObservableCollection<string> _SortOptions;

        #endregion

        #region Properties

        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                {
                    load();
                }
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);
            }
        }

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                OnPropertyChanged(() => FilterText);
            }
        }

        public ObservableCollection<string> SortOptions
        {
            get => _SortOptions;
            protected set
            {
                _SortOptions = value;
                OnPropertyChanged(() => SortOptions);
            }
        }

        public string SelectedSortField
        {
            get => _SelectedSortField;
            set
            {
                _SelectedSortField = value;
                OnPropertyChanged(() => SelectedSortField);
            }
        }

        public bool SortDescending
        {
            get => _SortDescending;
            set
            {
                _SortDescending = value;
                OnPropertyChanged(() => SortDescending);
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return GetCommand(_LoadCommand, load);
            }
        }

        public ICommand ApplyFilterCommand
        {
            get
            {
                return GetCommand(_ApplyFilterCommand, ApplyFilterAndSort);
            }
        }

        public ICommand ClearFilterCommand
        {
            get
            {
                return GetCommand(_ClearFilterCommand, ClearFilter);
            }
        }

        #endregion

        #region Constructor

        protected WszystkieViewModel()
            : base()
        {
            FilterText = string.Empty;
            SortDescending = false;
            SortOptions = new ObservableCollection<string>();
            SelectedSortField = null;
        }

        #endregion

        #region Helpers

        protected void SetSortOptions(IEnumerable<string> options)
        {
            SortOptions = new ObservableCollection<string>(options ?? Enumerable.Empty<string>());
            EnsureDefaultSortField();
        }

        public virtual void load()
        {
            var data = LoadData() ?? Enumerable.Empty<T>();
            _allItems = data.ToList();
            EnsureDefaultSortField();
            ApplyFilterAndSort();
        }

        protected abstract IEnumerable<T> LoadData();

        protected abstract bool MatchesFilter(T item, string filterText);

        protected abstract IOrderedEnumerable<T> ApplySort(IEnumerable<T> query, string sortField, bool descending);

        protected void ApplyFilterAndSort()
        {
            if (_allItems == null)
                return;

            IEnumerable<T> query = _allItems;

            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                var filter = FilterText.Trim();
                query = query.Where(item => MatchesFilter(item, filter));
            }

            EnsureDefaultSortField();

            var sorted = ApplySort(query, SelectedSortField, SortDescending) ?? query.OrderBy(x => 0);

            List = new ObservableCollection<T>(sorted);
        }

        protected void ClearFilter()
        {
            FilterText = string.Empty;
            SortDescending = false;
            EnsureDefaultSortField(reset: true);
            ApplyFilterAndSort();
        }

        private void EnsureDefaultSortField(bool reset = false)
        {
            if (SortOptions == null || SortOptions.Count == 0)
                return;

            if (reset || string.IsNullOrWhiteSpace(SelectedSortField))
                SelectedSortField = SortOptions[0];
        }

        #endregion
    }
}
