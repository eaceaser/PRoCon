using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Accounts {
    using Core.Remote;
    public class Account {

        public delegate void AccountPasswordChangedHandler(Account item);
        public event AccountPasswordChangedHandler AccountPasswordChanged;
        /*
        public AccountPrivilegeDictionary AccountPrivileges {
            get;
            private set;
        }
        */
        public string Name {
            get;
            private set;
        }

        private string m_strPassword;
        public string Password {
            get {
                return this.m_strPassword;
            }
            set {
                this.m_strPassword = value;

                if (this.AccountPasswordChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.AccountPasswordChanged.GetInvocationList(), this);
                }
            }
        }

        public Account(string strName, string strPassword) {
            //this.AccountPrivileges = new AccountPrivilegeDictionary();

            this.Name = strName;
            this.Password = strPassword;
        }
    }
}
