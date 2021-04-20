using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.FireBaseNotificationService
{
    public class NotificationPostData
    {
        [JsonProperty(PropertyName = "registration_ids")]
        public List<string> FcmTokens { get; set; }

        [JsonProperty(PropertyName = "data")]
        public NotificationMessageData MessageData { get; set; }
    }
}
