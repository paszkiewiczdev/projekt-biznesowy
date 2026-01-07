using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models;

namespace MVVMFirma.ViewModels.Abstract
{
    public abstract class JedenViewModel<T> : DataBaseViewModel
        where T : class
    {
        #region Fields

        protected T _Item;
        private BaseCommand _SaveCommand;

        #endregion

        #region Properties

        public T Item
        {
            get { return _Item; }
            set
            {
                _Item = value;
                OnPropertyChanged(() => Item);
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return GetCommand(_SaveCommand, Save);
            }
        }

        #endregion

        #region Constructor

        protected JedenViewModel()
            : base()
        {
        }

        #endregion

        #region Helpers

        public abstract void Save();

        #endregion
    }
}
