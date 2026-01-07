using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class NowyTowarViewModel : JedenViewModel<Towar>
    {
        #region Constructor

        public NowyTowarViewModel()
            : base()
        {
            base.DisplayName = "Nowy towar";

            
            Item = new Towar();
        }

        #endregion

        #region Helpers

        public override void Save()
        {

        }

        #endregion
    }
}
