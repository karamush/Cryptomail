/*
 * Developer Radik Khaydarov, http://rjump.net
 * */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace CryptoMail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormReg FR = new FormReg();
            FR.ShowDialog();
        }
        static string sha256(string password)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "") { return; }
            
            if (File.Exists("db.db3") == false)
            {
                String def = sha256("123456");
                SQLLiteconnect sql = new SQLLiteconnect("db.db3");
                sql.SQLTransact("CREATE TABLE MyTable (pass String, pubkey String, privkey String)");                
                sql.SQLTransact("CREATE TABLE MyContacts (name String, publickey String)");
                sql.SQLTransact("CREATE TABLE PubKeySite (name String)");
                sql.SQLTransact("INSERT INTO MyTable (pass,pubkey,privkey) VALUES('" + def +"','','')");
                sql.SQLTransact("CREATE TABLE MyMail(S1 String, P1 String, W1 String, L1 String, S2 String, P2 String, W2 String, L2 String,id Int)");
                sql.SQLTransact("INSERT INTO MyMail (S1,S2,W1,W2,L1,L2,P1,P2,id) VALUES('','','','','','','993','25',0)");
                sql.SQLTransact("INSERT INTO PubKeySite (name) VALUES('http://localhost:8080')");

            }
            else
            {
                String def = sha256(textBox1.Text.Trim());
                DataTable tbl = new DataTable();
                SQLLiteconnect sql = new SQLLiteconnect("db.db3");
                tbl= sql.SelectTable("select * from MyTable where pass='"+def+"'");
                if (tbl.Rows.Count == 0) { return; }

            }


            FormMain FM = new FormMain(textBox1.Text.Trim());
            FM.FormClosed += FM_FormClosed;
            this.Hide();
            FM.Show();
        }

        void FM_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
