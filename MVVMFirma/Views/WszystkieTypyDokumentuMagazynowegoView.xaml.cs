using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieTypyDokumentuMagazynowegoView : UserControl
    {
        public WszystkieTypyDokumentuMagazynowegoView()
        {
            InitializeComponent();
            DataContext = new WszystkieTypyDokumentuMagazynowegoViewModel();
        }
    }
}
