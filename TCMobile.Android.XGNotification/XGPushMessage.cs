using System.Collections.Generic;
using Newtonsoft.Json;

namespace TCMobile.Android.XGNotification
{
    /// <summary>
    /// 推送通知消息
    /// </summary>
    public sealed class XGPushTileMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        /// <summary>
        /// 用户自定义的key-value，选填
        /// </summary>
        [JsonProperty(PropertyName = "custom_content")]
        public Dictionary<string, string> CustomContent { get; set; }
        /// <summary>
        /// 是否响铃，0否，1是，下同。选填，默认0
        /// </summary>
        [JsonProperty(PropertyName = "ring")]
        public int Ring { get; set; }
        /// <summary>
        /// 是否振动，选填，默认0
        /// </summary>
        [JsonProperty(PropertyName = "vibrate")]
        public int Vibrate { get; set; }
        /// <summary>
        /// 通知栏是否可清除，选填，默认1
        /// </summary>
        [JsonProperty(PropertyName = "clearable")]
        public int ClearAble { get; set; }
        /// <summary>
        /// 动作，必填
        /// </summary>
        [JsonProperty(PropertyName = "action")]
        public XGPushMessageAction Action { get; set; }
    }

    /// <summary>
    /// 动作
    /// </summary>
    public sealed class XGPushMessageAction
    {
        /// <summary>
        /// 动作类型，1打开activity或app本身，2打开浏览器，3打开Intent 
        /// </summary>
        [JsonProperty(PropertyName = "action_type")]
        public int ActionType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "browser")]
        public XGPushMessageActionBrowser Browser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "activity")]
        public string Activity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "intent")]
        public string Intent { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class XGPushMessageActionBrowser
    {
        /// <summary>
        /// 打开的url
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        /// <summary>
        /// 是否需要用户确认
        /// </summary>
        [JsonProperty(PropertyName = "confirm")]
        public int Confirm { get; set; }
    }

    /// <summary>
    /// 推送透传消息
    /// </summary>
    public sealed class XGPushRawMessage
    {
        #region emoji正则表达式
        private static string[] emojiRegexExpressions = new string[]
        {
            @"\[u[a-fA-F0-9]{2}\]", @"\[u[a-fA-F0-9]{4}\]", @"\[u[a-fA-F0-9]{8}\]"
        };
        #endregion

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        /// <summary>
        /// 用户自定义的key-value，选填
        /// </summary>
        [JsonProperty(PropertyName = "custom_content")]
        public Dictionary<string, string> CustomContent { get; set; }
        /// <summary>
        /// 允许推送给用户的时段，选填
        /// </summary>
        [JsonProperty(PropertyName = "accept_time")]
        public List<XGPushMessageAcceptTime> AcceptTime { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            return this.ToJson();
        }
    }

    public sealed class XGPushMessageAcceptTime
    {
        [JsonProperty(PropertyName = "start")]
        public XGPushMessageAcceptTimeDetail Start { set; get; }

        [JsonProperty(PropertyName = "end")]
        public XGPushMessageAcceptTimeDetail End { set; get; }
    }

    public sealed class XGPushMessageAcceptTimeDetail
    {
        [JsonProperty(PropertyName = "hour")]
        public string Hour { set; get; }

        [JsonProperty(PropertyName = "min")]
        public string Min { set; get; }
    }
}
