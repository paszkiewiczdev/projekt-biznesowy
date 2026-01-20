using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MVVMFirma.Models;

namespace MVVMFirma.Helper
{
    public class VatIdToValueConverter : IValueConverter
    {
        // UWAGA: WPF nie ma dostępu do Twojego VM wprost, więc pobieramy z DB "na żądanie".
        // Dla listy Towarów to będzie działało OK na zaliczenie.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";

            if (!int.TryParse(value.ToString(), out var idVat)) return "";

            try
            {
                using var db = new FakturyEntities(); // jeśli u Ciebie context nazywa się inaczej, podmień
                var vat = db.StawkaVat.FirstOrDefault(v => v.IdStawkiVat == idVat);
                if (vat == null) return "";

                return $"{vat.Wartosc:0.##}%";
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Nie potrzebujemy ConvertBack, bo w tabeli jest IsReadOnly=True
            return Binding.DoNothing;
        }
    }
}
