/*
 * Developer Radik Khaydarov, http://rjump.net
 * */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace CryptoMail
{
    public partial class FormMain : Form
    {
        public String pass;
        public String S1="";
        public String S2 = "";
        public String L1 = "";
        public String L2 = "";
        public String W1 = "";
        public String W2 = "";
        public String P1 = "";
        public String P2 = "";
        public String from = "";
        public String datesend = "";
        public String nummail = "";
        public StringBuilder sb;
        public String pubKey = "";
        public String privKey = "";
        private List<string> LS;
        Boolean t = false;

        public String DateFormat(DateTime DT)
        {
            String R = "";
            //31-Oct-2013
            String day = Convert.ToString(DT.Day);
            String god = Convert.ToString(DT.Year);
            String mes = "";
            switch (DT.Month)
            {
                case 1: mes = "Jan";
                    break;
                case 2: mes = "Feb";
                    break;
                case 3: mes = "Mar";
                    break;
                case 4: mes = "Apr";
                    break;
                case 5: mes = "May";
                    break;
                case 6: mes = "Jun";
                    break;
                case 7: mes = "Jul";
                    break;
                case 8: mes = "Aug";
                    break;
                case 9: mes = "Sep";
                    break;
                case 10: mes = "Oct";
                    break;
                case 11: mes = "Nov";
                    break;
                case 12: mes = "Dec";
                    break;
            }

            R = day + "-" + mes + "-" + god;
            return R;
        }


        public FormMain(String Pass )
        {
            InitializeComponent();
            pass = Pass; 
            LS = new List<string>();
            date1.Value = DateTime.Now;
            date2.Value = DateTime.Now.AddDays(1);
        }

        public void Loadheader(){
            listBox1.Items.Clear();

            String Period1 = DateFormat(date1.Value);
            String Period2 = DateFormat(date2.Value);
            LS.Clear();
            ReadCryptoMailSearch(Period1, Period2);
            try
            {
                String[] S = LS[0].Split(' ');
                if (S.Length > 2)
                {
                    ReadCryptoMailHeader(S);
                }
                MessageBox.Show("OK");
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            FormOptions FO = new FormOptions(pass);
            FO.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //RSA key Gen and sync
            RSAForm RSF = new RSAForm(pass);
            RSF.ShowDialog();
        }

        private void ReadResponse(string tag, System.IO.StreamReader sr)//imap response
        {
            string response;
            while ((response = sr.ReadLine()) != null)
            {

                if (tag == "tag4")
                {
                   // richTextBox1.Text += response + "\r\n"; 

                    if (response.IndexOf(")") == 0)
                    {
                        t = false;
                    }
                    
                    if ((response.IndexOf("(BODY[TEXT]") == -1) && (t == true))
                    {
                        sb.Append(response);
                    }
                    
                }                
               
                if (tag == "tag5")
                { 
                    //richTextBox1.Text += response + "\r\n"; 
                }
                

                if (tag == "tag2")
                { 
                  //  richTextBox1.Text += response + "\r\n";

                    LS.Add(response);
                }

                if (response.StartsWith(tag, StringComparison.Ordinal))
                {
                    break;
                }
            }
        }

        private void ReadResponseMail(string tag, System.IO.StreamReader sr)//create list mailobject
        {
            string response;
            string s = "";
            int i = 0; int j = 0;

            while ((response = sr.ReadLine()) != null)
            {
               // richTextBox1.Text += response + "\r\n";
                    if (response.IndexOf("From:") == 0)
                    {
                        i = response.IndexOf("<") + 1;
                        j = response.IndexOf(">");
                        if (i == 0 || j == 0) { s = response.Substring(6); }
                        else
                        {
                            s = response.Substring(i, j - i);
                        }
                        from = s.Trim();
                    }

                    if (response.IndexOf("Date:") == 0)
                    {
                        datesend = response.Substring(5).Trim();
                    }

                    if ((response.IndexOf("Subject: Cryptomail") == 0) || (response.IndexOf("Subject: =?UTF-8?B?Q3J5cHRvbWFpbA==?=") == 0))
                    {
                        //create obj list
                        MailObjClass MO = new MailObjClass();
                        MO.Nummail = nummail;
                        MO.Datesend = datesend;
                        MO.Mailfrom = from;
                        listBox1.DisplayMember = "Mailfrom";
                        listBox1.Items.Add(MO);
                    }
                

                if (response.StartsWith(tag, StringComparison.Ordinal))
                {
                    break;
                }
            }
        }


        private void ReadCryptoMailHeader(String[] Num)//imap command read header mail
        {
            TcpClient tcpclient = new TcpClient();
            tcpclient.Connect(S1, Convert.ToInt32(P1));

            SslStream sslstream = new SslStream(tcpclient.GetStream());
            sslstream.AuthenticateAsClient(S1);

            if (sslstream.IsAuthenticated)
            {
                StreamWriter sw = new StreamWriter(sslstream);
                StreamReader sr = new StreamReader(sslstream);

                sw.WriteLine("0 LOGIN " + L1 + " " + W1);
                sw.Flush();
                ReadResponse("0", sr);

                sw.WriteLine("1 SELECT inbox");
                sw.Flush();
                ReadResponse("1", sr);
                for (int i=2;i<Num.Length;i++){
                    nummail = Num[i];
                   
                    sw.WriteLine("tag"+i.ToString()+" FETCH " + nummail + " body[header]\r\n");                    
                    sw.Flush();
                    ReadResponseMail("tag" + i.ToString(), sr);
                    }
                
                sw.WriteLine(Num.Length.ToString()+" LOGOUT");
                sw.Flush();
                ReadResponse(Num.Length.ToString(), sr);

            }

        }

        private void ReadCryptoMail(int number)//imap command read body mail
        {
            TcpClient tcpclient = new TcpClient();
            tcpclient.Connect(S1, Convert.ToInt32(P1));

            SslStream sslstream = new SslStream(tcpclient.GetStream());
            sslstream.AuthenticateAsClient(S1);

            if (sslstream.IsAuthenticated)
            {
                StreamWriter sw = new StreamWriter(sslstream);
                StreamReader sr = new StreamReader(sslstream);

                sw.WriteLine("tag LOGIN " + L1 + " " + W1);
                sw.Flush();
                ReadResponse("tag", sr);

                sw.WriteLine("tag1 SELECT inbox");
                sw.Flush();
                ReadResponse("tag1", sr);

                sw.WriteLine("tag4 FETCH " + number + " body[text]\r\n");
                sw.Flush();
                ReadResponse("tag4", sr);
                sw.WriteLine("tag0 LOGOUT");
                sw.Flush();
                ReadResponse("tag0", sr);
            }

        }

        private void DelCryptoMail(int number)//imap command delete mail
        {
            TcpClient tcpclient = new TcpClient();
            tcpclient.Connect(S1, Convert.ToInt32(P1));

            SslStream sslstream = new SslStream(tcpclient.GetStream());
            sslstream.AuthenticateAsClient(S1);

            if (sslstream.IsAuthenticated)
            {
                StreamWriter sw = new StreamWriter(sslstream);
                StreamReader sr = new StreamReader(sslstream);

                sw.WriteLine("tag LOGIN " + L1 + " " + W1);
                sw.Flush();
                ReadResponse("tag", sr);

                sw.WriteLine("tag1 SELECT inbox");
                sw.Flush();
                ReadResponse("tag1", sr);

                sw.WriteLine("tag5 store "+number+" +flags ("+@"\Deleted)");
                sw.Flush();
                ReadResponse("tag5", sr);
                sw.WriteLine("tag0 LOGOUT");
                sw.Flush();
                ReadResponse("tag0", sr);
                
            }
        }


        private void button1_Click(object sender, EventArgs e)//load header mail
        {
            if (date1.Value > date2.Value)
            {
                MessageBox.Show("ERROR period.");
                return;
            }
            sb = new StringBuilder();
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            tbl = sql.SelectTable("select S1,S2,W1,W2,L1,L2,P1,P2 from MyMail");

            if (tbl.Rows[0][0].ToString().Trim().Length > 0)
            {
                S1 = DesClass.Decrypt(tbl.Rows[0][0].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][1].ToString().Trim().Length > 0)
            {
                S2 = DesClass.Decrypt(tbl.Rows[0][1].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][2].ToString().Trim().Length > 0)
            {
                W1 = DesClass.Decrypt(tbl.Rows[0][2].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][3].ToString().Trim().Length > 0)
            {
                W2 = DesClass.Decrypt(tbl.Rows[0][3].ToString().Trim(), pass);
            }


            if (tbl.Rows[0][4].ToString().Trim().Length > 0)
            {
                L1 = DesClass.Decrypt(tbl.Rows[0][4].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][5].ToString().Trim().Length > 0)
            {
                L2 = DesClass.Decrypt(tbl.Rows[0][5].ToString().Trim(), pass);
            }

            P1 = tbl.Rows[0][6].ToString();
            P2 = tbl.Rows[0][7].ToString();

            if (S1 == "") { MessageBox.Show("Options failed"); return; }
            Loadheader();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)//read body mail
        {
            //select from
            if (listBox1.SelectedIndex == -1) { return; }
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            tbl = sql.SelectTable("select privkey from MyTable");

            if (tbl.Rows[0][0].ToString().Trim().Length > 0)
            {
                privKey = DesClass.Decrypt(tbl.Rows[0][0].ToString().Trim(), pass);
            }
            
            int k = 0;
            MailObjClass MO = (MailObjClass)listBox1.Items[listBox1.SelectedIndex];
            richTextBox1.Clear();
            richTextBox1.Text += "From: " + MO.Mailfrom + "\r\n";
            richTextBox1.Text += "Date: " + MO.Datesend + "\r\n";
            richTextBox1.Text += "\r\n";
            t = true; 
            sb = new StringBuilder();            
            ReadCryptoMail(Convert.ToInt32(MO.Nummail));
            String bodymail2 = "";
            String bodymail = sb.ToString();
            //richTextBox2.Text = sb.ToString();
            while (bodymail.IndexOf("=") > 0)
            {
                k = bodymail.IndexOf("==");
            if (k <= 0)
            {
                k = bodymail.IndexOf("=");
                if (k > 0)
                {
                    bodymail2 = bodymail.Substring(0, k + 1) ;
                    bodymail = bodymail.Remove(0, k + 1);
                    if (bodymail2.Length > 3)
                    {
                        //  MessageBox.Show(Parts[i]);
                        String part = bodymail2.Replace(".", "");
                        if (part.IndexOf("3D") == 0)
                        {
                            part = part.Remove(0, 2);
                        }
                        richTextBox1.Text += DesClass.RSADec(part, privKey);

                    }
                }
            }
            else
            {
                bodymail2 = bodymail.Substring(0, k + 2);
                bodymail = bodymail.Remove(0, k + 2);
                if (bodymail2.Length > 3)
                {
                    //  MessageBox.Show(Parts[i]);
                    String part = bodymail2.Replace(".", "");
                    if (part.IndexOf("3D") == 0)
                    {
                        part = part.Remove(0, 2);
                    }
                    richTextBox1.Text += DesClass.RSADec(part, privKey);

                }
            }
            }
          
            //----------------bodymail;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) { return; } 
            if (MessageBox.Show("Delete the selected cryptomail?", "Delete cryptomail", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                richTextBox1.Text = ""; 
                MailObjClass MO = (MailObjClass)listBox1.Items[listBox1.SelectedIndex];
                DelCryptoMail(Convert.ToInt32(MO.Nummail));                
                Loadheader();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //send
            SendMailForm SMF = new SendMailForm(pass,"","");
            SMF.ShowDialog();
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (listBox1.SelectedIndex == -1) { return; }
            
            if (e.ClickedItem == contextMenuStrip1.Items[0])//read body mail
            {
                //answer
                if (listBox1.SelectedIndex == -1) { return; }
                DataTable tbl = new DataTable();
                SQLLiteconnect sql = new SQLLiteconnect("db.db3");
                tbl = sql.SelectTable("select privkey from MyTable");

                if (tbl.Rows[0][0].ToString().Trim().Length > 0)
                {
                    privKey = DesClass.Decrypt(tbl.Rows[0][0].ToString().Trim(), pass);
                }

                int k = 0;
                MailObjClass MO = (MailObjClass)listBox1.Items[listBox1.SelectedIndex];
                richTextBox1.Clear();
                richTextBox1.Text += "From: " + MO.Mailfrom + "\r\n";
                richTextBox1.Text += "Date: " + MO.Datesend + "\r\n";
                richTextBox1.Text += "\r\n";
                t = true;
                sb = new StringBuilder();
                ReadCryptoMail(Convert.ToInt32(MO.Nummail));
                String bodymail2 = "";
                String bodymail = sb.ToString();
                //richTextBox2.Text = sb.ToString();
                while (bodymail.IndexOf("=") > 0)
                {
                    k = bodymail.IndexOf("==");
                    if (k <= 0)
                    {
                        k = bodymail.IndexOf("=");
                        if (k > 0)
                        {
                            bodymail2 = bodymail.Substring(0, k + 1);
                            bodymail = bodymail.Remove(0, k + 1);
                            if (bodymail2.Length > 3)
                            {
                                //  MessageBox.Show(Parts[i]);
                                String part = bodymail2.Replace(".", "");
                                if (part.IndexOf("3D") == 0)
                                {
                                    part = part.Remove(0, 2);
                                }
                                richTextBox1.Text += DesClass.RSADec(part, privKey);

                            }
                        }
                    }
                    else
                    {
                        bodymail2 = bodymail.Substring(0, k + 2);
                        bodymail = bodymail.Remove(0, k + 2);
                        if (bodymail2.Length > 3)
                        {
                            //  MessageBox.Show(Parts[i]);
                            String part = bodymail2.Replace(".", "");
                            if (part.IndexOf("3D") == 0)
                            {
                                part = part.Remove(0, 2);
                            }
                            richTextBox1.Text += DesClass.RSADec(part, privKey);

                        }
                    }
                }
               
                //----------------bodymail;                
                SendMailForm SMF = new SendMailForm(pass, MO.Mailfrom, MO.Datesend + "\r\n" + richTextBox1.Text);

                SMF.ShowDialog();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Contacts
            ContactsForm CF = new ContactsForm(pass);
            CF.ShowDialog();
        }

        private void ReadCryptoMailSearch(String Period1, String Period2)//imap command search
        {
            TcpClient tcpclient = new TcpClient();
            tcpclient.Connect(S1, Convert.ToInt32(P1));

            SslStream sslstream = new SslStream(tcpclient.GetStream());
            sslstream.AuthenticateAsClient(S1);

            if (sslstream.IsAuthenticated)
            {
                StreamWriter sw = new StreamWriter(sslstream);
                StreamReader sr = new StreamReader(sslstream);

                sw.WriteLine("tag LOGIN " + L1 + " " + W1);
                sw.Flush();
                ReadResponse("tag", sr);

                sw.WriteLine(@"tag1 SELECT inbox");
                sw.Flush();
                ReadResponse("tag1", sr);

               // sw.WriteLine(@"tag6 SEARCH SINCE 31-Oct-2013");
                sw.WriteLine(@"tag2 SEARCH SINCE " + Period1 + " BEFORE "+Period2);
                sw.Flush();
                ReadResponse("tag2", sr);


                sw.WriteLine("tag0 LOGOUT");
                sw.Flush();
                ReadResponse("tag0", sr);
            }
        }
    }
}
