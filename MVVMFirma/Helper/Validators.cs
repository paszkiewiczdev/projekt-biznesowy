using System.Globalization;

namespace MVVMFirma.Helper
{
    public static class Validator
    {
        public static string Required(string value, string fieldName)
        {
            return string.IsNullOrWhiteSpace(value) ? $"{fieldName} jest wymagane." : null;
        }

        public static string Required(object value, string fieldName)
        {
            return value == null ? $"{fieldName} jest wymagane." : null;
        }

        public static string DecimalInRange(string value, string fieldName, decimal min, decimal max)
        {
            if (string.IsNullOrWhiteSpace(value))
                return $"{fieldName} jest wymagane.";

            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed))
                return $"{fieldName} ma niepoprawny format.";

            if (parsed < min || parsed > max)
                return $"{fieldName} musi być w zakresie {min}-{max}.";

            return null;
        }

        public static string PositiveDecimal(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return $"{fieldName} jest wymagane.";

            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed))
                return $"{fieldName} ma niepoprawny format.";

            if (parsed <= 0)
                return $"{fieldName} musi być większe od 0.";

            return null;
        }
    }

    public static class StringValidator
    {
        public static string RequiredAndLength(string value, string fieldName, int minLength, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return $"{fieldName} jest wymagane.";

            var length = value.Trim().Length;
            if (length < minLength)
                return $"{fieldName} musi mieć min. {minLength} znaki.";

            if (length > maxLength)
                return $"{fieldName} może mieć maks. {maxLength} znaków.";

            return null;
        }
    }

    public static class BiznesValidator
    {
        public static string Percent(string value, string fieldName)
        {
            return Validator.DecimalInRange(value, fieldName, 0, 100);
        }
    }
}
