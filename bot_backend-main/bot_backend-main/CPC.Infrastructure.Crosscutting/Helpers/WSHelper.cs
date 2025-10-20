using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CPC.Infrastructure.CrossCutting.Helpers;
using Mysqlx.Session;

namespace CPC.Infrastructure.Crosscutting.Helpers
{
    public class WSHelper
    {
        public static async Task<string> PostClientFormUrlEncoded(string uri, IEnumerable<KeyValuePair<string, string>> postData)
        {
            var responseData = string.Empty;
            try
            {
                HttpClient request = new HttpClient();

                using (var content = new FormUrlEncodedContent(postData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application /x-www-form-urlencoded");

                    HttpResponseMessage response = await request.PostAsync(uri, content);
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        throw new Exception(respStream.ReadToEnd());
                    }
                }
            }
            return responseData;
        }

        public static async Task<TR> UploadAsync<TR>(string url, string content)
        {
            var responseEntity = default(TR);
            byte[] byteArray = Encoding.ASCII.GetBytes(content);
            var stream = new MemoryStream(byteArray);
            HttpContent fileStreamContent = new StreamContent(stream);

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(fileStreamContent, "file1", "file1");

                var response = await client.PostAsync(url, formData);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error SSO response");
                }
                var result = await response.Content.ReadAsStreamAsync();
                responseEntity = result.GetEntityFromJSON<TR>();
            }
            return responseEntity;
        }


        public static async Task<TR> CallWebServiceAsync<TR>(string uri, string source)
        {
            var responseEntity = default(TR);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(source);
                }
                var response = await request.GetResponseAsync();
                responseEntity = response.GetResponseStream().GetEntity<TR>();
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        throw new Exception(respStream.ReadToEnd());
                    }
                }
            }
            return responseEntity;
        }

        public static async Task<TR> CallWebServiceJsonAsync<TR>(string uri, string source, HttpMethod method, Dictionary<string, string> headers = null)
        {
            var responseEntity = default(TR);
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(method, uri);
                var content = new StringContent(source, null, "application/json");

                // request.Headers.Add("Authorization", $"Basic {authentication}");

                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                responseEntity = result.DeserializeJson<TR>();
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        throw new Exception(respStream.ReadToEnd());
                    }
                }
            }
            return responseEntity;
        }

        public static async Task<Stream> CallAPIStreamAsync(string uri, string source, HttpMethod method, Dictionary<string, string> headers = null)
        {
            var responseStream = default(Stream);
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(method, uri);
                var content = new StringContent(source, null, "application/json");

                // request.Headers.Add("Authorization", $"Basic {authentication}");

                request.Content = content;
                var response = await client.PostAsync(uri, content);

                var responseContent = response.Content;

                if (response.IsSuccessStatusCode)
                {
                    responseStream = await responseContent.ReadAsStreamAsync();                    
                }
                return responseStream;
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        throw new Exception(respStream.ReadToEnd());
                    }
                }
            }
            return responseStream;
        }

        public static async Task<TR> CallWebServiceJAsync<TR>(string uri, string source, HttpMethod method, Dictionary<string, string> headers = null)
        {
            var responseEntity = default(TR);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = method.ToString();
                request.ContentType = "application/json;";

                if (!ReferenceEquals(headers, null))
                    headers.ToList().ForEach((_) => { request.Headers.Add(_.Key, _.Value); });

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(source);
                }

#if DEBUG
                ServicePointManager.ServerCertificateValidationCallback =
                            delegate (
                                object s,
                                X509Certificate certificate,
                                X509Chain chain,
                                SslPolicyErrors sslPolicyErrors
                            )
                            {
                                return true;
                            };
#endif

                var response = await request.GetResponseAsync();
                responseEntity = response.GetResponseStream().GetEntityFromJSON<TR>();
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        throw new Exception(respStream.ReadToEnd());
                    }
                }
            }
            return responseEntity;
        }

        public static TR CallWebService<TR>(string uri, string source)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(source);
            }
            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream().GetEntity<TR>();
        }

        public static string CallWebServiceJSON(string uri, string source)
        {
            string objText = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "text/plain; charset=UTF-8";

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(source);
                }

                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                var response = (HttpWebResponse)request.GetResponse();


                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    objText = HttpUtility.HtmlDecode(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        var errorResult = respStream.ReadToEnd();
                    }
                }
            }

            return objText;
        }

        public static async Task<TR> CallWebServiceJSONAPI<TR>(string uri, string source, HttpMethod method, string Token)
        {
            var responseEntity = default(TR);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = method.ToString();
                //request.ContentType = "application/json;";
                //request.Accept = "application/json;";

                var headers = new WebHeaderCollection { { "Content-Type", "application/json; charset=UTF-8" }, { "Authorization", $"Bearer {Token}" } };
                request.Headers = headers;

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(source);
                }

                var response = await request.GetResponseAsync();
                responseEntity = response.GetResponseStream().GetEntityFromJSON<TR>();
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    if (((HttpWebResponse)errResp).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        using (var respStream = new StreamReader(errResp.GetResponseStream()))
                        {
                            throw new Exception(respStream.ReadToEnd());
                        }
                    }
                }
            }
            return responseEntity;
        }

        public static string CallWebServiceGetJSON(string uri, string Token)
        {
            string objText = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.ContentType = "text/plain; charset=UTF-8";
                request.Accept = "application/vnd.api+json";
                var headers = new WebHeaderCollection { { "Authorization", "Bearer " + Token } };
                request.Headers = headers;

                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                var response = (HttpWebResponse)request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    objText = HttpUtility.HtmlDecode(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        var errorResult = respStream.ReadToEnd();
                    }
                }
            }

            return objText;
        }

        public static string CallWebServiceGet(string uri)
        {
            string objText = string.Empty;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.ContentType = "text/plain; charset=UTF-8";
                request.Accept = "application/vnd.api+json";

                var response = (HttpWebResponse)request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    objText = HttpUtility.HtmlDecode(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        var errorResult = respStream.ReadToEnd();
                    }
                }
            }

            return objText;
        }

        public static async Task<T> WebApiGetRequest<T>(string uri, string Token)
        {
            var responseEntity = default(T);

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.ContentType = "text/plain; charset=UTF-8";
                request.Accept = "application/vnd.api+json";
                var headers = new WebHeaderCollection { { "Authorization", "Bearer " + Token } };
                request.Headers = headers;

                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                var response = await request.GetResponseAsync();
                responseEntity = response.GetResponseStream().GetEntityFromJSON<T>();
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        var errorResult = respStream.ReadToEnd();
                    }
                }
            }

            return responseEntity;
        }
        // web api generico
        public static async Task<R> WebApiRequestGeneric<R, T>(string uri, string token, T content, string method = "POST")
        {
            var responseEntity = default(R);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = method;
                request.ContentType = "text/plain; charset=UTF-8";
                request.Accept = "application/vnd.api+json";
                var headers = new WebHeaderCollection { { "Authorization", "Bearer " + token } };
                request.Headers = headers;

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(content.SerializeToJson());
                }
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                var response = await request.GetResponseAsync();
                responseEntity = response.GetResponseStream().GetEntityFromJSON<R>();
            }
            catch (Exception e)
            {
                if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)e).Response;
                    using (var respStream = new StreamReader(errResp.GetResponseStream()))
                    {
                        var errorResult = respStream.ReadToEnd();
                    }
                }
            }

            return responseEntity;
        }

        private static async Task<string> WebApiResponse(string baseUri, string token, Func<HttpClient, Task<HttpResponseMessage>> func)
        {
            string responseString;
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(baseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await func(client);
                    response.EnsureSuccessStatusCode();
                    responseString = await response.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    if (e is WebException && ((WebException)e).Status == WebExceptionStatus.ProtocolError)
                    {
                        WebResponse errResp = ((WebException)e).Response;
                        using (var respStream = new StreamReader(errResp.GetResponseStream()))
                        {
                            responseString = respStream.ReadToEnd();
                        }
                    }
                    else
                    {
                        responseString = e.Message;
                    }
                }
            }
            return responseString;
        }

    }
}