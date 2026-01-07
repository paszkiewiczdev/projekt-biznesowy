using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieMagazynyView : UserControl
    {
        public WszystkieMagazynyView()
        {
            InitializeComponent();
            DataContext = new WszystkieMagazynyViewModel();
        }
    }
}
