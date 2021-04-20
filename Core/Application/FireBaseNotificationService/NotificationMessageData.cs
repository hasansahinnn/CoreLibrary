using Newtonsoft.Json;

namespace Core.FireBaseNotificationService
{
    public class NotificationMessageData
    {
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
}
