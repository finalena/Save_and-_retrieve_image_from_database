using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace TLC
{      
    public class DB
    {
        public static string Conn = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = WinFormTest; Integrated Security = True";
        /// <summary>
        /// 取得新的DataTable
        /// </summary>
        /// <param name="dtOld">舊的DataTable</param>
        /// <param name="sFilter">篩選</param>
        /// <param name="sSort">排序</param>
        /// <returns>新的DataTable</returns>
        public static DataTable DTFilter(DataTable dtOld, string sFilter, string sSort)
        {
            return (new DataView(dtOld, sFilter, sSort, DataViewRowState.CurrentRows)).ToTable();
        }
        
        # region GetDataTable 取得DataTable
        /// <summary> 取得DataTable </summary>
        /// <param name="sSql">SQL子句</param>
        /// <param name="sTableName">Table名稱(可不輸入)</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDt(string sSql, string sTableName = "TABLE")
        {
            SqlConnection conn = new SqlConnection(Conn);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sSql , conn);
                da.Fill(dt);

                //sTableName!=TABLE 表示有設定TableName
                if (sTableName != "TABLE")
                {
                    dt.TableName = sTableName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        /// <summary> 取得DataTable(SQLServer連線) </summary>
        /// <param name="sSql">SQL子句</param>
        /// <param name="sConn">SQL Server連線字串</param>
        /// <param name="sTableName">Table名稱(可不輸入)</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDt ( string sSql , SqlConnection sConn , string sTableName = "TABLE" )
        {
            DataTable dt = new DataTable();
            ConnectionState BakState = sConn.State;
            try
            {
                if (sConn.State == ConnectionState.Closed)
                {
                    sConn.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter(sSql, sConn);
                da.Fill(dt);
                if (sTableName != "TABLE")
                {
                    dt.TableName = sTableName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (BakState == ConnectionState.Closed)
                {
                    sConn.Close();
                }
            }
            return dt;
        }

        /// <summary> 取得DataTable(OleDB連線) </summary>
        /// <param name="sSql">SQL子句</param>
        /// <param name="sConn">OleDB連線字串</param>
        /// <param name="sTableName">Table名稱(可不輸入)</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDt ( string sSql , OleDbConnection sConn , string sTableName = "TABLE" )
        {
            DataTable dt = new DataTable();
            try
            {
                sConn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(sSql , sConn);
                da.Fill(dt);
                if (sTableName != "TABLE")
                {
                    dt.TableName = sTableName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sConn.Close();
            }
            return dt;
        }
        
        # endregion

        /// <summary> 傳入SQL語法以變更資料庫資料 </summary>
        /// <param name="sDBSql">SQL子句</param>
        public static int ExecuteSQL ( string sDBSql )
        {
            SqlConnection conn = new SqlConnection(Conn);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sDBSql , conn);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
