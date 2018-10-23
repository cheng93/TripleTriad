using Npgsql.NameTranslation;

namespace TripleTriad.Data.Extensions
{
    internal static class StringExtensions
    {
        public static string ToSnakeCase(this string input) => NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase(input);
    }
}