using System.Windows.Controls;
using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class WszystkieJednostkiMiaryView : UserControl
    {
        public WszystkieJednostkiMiaryView()
        {
            InitializeComponent();
            DataContext = new WszystkieJednostkiMiaryViewModel();
        }
    }
}
