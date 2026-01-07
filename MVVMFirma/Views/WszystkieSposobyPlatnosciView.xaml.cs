using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieSposobyPlatnosciView : UserControl
    {
        public WszystkieSposobyPlatnosciView()
        {
            InitializeComponent();
            DataContext = new WszystkieSposobyPlatnosciViewModel();
        }
    }
}
