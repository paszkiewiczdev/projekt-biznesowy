using MVVMFirma.Models;

namespace MVVMFirma.ViewModels
{
    public class DataBaseViewModel : WorkspaceViewModel
    {
        #region Fields
        protected FakturyEntities fakturyEntities;
        #endregion

        #region Constructor
        public DataBaseViewModel()
            : base()
        {
            this.fakturyEntities = new FakturyEntities();
        }
        #endregion
    }
}