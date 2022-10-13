using Newtonsoft.Json;

namespace ClassLibrary.Common.Exceptions.Logging
{
    public class ErrorDetails
    {
        [JsonProperty("status")]
        public int StatucCode { get; set; }
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
