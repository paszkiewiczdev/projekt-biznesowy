using System.Collections.ObjectModel;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;

namespace MVVMFirma.ViewModels.Abstract
{
    public abstract class WszystkieViewModel<T> : DataBaseViewModel
    {
        #region Fields

        private ObservableCollection<T> _List;
        private BaseCommand _LoadCommand;

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

        public ICommand LoadCommand
        {
            get
            {
                return GetCommand(_LoadCommand, load);
            }
        }

        #endregion

        #region Constructor

        protected WszystkieViewModel()
            : base()
        {
        }

        #endregion

        #region Helpers

        public abstract void load();

        #endregion
    }
}