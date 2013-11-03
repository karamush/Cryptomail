/*
 * Developer Radik Khaydarov, http://rjump.net
 * */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CryptoMail
{
    class ServerAPIClass
    {

        public static string ReadPubKey(string email)//считываем pubkey
        {
            string site = "http://localhost:8080";
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3"); 
            tbl = sql.SelectTable("select name from PubKeySite");
            if (tbl.Rows[0][0].ToString().Trim().Length > 0)
            {
                site = tbl.Rows[0][0].ToString().Trim();
            }

            string result = "";
            string url = site+"/readpubkey.php";
            WebResponse response = null;
            System.IO.StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                string postData = "mail=" + email; 
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);                
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            catch
            {
               
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }
            return result;
        }

        public static string SendPubKey(string email, string pubkey)//отправляем pubkey
        {

            string site = "http://localhost:8080";
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            tbl = sql.SelectTable("select name from PubKeySite");
            if (tbl.Rows[0][0].ToString().Trim().Length > 0)
            {
                site = tbl.Rows[0][0].ToString().Trim();
            }
            
            string result = "";
            string url = site+"/sendpubkey.php";

            WebResponse response = null;
            System.IO.StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                string postData = "mail=" + email + "&pubkey=" + pubkey;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            catch
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }
            return result; 
        }

    }
}
