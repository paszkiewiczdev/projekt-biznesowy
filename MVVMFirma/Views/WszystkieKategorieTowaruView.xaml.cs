using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieKategorieTowaruView : UserControl
    {
        public WszystkieKategorieTowaruView()
        {
            InitializeComponent();
            DataContext = new WszystkieKategorieTowaruViewModel();
        }
    }
}
