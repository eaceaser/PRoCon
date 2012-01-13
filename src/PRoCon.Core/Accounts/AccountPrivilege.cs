using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Accounts {
    using Core.Remote;
    public class AccountPrivilege {

        public delegate void AccountPrivilegesChangedHandler(AccountPrivilege item);
        public event AccountPrivilegesChangedHandler AccountPrivilegesChanged;

        public Account Owner {
            get;
            private set;
        }

        public CPrivileges Privileges {
            get;
            private set;
        }

        public void SetPrivileges(CPrivileges cpUpdatedPrivileges) {
            this.Privileges = cpUpdatedPrivileges;

            if (this.AccountPrivilegesChanged != null) {
                FrostbiteConnection.RaiseEvent(this.AccountPrivilegesChanged.GetInvocationList(), this);
            }
        }

        public AccountPrivilege(Account accOwner, CPrivileges cpPrivileges) {
            this.Owner = accOwner;
            this.Privileges = cpPrivileges;
        }


    }
}
