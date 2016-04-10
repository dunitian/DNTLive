using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace ShopMenu
{
    public static partial class ShopMenuHelper
    {
        #region 数据库操作
        /// <summary>
        /// 获取CityInfo集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connStr"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static IList<T> GetCityInfoList<T>(string connStr, int pid = 1) where T : class, new()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var dt = new DataTable();
                var adapter = new SqlDataAdapter("select CId,CName,CPid from CityInfo where CPid=@CPid and CDataStatus<>99", conn);
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@CPid", pid));
                adapter.Fill(dt);
                return DataTableToList<T>(dt);
            }
        }

        /// <summary>
        /// 获取所有超市名
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <returns></returns>
        public static IList<ShopModel> GetShopInfos(string connStr)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var dt = new DataTable();
                var adapter = new SqlDataAdapter("select distinct SId,SName from ShopModel where SDataStatus<>99", conn);
                adapter.Fill(dt);
                return DataTableToList<ShopModel>(dt);
            }
        }

        /// <summary>
        /// 获取所有类型名
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <returns></returns>
        public static IList<NameModel> GetTypeInfos(string connStr)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var dt = new DataTable();
                var adapter = new SqlDataAdapter("select distinct MType from ShopMenu where MDataStatus<>99 group by MType", conn);
                adapter.Fill(dt);
                return DataTableToList<NameModel>(dt);
            }
        }

        /// <summary>
        /// 插入数据库
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <param name="path">文件路径</param>
        /// <param name="model">超市菜单模型</param>
        /// <returns></returns>
        public static bool GetInsertCount(string connStr, string path, ShopMenuModel model)
        {
            if (model == null) { return false; }
            var dics = GetDictionary(path);
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "insert into ShopMenu values(@MName,@MPrice,@MType,@MShopId,@MCityId,0,@MCreateTime)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Transaction = conn.BeginTransaction();
                    try
                    {
                        foreach (var item in dics)
                        {
                            var pms = new SqlParameter[]
                            {
                                new SqlParameter("@MName",item.Key),
                                new SqlParameter("@MPrice", item.Value),
                                new SqlParameter("@MType",model.MType ),
                                new SqlParameter("@MShopId",model.MShopId ),
                                new SqlParameter("@MCityId",model.MCityId ),
                                new SqlParameter("@MCreateTime", DateTime.Now)
                            };
                            cmd.Parameters.AddRange(pms);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        cmd.Transaction.Commit();
                    }
                    catch
                    {
                        cmd.Transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="model"></param>
        public static string Init(TempModel model)
        {
            if (!Directory.Exists("App"))
            {
                Directory.CreateDirectory("App");
            }
            if (!File.Exists("App/DNT.json"))
            {
                try
                {
                    //throw new Exception("调试的时候手动抛出的异常~勿要慌");
                    File.WriteAllText("App/DNT.json", model.ObjectToJson(), Encoding.UTF8);
                }
                catch
                {
                    return "初始化失败,请重新打开或者在App文件夹创建DNT.json\n内容如下：{\"CityId\":\"城市ID\"ConnStr\":\"数据库连接语句\"}";
                }
            }
            return "初始化成功";
        }

        /// <summary>
        /// 将指定的datatable转换成IList<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">datatable</param>
        /// <returns></returns>
        public static IList<T> DataTableToList<T>(DataTable data) where T : class, new()
        {
            List<PropertyInfo> pList = new List<PropertyInfo>();
            Type t = typeof(T);
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (data.Columns.IndexOf(p.Name) != -1) { pList.Add(p); } });
            IList<T> result = new List<T>();
            foreach (DataRow item in data.Rows)
            {
                T model = new T();
                pList.ForEach(p => { if (item[p.Name] != DBNull.Value) { p.SetValue(model, item[p.Name], null); } });
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 读取文件返回Dictionary
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetDictionary(string path)
        {
            var dics = new Dictionary<string, string>();
            var encoding = EncodingHelper.GetType(path);//获取文件编码
            var strs = File.ReadAllLines(path, encoding);//获取文本内容
            for (int i = 0; i < strs.Length; i++)
            {
                if (!string.IsNullOrEmpty(strs[i]))
                {
                    var temps = strs[i].Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    if (temps.Length == 2)
                    {
                        if (dics.ContainsKey(temps[0]))
                        {
                            continue;
                        }
                        else
                        {
                            dics.Add(temps[0], temps[1]);
                        }
                    }
                }
            }
            return dics;
        }
        #endregion
    }
}