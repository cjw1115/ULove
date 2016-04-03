using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace ULove.DAL
{
    public class Result
    {
        public object Data { get; set; }
        public string Message { get; set; }
    }
    public class ULoveWebDAL
    {
        private  HttpClientHandler handler;
        public ULoveWebDAL():this(false,null)
        {
        }
        public ULoveWebDAL(bool isauth,string uid)
        {
            Client = new HttpClient();
            if (isauth&&Client!=null)
            {
                var token = DAL.LocalUserDAL.GetLocalSetting<string>("Token");
                Client.DefaultRequestHeaders.Add("Token", token);
                Client.DefaultRequestHeaders.Add("Uid", uid);
            }
           
        }
        public HttpClient Client { get; set; }

        public async Task<Result> Post(string uri,Dictionary<string,string> param=null)
        {
            HttpResponseMessage response = null;
            try
            {
                FormUrlEncodedContent content = new FormUrlEncodedContent(param);
                response = await Client.PostAsync(uri, content);
                string jsonString = await response.Content.ReadAsStringAsync();
                Result re = JsonConvert.DeserializeObject<Result>(jsonString);
                 
                return re;

            }
            catch(HttpRequestException requsetException)
            {
                Result re = new Result();
                re.Message = "发送请求失败";
                return re;
            }
            catch(JsonException jsonException)
            {
                Result re = new Result();
                re.Message = "json解析错误";
                return re;
            }
            finally
            {
                response?.Dispose();
            }
            
        }
        public async Task<T> Post<T>(string uri, Dictionary<string, string> param = null)where T :class
        {
            HttpResponseMessage response = null;
            try
            {
                FormUrlEncodedContent content = new FormUrlEncodedContent(param);
                response = await Client.PostAsync(uri, content);
                string jsonString = await response.Content.ReadAsStringAsync();
                T re = JsonConvert.DeserializeObject<T>(jsonString);
               
                return re;

            }
            catch (HttpRequestException requsetException)
            {
               
                return null;
            }
            catch (JsonException jsonException)
            {
                
                return null;
            }
            finally
            {
                response?.Dispose();
            }

        }
        public async Task<Result> Get(string uri)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await Client.GetAsync(uri);
                string jsonString = await response.Content.ReadAsStringAsync();
                Result re = JsonConvert.DeserializeObject<Result>(jsonString);
                
                return re;

            }
            catch (HttpRequestException requsetException)
            {
                Result re = new Result();
                re.Message = "发送请求失败";
                return re;
            }
            catch (JsonException jsonException)
            {
                Result re = new Result();
                re.Message = "json解析错误";
                return re;
            }
            finally
            {
                response?.Dispose();
            }

        }
        public async Task<T> Get<T>(string uri)where T :class
        {
            HttpResponseMessage response = null;
            try
            {
                response = await Client.GetAsync(uri);
                string jsonString = await response.Content.ReadAsStringAsync();
                T re = JsonConvert.DeserializeObject<T>(jsonString);
                
                return re;

            }
            catch (HttpRequestException requsetException)
            {

                return null ;
            }
            catch (JsonException jsonException)
            {
                
                return null;
            }
            finally
            {
                response?.Dispose();
            }

        }
    }
}
