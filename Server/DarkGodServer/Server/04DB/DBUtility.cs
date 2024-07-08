using MySql.Data.MySqlClient;
using PENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server._04DB
{
    public class DBUtility
    {
        //查询的类型要与数据库的类型一致
        public static T Query<T>(string sql, SqlParam[] sqlParams, MySqlConnection sqlConnection, T res)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(sql, sqlConnection);
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    command.Parameters.AddWithValue(sqlParams[i].name, sqlParams[i].value);
                }
                using (MySqlDataReader dataReader = command.ExecuteReader())
                {
                    FieldInfo[] infos = res.GetType().GetFields();
                    if (dataReader.Read())
                    {
                        foreach (FieldInfo pi in infos)
                        {
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                if (pi.Name.Equals(dataReader.GetName(i)))
                                {
                                    pi.SetValue(res, dataReader[i]);
                                    break;
                                }
                            }
                        }
                    }
                    return res;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Get Data Fail     " + e.Message);
            }
        }
        public static int Query(string sql, SqlParam[] sqlParams, MySqlConnection sqlConnection)
        {
            int res = -1;
            try
            {
                MySqlCommand command = new MySqlCommand(sql, sqlConnection);
              
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    command.Parameters.AddWithValue(sqlParams[i].name, sqlParams[i].value);
                }
                MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read()) {
                    res= (int)dataReader.GetValue(0);
                }
                return res;
            }
            catch (Exception e)
            {
                throw new Exception("Get Data Fail     " + e.Message);
            }
        }
        public static bool NoQuery(string sql, SqlParam[] sqlParams, MySqlConnection sqlConnection)
        {
            MySqlCommand command=null;
            try
            {
                command = new MySqlCommand(sql, sqlConnection);
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    command.Parameters.AddWithValue(sqlParams[i].name, sqlParams[i].value);
                }
               
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(command.CommandText+"   "+command.Parameters.ToString()+"     "+command.ToString());
                throw new Exception("Update Data Fail   " + e.Message);
            }
        }
        public static void CloseConnect(MySqlConnection mySqlConnection) {
            if (mySqlConnection != null) {
                mySqlConnection.Dispose();
                mySqlConnection.Close();
            }
        }
    }
    public class SqlParam
    {
        public string name;
        public Object value;
        public SqlParam(string name, object value)
        {
            {
                this.name = name;
                this.value = value;
            }
        }
    }
}
