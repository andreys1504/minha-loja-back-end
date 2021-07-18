namespace MinhaLoja
{
    public static partial class Helpers
    {
        public static string TrimString(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            return value;
        }

        public static string GetNumbers(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            value = value.Trim();
            string result = string.Empty;

            for (int index = 0; index < value.Length; index++)
            {
                if (char.IsDigit(value[index]))
                {
                    result += value[index];
                }
            }

            return result;
        }
    }
}
