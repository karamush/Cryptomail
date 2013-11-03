/*
 * Developer Radik Khaydarov, http://rjump.net
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace CryptoMail
{
    class SQLLiteconnect
    {

        private SQLiteConnection connection;

        private string basename;

        public bool ConnectSQLLite()
        {
            try
            {
                connection = new SQLiteConnection("Data Source=" + basename);                
                return true;
            }
            catch
            {
                CloseSQLLite();
                return false;
            }
        }

        public void CloseSQLLite()
        {
            connection.Close();
        }


        public DataTable SelectTable(String str_sql)//вывод таблицы
        {
            try
            {
                ConnectSQLLite();
                connection.Open();
                DataSet myDataSet = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(str_sql, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable dt1 = new DataTable();
                adapter.Fill(dt1);
                CloseSQLLite();
                return dt1;
            }
            catch { return null; }
        }



        public bool SQLTransact(String str_sql)//SQL транзакция
        {
            try
            {
                ConnectSQLLite();
                connection.Open();
                using (SQLiteTransaction mytransaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(connection))
                    {
                        mycommand.CommandText = str_sql;
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }


                CloseSQLLite();
                return true;
            }
            catch { CloseSQLLite(); return false; }
        }

        public bool SQLTransactParam(String str_sql,List<string> ListParam)//SQL транзакция с параметрами
        {
            try
            {
                ConnectSQLLite();
                connection.Open();
                using (SQLiteTransaction mytransaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(connection))
                    {
                        mycommand.CommandText = str_sql;

                        foreach (string param in ListParam)
                        {
                            SQLiteParameter myparam = new SQLiteParameter();
                            myparam.Value = param;
                            mycommand.Parameters.Add(myparam);
                        }

                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }


                CloseSQLLite();
                return true;
            }
            catch { CloseSQLLite(); return false; }
        }


        public SQLLiteconnect(string Basename) {
            basename = Basename;
        }
    }
}
