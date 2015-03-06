using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCMobilePush.Common;

namespace TCMobile.Android.XGNotification
{
    public enum XGNotificationType
    {
        Single,
        Tags,
        AllDevice
    }

    public interface INotification
    {
        uint AccessId { get; set; }
        long TimeStamp { get; set; }
        uint ValidTime { get; set; }
        string Sign { get; set; }

        void GetSign(XGPushChannelSetting channelSetting);
        string GetPostData();
    }

    public delegate void SendNotificationCallbackDelegate(object sender, SendNotificationResult result);

    public class SendNotificationResult
    {
        public SendNotificationResult(INotification notification, Exception error = null)
        {
            this.Notification = notification;
            this.Error = error;
        }

        public INotification Notification { get; set; }
        public Exception Error { get; set; }
        public System.Net.HttpStatusCode HttpStatus { get; set; }
        /// <summary>
        /// 0	调用成功;
        /// -1	参数错误，请对照错误提示和文档检查请求参数;
        /// -2	请求时间戳不在有效期内;
        /// -3	sign校验无效，检查access id和secret key（注意不是access key）;
        /// -99	notification错误;
        /// 2	参数错误，请对照文档检查请求参数;
        /// 7	别名/账号绑定的终端数满了（10个）;
        /// 14	收到非法token，例如ios终端没能拿到正确的token;
        /// 15	信鸽逻辑服务器繁忙;
        /// 19	操作时序错误
        ///     例如进行tag操作前未获取到deviceToken 没有获取到deviceToken的原因: 1.没有注册信鸽或者苹果推送。 2.provisioning profile制作不正确;
        /// 40	推送的token没有在信鸽中注册，请检查终端注册是否成功;
        /// 48	推送的账号没有在信鸽中注册，请检查终端注册是否成功;
        /// 73	消息字符数超限，请减少消息内容再试;
        /// 76	请求过于频繁，请稍后再试;
        /// 其他	内部错误.
        /// </summary>
        [JsonProperty(PropertyName = "ret_code")]
        public int RetCode { get; set; }
        [JsonProperty(PropertyName = "err_msg")]
        public string ErrMsg { get; set; }
        [JsonProperty(PropertyName = "result")]
        public XGNotificationResultStatus Result { get; set; }
    }

    public sealed class XGNotificationResultStatus
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
    }

    public class XGSingleDeviceNotification : INotification
    {
        /// <summary>
        /// 针对某一设备推送
        /// </summary>
        [JsonProperty(PropertyName = "device_token")]
        public string DeviceToken { get; set; }
        /// <summary>
        /// 消息离线存储多久，单位为秒，最长存储时间3天。设为0，则存储时间3天
        /// </summary>
        [JsonProperty(PropertyName = "expire_time")]
        public uint ExpireTime { get; set; }
        /// <summary>
        /// 指定推送时间，格式为year-mon-day hour:min:sec 若小于服务器当前时间，则会立即推送
        /// </summary>
        [JsonProperty(PropertyName = "send_time")]
        public string SendTime { get; set; }
        /// <summary>
        /// 0表示按注册时提供的包名分发消息；1表示按access id分发消息，所有以该access id成功注册推送的app均可收到消息。
        /// </summary>
        [JsonProperty(PropertyName = "multi_pkg")]
        public uint MultiPkg { get; set; }

        /// <summary>
        /// 应用的唯一标识符，在提交应用时管理系统返回
        /// </summary>
        [JsonProperty(PropertyName = "access_id")]
        public uint AccessId { get; set; }
        /// <summary>
        /// 本请求的unix时间戳，用于确认请求的有效期。默认情况下，请求时间戳与服务器时间（北京时间）偏差大于600秒则会被拒绝
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public long TimeStamp { get; set; }
        /// <summary>
        /// 配合timestamp确定请求的有效期，单位为秒，最大值为600。若不设置此参数或参数值非法，则按默认值600秒计算有效期
        /// </summary>
        [JsonProperty(PropertyName = "valid_time")]
        public uint ValidTime { get; set; }
        /// <summary>
        /// 内容签名。生成规则：
        /// A）提取请求方法method（GET或POST）； 
        /// B）提取请求url信息，包括Host字段的IP或域名和URI的path部分，注意不包括Host的端口和Path的querystring。请在请求中带上Host字段，否则将视为无效请求。 
        /// 比如openapi.xg.qq.com/v2/push/single_device或者10.198.18.239/v2/push/single_device; 
        /// C）将请求参数（不包括sign参数）格式化成K=V方式； 
        /// 注意：计算sign时所有参数不应进行urlencode； 
        /// D）将格式化后的参数以K的字典序升序排列，拼接在一起， 
        /// E）拼接请求方法、url、排序后格式化的字符串以及应用的secret_key； 
        /// F）将E形成字符串计算MD5值，形成一个32位的十六进制（字母小写）字符串，即为本次请求sign（签名）的值； 
        /// Sign=MD5($http_method$url$k1=$v1$k2=$v2$secret_key); 该签名值基本可以保证请求是合法者发送且参数没有被修改，但无法保证不被偷窥。 
        /// 例如： POST请求到接口http://openapi.xg.qq.com/v2/push/single_device，有四个参数，access_id=123，timestamp=1386691200，Param1=Value1，Param2=Value2，secret_key为abcde。
        /// 则上述E步骤拼接出的字符串为POSTopenapi.xg.qq.com/v2/push/single_deviceParam1=Value1Param2=Value2access_id=123timestamp=1386691200abcde，计算出该字符串的MD5为ccafecaef6be07493cfe75ebc43b7d53，以此作为sign参数的值
        /// </summary>
        [JsonProperty(PropertyName = "sign")]
        public string Sign { get; set; }

        public virtual void GetSign(XGPushChannelSetting channelSetting)
        {
        }

        public virtual string GetPostData()
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 单推透传提醒
    /// </summary>
    public sealed class XGSingleDeviceRawNotification : XGSingleDeviceNotification
    {
        /// <summary>
        /// 消息类型：1：通知 2：透传消息。
        /// </summary>
        [JsonProperty(PropertyName = "message_type")]
        public uint MessageType { get { return 2; } }
        /// <summary>
        /// 推送消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public XGPushRawMessage Message { get; set; }

        public override void GetSign(XGPushChannelSetting channelSetting)
        {
            StringBuilder sbParam = new StringBuilder();

            sbParam.Append(channelSetting.XGSingleDevice_RestAPI_Method);
            sbParam.Append(channelSetting.XGSingleDevice_RestAPI_Url);

            Dictionary<string, string> dicParam = new Dictionary<string, string>();
            dicParam.Add("access_id", this.AccessId.ToString());
            dicParam.Add("timestamp", this.TimeStamp.ToString());
            dicParam.Add("device_token", this.DeviceToken);
            dicParam.Add("message_type", this.MessageType.ToString());
            dicParam.Add("message", this.Message.ToString());
            dicParam.Add("expire_time", this.ExpireTime.ToString());
            dicParam.Add("send_time", this.SendTime);
            dicParam.Add("multi_pkg", this.MultiPkg.ToString());

            var sortedDic = dicParam.OrderBy(k => k.Key);

            foreach (var item in sortedDic)
            {
                sbParam.AppendFormat("{0}={1}", item.Key, item.Value);
            }

            sbParam.Append(channelSetting.Secret_Key);

            this.Sign = MD5Helper.GetMD5(sbParam.ToString());
        }

        public override string GetPostData()
        {
            StringBuilder sbData = new StringBuilder();

            sbData.AppendFormat("access_id={0}&", this.AccessId.ToString());
            sbData.AppendFormat("timestamp={0}&", this.TimeStamp.ToString());
            sbData.AppendFormat("sign={0}&", this.Sign);
            sbData.AppendFormat("device_token={0}&", this.DeviceToken);
            sbData.AppendFormat("message_type={0}&", this.MessageType.ToString());
            sbData.AppendFormat("message={0}&", this.Message.ToString());
            sbData.AppendFormat("expire_time={0}&", this.ExpireTime.ToString());
            sbData.AppendFormat("send_time={0}&", this.SendTime);
            sbData.AppendFormat("multi_pkg={0}&", this.MultiPkg.ToString());

            return sbData.ToString();
        }
    }
}
