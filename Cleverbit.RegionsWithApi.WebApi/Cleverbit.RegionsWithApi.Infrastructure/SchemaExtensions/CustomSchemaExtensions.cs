namespace Cleverbit.RegionsWithApi.Infrastructure.SchemaExtensions
{
    public static class CustomSchemaExtensions
    {
        public static string GetSchemaFileName(this string str)
        {
            return str.Replace("Cleverbit.RegionsWithApi.Core.Features.", "")
                      .Replace("Cleverbit.RegionsWithApi.Data.Entities.Enums.", "")
                      .Replace("+", "")
                      .Replace("-", "")
                      .Replace("_", "");
        }
    }
}
