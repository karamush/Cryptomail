/*
 * Developer Radik Khaydarov, http://rjump.net
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoMail
{
    class ContactClass
    {
        private string nummail;
        public string Publickey
        {
            get { return nummail; }
            set
            {
                nummail = value;
            }
        }        

        private string from;
        public string Mail
        {
            get { return from; }
            set
            {
                from = value;
            }
        }

        public ContactClass() { }
    }
}
