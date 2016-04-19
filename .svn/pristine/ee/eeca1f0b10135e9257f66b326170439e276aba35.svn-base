using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DNTLive.Common
{
    public abstract class DapperHelperAsync
    {
        public static readonly string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnStr"].ConnectionString;

        #region 查询系列
        /// <summary>
        /// 强类型查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                await conn.OpenAsync();
                return await conn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 动态类型查询 | 多映射动态查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                await conn.OpenAsync();
                return await conn.QueryAsync(sql, param, transaction, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 多返回值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
               await conn.OpenAsync();
                return await conn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);
            }
        }
        #endregion

        #region 增删改
        /// <summary>
        /// 增删改查
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                await  conn.OpenAsync();
                return await conn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
            }
        }
        #endregion
    }
}