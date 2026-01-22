using MVVMFirma.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace MVVMFirma.Helper
{
    public static class InvoicePdfGenerator
    {
        public static void Generate(
            FakturaSprzedazy faktura,
            IReadOnlyCollection<PozycjaFakturySprzedazy> pozycje,
            string filePath)
        {
            if (faktura == null)
                throw new ArgumentNullException(nameof(faktura));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Ścieżka zapisu PDF nie może być pusta.", nameof(filePath));

            var culture = CultureInfo.GetCultureInfo("pl-PL");
            using var document = new PdfDocument
            {
                Info =
                {
                    Title = $"Faktura {faktura.Numer ?? faktura.IdFakturySprzedazy.ToString(culture)}"
                }
            };

            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var titleFont = new XFont("Arial", 16, XFontStyleEx.Bold);
            var headerFont = new XFont("Arial", 12, XFontStyleEx.Bold);
            var font = new XFont("Arial", 11, XFontStyleEx.Regular);

            var margin = 40d;
            var y = margin;

            void DrawLine(string text, XFont fontToUse)
            {
                gfx.DrawString(
                    text,
                    fontToUse,
                    XBrushes.Black,
                    new XRect(
                        XUnit.FromPoint(margin),
                        XUnit.FromPoint(y),
                        XUnit.FromPoint(page.Width.Point - margin * 2),
                        XUnit.FromPoint(20)),
                    XStringFormats.TopLeft);

                y += 18;
            }

            void EnsureSpace(double requiredSpace)
            {
                if (y + requiredSpace <= page.Height.Point - margin)
                    return;

                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                y = margin;
            }

            DrawLine("Faktura sprzedaży", titleFont);
            DrawLine(string.Empty, font);

            DrawLine($"Numer: {faktura.Numer}", font);
            DrawLine($"Data wystawienia: {faktura.DataWystawienia:yyyy-MM-dd}", font);
            DrawLine($"Data sprzedaży: {faktura.DataSprzedazy:yyyy-MM-dd}", font);
            DrawLine($"Kontrahent: {faktura.Kontrahent?.NazwaPelna}", font);
            DrawLine($"Status: {faktura.StatusFaktury?.Nazwa}", font);
            DrawLine($"Waluta: {faktura.Waluta?.Kod}", font);
            DrawLine($"Sposób płatności: {faktura.SposobPlatnosci?.Nazwa}", font);

            if (faktura.TerminPlatnosci.HasValue)
                DrawLine($"Termin płatności: {faktura.TerminPlatnosci:yyyy-MM-dd}", font);

            DrawLine(string.Empty, font);

            DrawLine("Pozycje faktury", headerFont);
            DrawLine("Lp | Towar | Ilość | Cena netto | Netto | VAT | Brutto", font);

            var pozycjeList = pozycje?.ToList() ?? new List<PozycjaFakturySprzedazy>();
            var index = 1;

            foreach (var pozycja in pozycjeList)
            {
                EnsureSpace(20);

                var line = string.Format(
                    culture,
                    "{0} | {1} | {2:N2} | {3:N2} | {4:N2} | {5:N2} | {6:N2}",
                    index,
                    pozycja.Towar?.Nazwa ?? "-",
                    pozycja.Ilosc,
                    pozycja.CenaNetto,
                    pozycja.WartoscNetto ?? 0m,
                    pozycja.WartoscVat ?? 0m,
                    pozycja.WartoscBrutto ?? 0m);

                DrawLine(line, font);
                index++;
            }

            DrawLine(string.Empty, font);
            EnsureSpace(60);

            DrawLine($"Razem netto: {faktura.RazemNetto?.ToString("N2", culture)}", headerFont);
            DrawLine($"Razem VAT: {faktura.RazemVat?.ToString("N2", culture)}", headerFont);
            DrawLine($"Razem brutto: {faktura.RazemBrutto?.ToString("N2", culture)}", headerFont);

            document.Save(filePath);
        }
    }
}
