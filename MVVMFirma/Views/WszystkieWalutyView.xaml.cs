using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieWalutyView : UserControl
    {
        public WszystkieWalutyView()
        {
            InitializeComponent();
            DataContext = new WszystkieWalutyViewModel();
        }
    }
}
