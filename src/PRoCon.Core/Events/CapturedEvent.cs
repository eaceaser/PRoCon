using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Events {
    public class CapturedEvent {

        public EventType eType { get; private set; }

        public CapturableEvents Event { get; private set; }

        public string EventText { get; private set; }

        public DateTime LoggedTime { get; set; }

        public string InstigatingAdmin { get; private set; }

        public CapturedEvent(EventType etType, CapturableEvents ceEvent, string strEventText, DateTime dtLoggedTime, string instigatingAdmin) {
            this.eType = etType;
            this.Event = ceEvent;
            this.EventText = strEventText;
            this.LoggedTime = dtLoggedTime;

            this.InstigatingAdmin = instigatingAdmin;
        }
    }
}
