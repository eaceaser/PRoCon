/*  Copyright 2010 Geoffrey 'Phogue' Green

    http://www.phogue.net
 
    This file is part of PRoCon Frostbite.

    PRoCon Frostbite is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PRoCon Frostbite is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with PRoCon Frostbite.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PRoCon.Core.Accounts {
    using Core.Remote;
    public class AccountDictionary : KeyedCollection<string, Account> {

        public delegate void AccountAlteredHandler(Account item);
        public event AccountAlteredHandler AccountAdded;
        public event AccountAlteredHandler AccountChanged;
        public event AccountAlteredHandler AccountRemoved;

        protected override string GetKeyForItem(Account item) {
            return item.Name;
        }

        protected override void InsertItem(int index, Account item) {
            base.InsertItem(index, item);

            if (this.AccountAdded != null) {
                FrostbiteConnection.RaiseEvent(this.AccountAdded.GetInvocationList(), item);
            }
        }

        protected override void RemoveItem(int index) {

            if (this.AccountRemoved != null) {
                FrostbiteConnection.RaiseEvent(this.AccountRemoved.GetInvocationList(), this[index]);
            }

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Account item) {
            if (this.AccountChanged != null) {
                FrostbiteConnection.RaiseEvent(this.AccountChanged.GetInvocationList(), item);
            }

            base.SetItem(index, item);
        }

        public void CreateAccount(string strUsername, string strPassword) {
            if (this.Contains(strUsername) == true) {
                this[strUsername].Password = strPassword;
            }
            else {
                Account accNewAccount = new Account(strUsername, strPassword);

                // Temporary until privileges can be extracted from here and into the layer.
                /*
                foreach (Account accCurrentAccount in this) {
                    foreach (AccountPrivilege accCurrentPrivilege in accCurrentAccount.AccountPrivileges) {
                        if (accNewAccount.AccountPrivileges.Contains(accCurrentPrivilege.HostNamePort) == false) {
                            accNewAccount.AccountPrivileges.Add(new AccountPrivilege(accNewAccount, accCurrentPrivilege.HostNamePort, new CPrivileges()));
                        }
                    }
                }
                */
                this.Add(accNewAccount);
            }
        }

        public void DeleteAccount(string strUsername) {
            if (this.Contains(strUsername) == true) {
                this.Remove(strUsername);
            }
        }

        public void ChangePassword(string strUsername, string strPassword) {
            if (this.Contains(strUsername) == true) {
                this[strUsername].Password = strPassword;
            }
        }

        public List<string> ListAccountNames() {
            return new List<string>(this.Dictionary.Keys);
        }

    }
}
