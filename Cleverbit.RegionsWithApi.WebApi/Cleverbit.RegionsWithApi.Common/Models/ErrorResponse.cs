using Cleverbit.RegionsWithApi.Common.Models.Enums;

namespace Cleverbit.RegionsWithApi.Common.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }
}
