using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MySql.Data;
using Fursion_CSharpTools.Tools;
using Fursion_CSharpTools.AsyncJob;
using MySql.Data.MySqlClient;

namespace Fursion_CSharpTools.Net.Server
{
    /// <summary>
    /// 服务器端的IO操作类，包含远程数据库和本地文件存储
    /// </summary>
    public class Service_IOSystem : Singleton<Service_IOSystem>
    {
        private MySqlConnection MainDataBase;
        private string MySQLConnectCommand = "Database=TankTest;DataSource=cdb-ahtsamo2.cd.tencentcdb.com;User ID=root;Password=Dj199706194430;port=10000;";

        /// <summary>
        /// 数据库连接的模板语句；不可直接用于连接，需要调用CreateMySQLConnectionStatement()方法生成实际的SQLConnectionSatement连接数据库;
        /// </summary>
        private const string MySQLConnectionStatementTemplate = "Database={0};DataSource={1};User ID={2};Password={3};port={4};";
        /// <summary>
        /// MySQL连接池
        /// </summary>
        private const string SqlConnectionPoolConfig = "Pooling=true;Max Pool Size={0}; Min Pool Size={1}";
        /// <summary>
        /// 拼接数据库连接语句
        /// </summary>
        /// <param name="Database">数据库名</param>
        /// <param name="DataSource">数据库地址</param>
        /// <param name="User_ID">数据库登录用户名</param>
        /// <param name="Password">数据库登录密码</param>
        /// <param name="port">数据库远程端口号</param>
        /// <returns>拼接好的连接语句</returns>
        public static string CreateMySQLConnectionStatement(string Database, string DataSource, string User_ID, string Password, int port)
        {
            string SQLConnectionSatement = string.Format(MySQLConnectionStatementTemplate, Database, DataSource, User_ID, Password, port);
            return SQLConnectionSatement;
        }
        /// <summary>
        /// MySQL-Select查询语句模板；需要的参数，查询的列名，表名，限制条件
        /// </summary>
        private const string MySQLSelectStatementTemplate = "select * from {0} where {1} = {2};";
        /// <summary>
        /// 拼接数据库查询语句
        /// </summary>
        /// <param name="ColumnName">要查询的列的名字</param>
        /// <param name="TableName">数据表的名字</param>
        /// <param name="Restrictions">查询的限制条件</param>
        /// <returns></returns>
        public static string CreateMySQLSelectStatement(string ColumnName, string TableName, string Restrictions)
        {
            var SQLSelectStatement = string.Format(MySQLSelectStatementTemplate, TableName, ColumnName, Restrictions);
            return SQLSelectStatement;
        }
        /// <summary>
        /// 查询语句
        /// </summary>
        /// <param name="term"></param>
        /// <param name="MathSymbol">数学符号：= > >= <![CDATA[=,<,>,>=,<=]]></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CreatSelectRestrictions(string term, string MathSymbol, object value)
        {
            var Restrictions = string.Format("{0}{1}{2}", term, MathSymbol, value);
            return Restrictions;
        }
        /// <summary>
        /// 创建连接实例
        /// </summary>
        /// <param name="SQLConnectionStatement"></param>
        /// <returns></returns>
        public static MySqlConnection ConnectSQL(string SQLConnectionStatement)
        {
            FDebug.Log("连接数据库中...");
            MySqlConnection SqlConnect = new MySqlConnection(SQLConnectionStatement);
            try
            {
                SqlConnect.Open();
                FDebug.Log("成功连接到数据库");
                return SqlConnect;
            }
            catch (MySqlException SqlEx)
            {
                FDebug.Log("连接数据库失败： {0}", SqlEx.Message);
                return SqlConnect;
            }
        }
        public void ConnectSQL(Object sateinfo)
        {
            //ThreadPool.QueueUserWorkItem();
        }
        /// <summary>
        /// 数据查询
        /// </summary>
        public void QueryData(string table, string itemName, string item)
        {
            var mysql = ConnectSQL(CreateMySQLConnectionStatement("TankTest", "cdb-ahtsamo2.cd.tencentcdb.com", "root", "Dj199706194430", 10000));
            MySqlCommand mySqlCommand = new MySqlCommand(CreateMySQLSelectStatement(itemName, table, item), mysql);

        }
        /// <summary>
        /// 向数据库添加数据
        /// </summary>
        public void SqlAddData()
        {

        }
        /// <summary>
        /// 从数据库删除数据
        /// </summary>
        public void DeleteData()
        {

        }
        /// <summary>
        /// 修改数据
        /// </summary>
        public void ModifyData()
        {

        }
        public void ReadPathfile()
        {

        }
        public void SavePathfile()
        {

        }
        public struct SQLJob:IJobTask
        {
            public string mysqlcmdStr;
            public string mysqlconnStr;

            public void CallBack(object obj)
            {
                throw new NotImplementedException();
            }

            public void Execute(object obj)
            {
                using(MySqlConnection mySql = Service_IOSystem.ConnectSQL(mysqlconnStr))
                {

                }
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public delegate void IO_SqlQueryCallBackAction();
}
