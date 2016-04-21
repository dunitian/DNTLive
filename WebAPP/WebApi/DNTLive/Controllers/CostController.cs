using DNTLive.Common;
using DNTLive.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNTLive.Controllers
{
    public class CostController : ApiController
    {
        /// <summary>
        /// 获取城市数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ShopMenu>> Get(string name)
        {
            if (string.IsNullOrEmpty(name)) { return null; }
            return await DapperHelperAsync.QueryAsync<ShopMenu>("select SName,MName,Mprice,MType from View_DNTMenu where CName=@Name order by MType,MName", new { Name = name });
        }
    }
}
