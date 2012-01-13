using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Accounts {
    using Core.Remote;
    public class AccountPrivilegeDictionary : KeyedCollection<string, AccountPrivilege> {

        public delegate void AccountPrivilegeAlteredHandler(AccountPrivilege item);
        public event AccountPrivilegeAlteredHandler AccountPrivilegeAdded;
        public event AccountPrivilegeAlteredHandler AccountPrivilegeChanged;
        public event AccountPrivilegeAlteredHandler AccountPrivilegeRemoved;

        protected override string GetKeyForItem(AccountPrivilege item) {
            return item.Owner.Name;
        }

        protected override void InsertItem(int index, AccountPrivilege item) {
            base.InsertItem(index, item);

            if (this.AccountPrivilegeAdded != null) {
                FrostbiteConnection.RaiseEvent(this.AccountPrivilegeAdded.GetInvocationList(), item);
            }
        }

        protected override void RemoveItem(int index) {
            AccountPrivilege apRemoved = this[index];

            base.RemoveItem(index);

            if (this.AccountPrivilegeRemoved != null) {
                FrostbiteConnection.RaiseEvent(this.AccountPrivilegeRemoved.GetInvocationList(), apRemoved);
            }
        }

        protected override void SetItem(int index, AccountPrivilege item) {
            if (this.AccountPrivilegeChanged != null) {
                FrostbiteConnection.RaiseEvent(this.AccountPrivilegeChanged.GetInvocationList(), item);
            }

            base.SetItem(index, item);
        }
    }
}
