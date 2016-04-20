using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DNTLive.Common
{
    public abstract class DapperHelper
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
        public static IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                return conn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
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
        public static IEnumerable<dynamic> Query(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                return conn.Query(sql, param, transaction, buffered, commandTimeout, commandType);
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
        public static SqlMapper.GridReader QueryMultiple(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                return conn.QueryMultiple(sql, param, transaction, commandTimeout, commandType);
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
        public static int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                return conn.Execute(sql, param, transaction, commandTimeout, commandType);
            }
        } 
        #endregion
    }
}