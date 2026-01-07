using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieStatusyFakturView : UserControl
    {
        public WszystkieStatusyFakturView()
        {
            InitializeComponent();
            DataContext = new WszystkieStatusyFakturViewModel();
        }
    }
}
