using System;
using System.Windows.Input;
using MVVMFirma.Models;
using MVVMFirma.ViewModels.Abstract;
using MVVMFirma.Helper;

namespace MVVMFirma.ViewModels
{
    public class NowySposobPlatnosciViewModel : WorkspaceViewModel
    {
        private readonly FakturyEntities _db;
        private readonly SposobPlatnosci _nowySposobPlatnosci;

        #region Właściwości bindowane

        public string Nazwa
        {
            get => _nowySposobPlatnosci.Nazwa;
            set
            {
                if (_nowySposobPlatnosci.Nazwa != value)
                {
                    _nowySposobPlatnosci.Nazwa = value;
                    OnPropertyChanged(() => Nazwa);
                }
            }
        }

        public string Opis
        {
            get => _nowySposobPlatnosci.Opis;
            set
            {
                if (_nowySposobPlatnosci.Opis != value)
                {
                    _nowySposobPlatnosci.Opis = value;
                    OnPropertyChanged(() => Opis);
                }
            }
        }

        public bool CzyAktywny
        {
            get => _nowySposobPlatnosci.CzyAktywny;
            set
            {
                if (_nowySposobPlatnosci.CzyAktywny != value)
                {
                    _nowySposobPlatnosci.CzyAktywny = value;
                    OnPropertyChanged(() => CzyAktywny);
                }
            }
        }

        #endregion

        #region Komendy

        public ICommand ZapiszCommand { get; }
        public ICommand AnulujCommand { get; }

        #endregion

        public NowySposobPlatnosciViewModel()
        {
            base.DisplayName = "Nowy sposób płatności";

            _db = new FakturyEntities();

            _nowySposobPlatnosci = new SposobPlatnosci
            {
                CzyAktywny = true
            };

            ZapiszCommand = new BaseCommand(Zapisz);
            AnulujCommand = new BaseCommand(Anuluj);
        }

        private void Zapisz()
        {
            _db.SposobPlatnosci.Add(_nowySposobPlatnosci);
            _db.SaveChanges();
            OnRequestClose();
        }

        private void Anuluj()
        {
            OnRequestClose();
        }
    }
}
