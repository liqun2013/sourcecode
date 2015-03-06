using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace TCMobile.Android.XGNotification
{
    public class XGPushChannel
    {
        private XGPushChannelSetting pushChannelSetting;

        public XGPushChannel()
        {
            pushChannelSetting = new XGPushChannelSetting();
        }

        public XGPushChannel(XGPushChannelSetting setting)
        {
            pushChannelSetting = setting;
        }

        /// <summary>
        /// 单推消息
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="callback"></param>
        public void SendSingleDeviceNotification(INotification notification, SendNotificationCallbackDelegate callback)
        {
            if (/*!(notification is XGSingleDeviceTileNotification) && */!(notification is XGSingleDeviceRawNotification))
            {
                callback(this, new SendNotificationResult(notification)
                {
                    ErrMsg = "notification 既不是XGSingleDeviceTileNotification类型,也不是XGSingleDeviceRawNotification类型",
                    RetCode = -99
                });

                return;
            }

            var wr = HttpWebRequest.Create("http://" + pushChannelSetting.XGSingleDevice_RestAPI_Url);
            wr.ContentType = "application/x-www-form-urlencoded";//"text/json;charset=\"utf-8\"";
            wr.Method = pushChannelSetting.XGSingleDevice_RestAPI_Method;

            notification.AccessId = pushChannelSetting.XGSingleDevice_RestAPI_AccessId;
            notification.TimeStamp = (DateTime.Now.ToLocalTime().Ticks - new DateTime(1970, 1, 1).ToLocalTime().Ticks) / 10000000;
            notification.GetSign(pushChannelSetting);

            var postData = notification.GetPostData();
            using (var wrs = new StreamWriter(wr.GetRequestStream()))
            {
                wrs.Write(postData);
            }

            try
            {
                wr.BeginGetResponse(getNotificationResponseCallback, new object[] { wr, notification, callback });
            }
            catch (WebException wex)
            {
                var resp = wex.Response as HttpWebResponse;
                var status = resp.StatusCode;

                if (callback != null)
                {
                    SendNotificationResult result = new SendNotificationResult(notification, wex);
                    result.HttpStatus = status;

                    callback(this, result);
                }
            }
        }

        void getNotificationResponseCallback(IAsyncResult asyncResult)
        {
            var objs = (object[])asyncResult.AsyncState;

            var wr = (HttpWebRequest)objs[0];
            var xgNotification = (XGSingleDeviceNotification)objs[1];
            var callback = (SendNotificationCallbackDelegate)objs[2];

            HttpWebResponse resp = null;
            SendNotificationResult result = null;

            try
            {
                resp = wr.EndGetResponse(asyncResult) as HttpWebResponse;
                var str = string.Empty;

                using (var sr = new StreamReader(resp.GetResponseStream()))
                {
                    str = sr.ReadToEnd();
                }

                result = JsonConvert.DeserializeObject<SendNotificationResult>(str);

                result.Notification = xgNotification;

                if (callback != null)
                {
                    callback(this, result);
                }
            }
            catch (WebException webEx)
            {
                resp = webEx.Response as HttpWebResponse;

                result = new SendNotificationResult(xgNotification, webEx);
                result.HttpStatus = resp.StatusCode;

                if (callback != null)
                {
                    callback(this, result);
                }
            }
            catch { }
            finally
            {
                resp.Close();
            }
        }
    }
}
