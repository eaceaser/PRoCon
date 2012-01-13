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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace PRoCon.Core.Battlemap {
    [Serializable]
    public class ZoneTagList : Collection<string> {

        public delegate void TagsEditedHandler(ZoneTagList sender);
        public event TagsEditedHandler TagsEdited;

        private bool m_isInternallyEditing;

        public ZoneTagList() {

        }

        public ZoneTagList(string tagList) {
            if (tagList.Length > 0) {
                this.FromString(tagList);
            }
        }

        protected override void InsertItem(int index, string item) {
            base.InsertItem(index, item.Trim());

            if (this.TagsEdited != null && this.m_isInternallyEditing == false) {
                this.TagsEdited(this);
            }
        }

        protected override void RemoveItem(int index) {
            base.RemoveItem(index);

            if (this.TagsEdited != null && this.m_isInternallyEditing == false) {
                this.TagsEdited(this);
            }
        }

        protected override void SetItem(int index, string item) {
            base.SetItem(index, item);

            if (this.TagsEdited != null && this.m_isInternallyEditing == false) {
                this.TagsEdited(this);
            }
        }

        public new bool Remove(string item) {

            bool isRemoved = false;
            string removeKey = String.Empty;
            foreach (string zoneTag in this) {
                if ((isRemoved = (String.Compare(zoneTag, item, true) == 0)) == true) {
                    removeKey = zoneTag;
                    break;
                }
            }

            base.Remove(removeKey);

            return isRemoved;
        }

        public new bool Contains(string item) {

            bool isListed = false;

            foreach (string zoneTag in this) {
                if ((isListed = (String.Compare(zoneTag, item, true) == 0)) == true) {
                    break;
                }
            }

            return isListed;
        }

        public void FromString(string tagList) {

            this.m_isInternallyEditing = true;

            this.Clear();

            foreach (string zoneTag in tagList.Split(';')) {
                //if (zoneTag.Trim().Length > 0) {
                    this.Add(zoneTag.Trim());
                //}
            }

            if (this.TagsEdited != null) {
                this.TagsEdited(this);
            }

            this.m_isInternallyEditing = false;
        }

        public override string ToString() {
            string returnString = String.Empty;
            bool hasLoopedOnce = false;

            foreach (string zoneTag in new List<string>(this)) {

                if (hasLoopedOnce == true) {
                    returnString += "; ";
                }

                returnString += zoneTag;

                hasLoopedOnce = true;
            }

            return returnString;
        }

    }
}
