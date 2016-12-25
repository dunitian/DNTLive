using System.Linq;
using WMAPP.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Net.Http.Headers;
using System.IO;

namespace WaterWaterWaterMark
{
    /// <summary>
    /// 作者：dunitian
    /// 时间：2016年12月23日 10:40
    /// 标题：微软人脸识别API的专用工具类
    /// </summary>
    public abstract partial class FaceHelper
    {
        /// <summary>
        /// 获取配置文件中所有的APIKey
        /// </summary>
        protected static List<string> apiKeys = System.Configuration.ConfigurationManager.AppSettings.AllKeys.Where(key => key.Contains("Facekey")).Select(key => System.Configuration.ConfigurationManager.AppSettings[key]).ToList();

        /// <summary>
        /// 随机获取APIKey
        /// </summary>
        /// <returns></returns>
        protected static string GetAPIKey()
        {
            int index = new Random().Next(0, apiKeys.Count());
            return apiKeys[index];
        }

        /// <summary>
        /// 在线调用API，返回对应结果
        /// </summary>
        /// <param name="apiKey">APIKey</param>
        /// <param name="imgPath">图片URL地址或者图片本地地址</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetFaceKey(string apiKey, string imgPath)
        {
            var client = new HttpClient();
            //请求头
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

            //请求参数
            var url = "https://api.projectoxford.ai/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false";

            //请求API
            using (var content = GetContent(imgPath))
            {
                return await client.PostAsync(url, content);
            }
        }

        /// <summary>
        /// 不同地址对应不同处理
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        private static HttpContent GetContent(string imgPath)
        {
            if (imgPath.StartsWith("http"))
            {
                byte[] bytesData = System.Text.Encoding.UTF8.GetBytes(string.Format("{url:'{0}'}", imgPath));
                var content = new ByteArrayContent(bytesData);
                //内容类型
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return content;
            }
            else
            {
                //var content = new ByteArrayContent(File.ReadAllBytes(imgPath));
                //content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                //{
                //    FileName = Path.GetFileName(imgPath)
                //};
                var content = new ByteArrayContent(File.ReadAllBytes(imgPath));
                //内容类型
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return content;
                //var fs = new System.IO.FileStream(imgPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //{
                //    byte[] buffByte = new byte[fs.Length];
                //    fs.Read(buffByte, 0, (int)fs.Length);
                //    return new StreamContent(fs);
                //}
            }
        }

        /// <summary>
        /// 获取一组图片里面的FaceModelList
        /// 可能错误为：FaceException
        /// 默认：在配置文件中随机获取配置的API
        /// </summary>
        /// <param name="imgPath">图片URL地址或者图片本地地址</param>
        /// <returns></returns>
        public static async Task<IEnumerable<FaceModel>> GetFaceModelList(string imgPath)
        {
            if (apiKeys == null || apiKeys.Count == 0)
            {
                throw new FaceException("请在Config配置一个或多个有效的APIKey");
            }

            //随机获取APIKey
            string apiKey = GetAPIKey();

            //获取Response
            var response = await GetFaceKey(apiKey, imgPath);

            //获取返回字符串
            string result = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new FaceException("人脸识别API返回信息为空！");
            }

            #region 针对响应报文进行逐条处理
            switch (response.StatusCode)
            {
                //Response 200
                case System.Net.HttpStatusCode.OK:
                    return await result.JsonToModelsAsync<IEnumerable<FaceModel>>();
                //Response 400
                case System.Net.HttpStatusCode.BadRequest:
                    if (result.Contains("BadArgument"))
                    {
                        throw new FaceException("请求Json格式出错 or ReturnFaceAttributes（参数间逗号分隔）");
                    }
                    else if (result.Contains("InvalidURL"))
                    {
                        throw new FaceException("图片链接无效 or 无效的图片格式（格式尽量用：JPG，PNG，Gif等常用格式）");
                    }
                    else if (result.Contains("InvalidImage"))
                    {
                        throw new FaceException("解码错误,图片格式不受支持或非正常图片（可能是伪造图片）");
                    }
                    else if (result.Contains("InvalidImageSize"))
                    {
                        throw new FaceException("图片大小或者太大（大小：1k ~ 4M）");
                    }
                    else
                    {
                        throw new FaceException("请求出错，请检测图片，请求Json或者请求报文");
                    }
                //Response 401
                case System.Net.HttpStatusCode.Unauthorized:
                    throw new FaceException($"你的开发Key可能已经失效，请联系开发者（当前Key:{apiKey}）");
                //Response 403
                case System.Net.HttpStatusCode.Forbidden:
                    throw new FaceException("你太猛了已经超过了本月分配的额度了！如想继续使用请联系开发者");
                //Response 408
                case System.Net.HttpStatusCode.RequestTimeout:
                    throw new FaceException("请求超时，请重试或者检测网络后再试");
                //Response 415
                case System.Net.HttpStatusCode.UnsupportedMediaType:
                    throw new FaceException("请求时Content-Type不对（application/json，application/octet-stream）");
                //Response 429 or Other
                default:
                    throw new FaceException("请求频率太高，稍作休息可否？如急用请联系开发者");
            }
            #endregion
        }
    }
}
