using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Tests.Events
{
    using Core.Events;
    using NUnit.Framework;

    [TestFixture]
    public class CapturedEventTests
    {
        [Test]
        public void TestCreation ()
        {
            var date = DateTime.Now;
            var capturedEvent = new CapturedEvent(EventType.Plugins, CapturableEvents.PluginLoaded, "a text", date, "");

            Assert.AreEqual(EventType.Plugins, capturedEvent.eType);
            Assert.AreEqual(CapturableEvents.PluginLoaded, capturedEvent.Event);
            Assert.AreEqual("a text", capturedEvent.EventText);
            Assert.AreEqual(date, capturedEvent.LoggedTime);
        }
    }
}
