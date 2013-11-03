/*
 * Developer Radik Khaydarov, http://rjump.net
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoMail
{
    class MailObjClass
    {
        private string nummail;
        public string Nummail
        {
            get { return nummail; }
            set
            {
                nummail = value;
            }
        }

        private string datesend;
        public string Datesend
        {
            get { return datesend; }
            set
            {
                datesend = value;
            }
        }

        private string from;
        public string Mailfrom
        {
            get { return from; }
            set
            {
                from = value;
            }
        }
                
        public MailObjClass() { }
    }
}
