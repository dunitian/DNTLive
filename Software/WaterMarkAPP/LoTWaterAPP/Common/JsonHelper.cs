using Newtonsoft.Json;
using System.Threading.Tasks;

/// <summary>
/// 作者：dunitian
/// 时间：2016年12月23日 10:40
/// 标题：Json序列化和反序列化工具类
/// </summary>
public static partial class JsonHelper
{
    /// <summary>
    /// Model转换成Json
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static async Task<string> ObjectToJsonAsync(this object obj)
    {
        return await Task.Factory.StartNew(() => JsonConvert.SerializeObject(obj));
    }
    /// <summary>
    /// Json转换成Model（异步方法）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static async Task<T> JsonToModelsAsync<T>(this string str)
    {
        return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(str));
    }
}