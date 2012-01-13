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
using System.Text;

// This class will move to .Core once ProConClient is in .Core.
namespace PRoCon.Core.Consoles {
    using Core;
    using Core.Logging;
    using Core.Remote;

    public class PunkbusterConsole : Loggable {

        public event WriteConsoleHandler WriteConsole;

        private PRoConClient m_prcClient;

        public PunkbusterConsole(PRoConClient prcClient)
            : base() {

            this.m_prcClient = prcClient;

            this.FileHostNamePort = this.m_prcClient.FileHostNamePort;
            this.LoggingStartedPrefix = "Punkbuster logging started";
            this.LoggingStoppedPrefix = "Punkbuster logging stopped";
            this.FileNameSuffix = "punkbuster";

            this.m_prcClient.Game.PunkbusterMessage += new FrostbiteClient.PunkbusterMessageHandler(m_prcClient_PunkbusterMessage);
            this.m_prcClient.Game.SendPunkbusterMessage += new FrostbiteClient.SendPunkBusterMessageHandler(m_prcClient_SendPunkbusterMessage);
        }

        private void m_prcClient_SendPunkbusterMessage(FrostbiteClient sender, string punkbusterMessage) {
            this.Write("^2" + punkbusterMessage.TrimEnd('\r', '\n').Replace("{", "{{").Replace("}", "}}"));
        }

        private void m_prcClient_PunkbusterMessage(FrostbiteClient sender, string punkbusterMessage) {
            this.Write(punkbusterMessage.TrimEnd('\r', '\n').Replace("{", "{{").Replace("}", "}}"));
        }

        public void Write(string strFormat, params string[] a_objArguments) {

            DateTime dtLoggedTime = DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime();
            string strText = String.Format(strFormat, a_objArguments);

            this.WriteLogLine(String.Format("[{0}] {1}", dtLoggedTime.ToString("HH:mm:ss"), strText));

            if (this.WriteConsole != null) {
                FrostbiteConnection.RaiseEvent(this.WriteConsole.GetInvocationList(), dtLoggedTime, strText);
            }
        }
    }
}
