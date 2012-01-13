using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core {
    [Serializable]
    public class TimeoutSubset {

        public enum TimeoutSubsetType {
            None,
            Permanent,
            Round,
            Seconds,
        }

        public TimeoutSubsetType Subset {
            get;
            private set;
        }

        public int Seconds {
            get;
            set;
        }

        public TimeoutSubset(List<string> lstTimeoutSubsetWords) {

            this.Subset = TimeoutSubsetType.None;
            int iLength = 0;

            if (String.Compare(lstTimeoutSubsetWords[0], "perm") == 0) {
                this.Subset = TimeoutSubsetType.Permanent;
            }
            else if (String.Compare(lstTimeoutSubsetWords[0], "round") == 0) {
                this.Subset = TimeoutSubsetType.Round;
            }
            else if (lstTimeoutSubsetWords.Count == 2 && String.Compare(lstTimeoutSubsetWords[0], "seconds") == 0 && int.TryParse(lstTimeoutSubsetWords[1], out iLength) == true) {
                this.Subset = TimeoutSubsetType.Seconds;
                this.Seconds = iLength;
            }
        }

        public TimeoutSubset(TimeoutSubsetType enTimeoutType) {
            this.Subset = enTimeoutType;
        }

        // Punkbuster..
        public TimeoutSubset(string strLength, string strServed) {

            int iServed = 0, iLength = 0;

            if (String.Compare(strServed, "0") == 0 || String.Compare(strServed, "-1") == 0) {
                this.Subset = TimeoutSubsetType.Permanent;
            }
            else if (int.TryParse(strServed, out iServed) == true && int.TryParse(strLength, out iLength) == true) {
                this.Subset = TimeoutSubsetType.Seconds;
                this.Seconds = (iLength - iServed) * 60;
            }
        }

        public TimeoutSubset(TimeoutSubsetType enTimeoutType, int iSeconds) {
            this.Subset = enTimeoutType;
            this.Seconds = iSeconds;
        }


        public static int RequiredLength(string strSubsetType) {

            int iRequiredLength = 1;

            if (String.Compare(strSubsetType, "seconds") == 0) {
                iRequiredLength = 2;
            }
            // perm and round only need a List<string> with 1 string in it.

            return iRequiredLength;
        }
    }
}
