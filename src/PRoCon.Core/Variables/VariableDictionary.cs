﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PRoCon.Core.Variables {
    using Core.Remote;
    public class VariableDictionary : KeyedCollection<string, Variable> {

        public delegate void PlayerAlteredHandler(Variable item);
        public event PlayerAlteredHandler VariableAdded;
        public event PlayerAlteredHandler VariableUpdated;
        public event PlayerAlteredHandler VariableRemoved;

        protected override string GetKeyForItem(Variable item) {
            return item.Name;
        }

        protected override void InsertItem(int index, Variable item) {
            if (this.VariableAdded != null) {
                FrostbiteConnection.RaiseEvent(this.VariableAdded.GetInvocationList(), item);
            }

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index) {

            if (this.VariableRemoved != null) {
                FrostbiteConnection.RaiseEvent(this.VariableRemoved.GetInvocationList(), this[index]);
            }

            base.RemoveItem(index);
        }
        
        protected override void SetItem(int index, Variable item) {
            if (this.VariableUpdated != null) {
                FrostbiteConnection.RaiseEvent(this.VariableUpdated.GetInvocationList(), item);
            }

            base.SetItem(index, item);
        }

        public T GetVariable<T>(string strVariable, T tDefault) {
            T tReturn = tDefault;

            if (this.Contains(strVariable) == true) {
                tReturn = this[strVariable].ConvertValue<T>(tDefault);
            }

            return tReturn;
        }

        public bool IsVariableNullOrEmpty(string strVariable) {
            bool blReturn = true;

            if (this.Contains(strVariable) == true) {
                blReturn = String.IsNullOrEmpty(this[strVariable].Value);
            }

            return blReturn;
        }

        public void SetVariable(string strVariable, string strValue) {
            if (this.Contains(strVariable) == true) {
                // TO DO: I doubt this will fire set event..
                this[strVariable].Value = strValue;

                if (this.VariableUpdated != null) {
                    FrostbiteConnection.RaiseEvent(this.VariableUpdated.GetInvocationList(), this[strVariable]);
                }
            }
            else {
                this.Add(new Variable(strVariable, strValue));
            }
        }
    }

}
