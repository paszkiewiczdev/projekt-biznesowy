using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieStatusyZamowienView : UserControl
    {
        public WszystkieStatusyZamowienView()
        {
            InitializeComponent();
            DataContext = new WszystkieStatusyZamowienViewModel();
        }
    }
}
