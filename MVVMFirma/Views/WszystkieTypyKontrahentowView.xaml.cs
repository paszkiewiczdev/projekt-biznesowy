using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieTypyKontrahentowView : UserControl
    {
        public WszystkieTypyKontrahentowView()
        {
            InitializeComponent();
            DataContext = new WszystkieTypyKontrahentowViewModel();
        }
    }
}
