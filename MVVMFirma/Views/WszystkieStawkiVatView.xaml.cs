using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieStawkiVatView : UserControl
    {
        public WszystkieStawkiVatView()
        {
            InitializeComponent();
            DataContext = new WszystkieStawkiVatViewModel();
        }
    }
}
