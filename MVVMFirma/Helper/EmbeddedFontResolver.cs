using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MVVMFirma.Helper
{
    public sealed class EmbeddedFontResolver : IFontResolver
    {
        private const string RegularFont = "DejaVuSans";
        private const string BoldFont = "DejaVuSans-Bold";

        private static readonly Dictionary<string, string> FamilyMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Arial"] = RegularFont,
                ["DejaVu Sans"] = RegularFont,
                ["Sans"] = RegularFont
            };

        public string DefaultFontName => RegularFont;

        public byte[] GetFont(string faceName)
        {
            return LoadFontData(faceName);
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            var resolvedFamily = ResolveFamilyName(familyName);
            var faceName = isBold || string.Equals(resolvedFamily, BoldFont, StringComparison.OrdinalIgnoreCase)
                ? BoldFont
                : RegularFont;

            return new FontResolverInfo(faceName);
        }

        private static string ResolveFamilyName(string familyName)
        {
            if (string.IsNullOrWhiteSpace(familyName))
                return RegularFont;

            return FamilyMap.TryGetValue(familyName, out var mapped)
                ? mapped
                : RegularFont;
        }

        private static byte[] LoadFontData(string faceName)
        {
            var resourceName = faceName switch
            {
                BoldFont => "MVVMFirma.Resources.Fonts.DejaVuSans-Bold.ttf",
                _ => "MVVMFirma.Resources.Fonts.DejaVuSans.ttf"
            };

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Nie znaleziono zasobu czcionki: {resourceName}.");
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
