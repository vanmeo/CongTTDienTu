using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Xim.Library.Https
{
    public static class HttpService
    {
        public static async Task<HttpResponseMessage> SendAsync(HttpClient httpClient, HttpMethod method, string url, Dictionary<string, string> headers = null, object data = null)
        {
            var requestMessage = new HttpRequestMessage();
            requestMessage.Method = method;
            var requestUrl = url;
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    SetRequestHeader(requestMessage.Headers, item.Key, item.Value);
                }
            }

            if (data != null)
            {
                if (method == HttpMethod.Get)
                {
                    if (data is string)
                    {
                        //nếu là string thì đây là chuỗi tham số rồi -> nhắm mắt + vào url
                        requestUrl += (string)data;
                    }
                    else
                    {
                        Dictionary<string, object> tempData;
                        if (data is Dictionary<string, object>)
                        {
                            tempData = (Dictionary<string, object>)data;
                        }
                        else
                        {
                            tempData = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(data));
                        }

                        var sb = new StringBuilder(requestUrl);
                        if (!requestUrl.Contains("?"))
                        {
                            sb.Append("?");
                        }

                        foreach (var ditem in tempData)
                        {
                            sb.Append($"&{ditem.Key}=");    //{ditem.Value}
                            var value = ditem.Value;

                            if (value is string stringValue)
                            {
                                sb.Append(HttpUtility.UrlEncode(stringValue));
                            }
                            else
                            {
                                sb.Append(value);
                            }
                        }
                        requestUrl = sb.ToString();
                    }
                }
                else
                {
                    if (data is HttpContent content)
                    {
                        requestMessage.Content = content;
                    }
                    else if (data is string)
                    {
                        requestMessage.Content = new StringContent(data as string, Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    }
                }
            }

            var ru = new StringBuilder();
            if (!string.IsNullOrEmpty(httpClient.BaseAddress?.AbsoluteUri))
            {
                ru.Append(httpClient.BaseAddress.AbsoluteUri);
            }

            if (!requestUrl.StartsWith("/") && ru[ru.Length - 1] != '/')
            {
                ru.Append("/");
            }
            ru.Append(requestUrl);

            requestMessage.RequestUri = new Uri(ru.ToString());
            var response = await httpClient.SendAsync(requestMessage);
            return response;
        }

        /// <summary>
        /// Gán header
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        private static void SetRequestHeader(HttpRequestHeaders header, string key, string value)
        {
            var lowerKey = key.ToLower();
            switch (lowerKey)
            {
                case "accept":
                case "content-length":
                case "content-type":
                case "host":
                    //ignore
                    break;
                default:
                    header.Add(key, value);
                    break;
            }
        }
    }
}
