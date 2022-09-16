namespace MyFavoriteMovie.WebAPI.Utiles
{
    public class Parser
    {
        public static double? ParseToHeight(string? str)
        {
            if (double.TryParse(str, out var height))
                return height;
            else
                return null;
        }

        public static DateTime? ParseToDateTime(string? str)
        {
            if (DateTime.TryParse(str, out DateTime result))
                return result;

            return null;
        }

        public static TimeSpan? ParseToTimeSpan(string? str)
        {
            if (TimeSpan.TryParse(str, out TimeSpan result))
                return result;

            return null;
        }
    }
}
