namespace TCMobile.Android.XGNotification
{
	public sealed class XGPushChannelSetting
	{
        public readonly string XGSingleDevice_RestAPI_Url;
        public readonly uint XGSingleDevice_RestAPI_AccessId;
        public readonly string XGSingleDevice_RestAPI_Method;
        public readonly string Secret_Key;

        public XGPushChannelSetting()
        {
            this.Secret_Key = "86e83b91a1ea0cb005e45ae169ff47ee";
            this.XGSingleDevice_RestAPI_AccessId = 2100046315;
            this.XGSingleDevice_RestAPI_Method = "POST";
            this.XGSingleDevice_RestAPI_Url = @"openapi.xg.qq.com/v2/push/single_device";
        }

        public XGPushChannelSetting(string secretKey, uint accessId, string method, string openApiUrl)
        {
            this.Secret_Key = secretKey;
            this.XGSingleDevice_RestAPI_AccessId = accessId;
            this.XGSingleDevice_RestAPI_Method = method;
            this.XGSingleDevice_RestAPI_Url = openApiUrl;
        }
    }
}
