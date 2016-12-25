using System.Linq;
using WMAPP.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WMAPP
{
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
            int index = new System.Random().Next(0, apiKeys.Count());
            return apiKeys[index];
        }

        /// <summary>
        /// 在线调用API，返回对应结果
        /// </summary>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetFaceKey(string apiKey)
        {
            var client = new HttpClient();
            //请求头
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

            //请求参数
            var url = "https://api.projectoxford.ai/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false";

            //请求主体（可以是图片URL的json格式，也可以是图片类型）
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes("{url:'https://images2015.cnblogs.com/blog/658978/201609/658978-20160922111329527-2030285818.png'}");

            using (var content = new ByteArrayContent(byteData))
            {
                //内容类型
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return await client.PostAsync(url, content);
            }
        }

        /// <summary>
        /// 获取一组图片里面的FaceModelList
        /// 可能错误为：FaceException
        /// 默认：在配置文件中随机获取配置的API
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<FaceModel>> GetFaceModelList()
        {
            if (apiKeys == null || apiKeys.Count == 0)
            {
                throw new FaceException("请在Config配置一个或多个有效的APIKey");
            }

            //随机获取APIKey
            string apiKey = GetAPIKey();

            //获取Response
            var response = await GetFaceKey(apiKey);

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
                    var errorModel = await result.JsonToModelsAsync<ErrorModel>();
                    switch (errorModel.Code)
                    {
                        case "BadArgument":
                            throw new FaceException("请求Json格式出错 or ReturnFaceAttributes（参数间逗号分隔）");
                        case "InvalidURL":
                            throw new FaceException("图片链接无效 or 无效的图片格式（格式尽量用：JPG，PNG，Gif等常用格式）");
                        case "InvalidImage":
                            throw new FaceException("解码错误,图片格式不受支持或非正常图片（可能是伪造图片）");
                        case "InvalidImageSize":
                            throw new FaceException("图片大小或者太大（大小：1k ~ 4M）");
                        default:
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