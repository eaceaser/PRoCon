using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace PRoCon.Core.Localization {
    using Core.Remote;
    public class LocalizationDictionary : KeyedCollection<string, CLocalization> {

        public delegate void AccountAlteredHandler(CLocalization item);
        public event AccountAlteredHandler LanguageAdded;
        public event AccountAlteredHandler LanguageRemoved;

        protected override string GetKeyForItem(CLocalization item) {
            return item.FileName;
        }

        protected override void InsertItem(int index, CLocalization item) {
            base.InsertItem(index, item);

            if (this.LanguageAdded != null) {
                FrostbiteConnection.RaiseEvent(this.LanguageAdded.GetInvocationList(), item);
            }
        }

        protected override void RemoveItem(int index) {
            CLocalization clocRemoved = this[index];

            base.RemoveItem(index);

            if (this.LanguageRemoved != null) {
                FrostbiteConnection.RaiseEvent(this.LanguageRemoved.GetInvocationList(), clocRemoved);
            }
        }

        public CLocalization LoadLocalizationFile(string fullFilePath, string localizationFileName) {

            CLocalization clocLoadedLanguage = new CLocalization(fullFilePath, localizationFileName);

            if (this.Contains(clocLoadedLanguage.FileName) == false) {
                this.Add(clocLoadedLanguage);
            }
            else {
                this.SetItem(this.IndexOf(this[clocLoadedLanguage.FileName]), clocLoadedLanguage);
            }

            return clocLoadedLanguage;
        }
    }
}
