// 
// Copyright (c) 2004-2010 Jaroslaw Kowalski <jaak@jkowalski.net>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

using System;
using System.Xml;
using System.Reflection;
using System.Globalization;
using System.IO;

using NLog;
using NLog.Config;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NLog.UnitTests.LayoutRenderers
{
    [TestClass]
    public class MessageTests : NLogTestBase
    {
        [TestMethod]
        public void MessageWithoutPaddingTest()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${message}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>");

            Logger logger = LogManager.GetLogger("A");
            logger.Debug("a");
            AssertDebugLastMessage("debug", "a");
            logger.Debug("a{0}", 1);
            AssertDebugLastMessage("debug", "a1");
            logger.Debug("a{0}{1}", 1, "2");
            AssertDebugLastMessage("debug", "a12");
            logger.Debug(CultureInfo.InvariantCulture, "a{0}", new DateTime(2005, 1, 1));
            AssertDebugLastMessage("debug", "a01/01/2005 00:00:00");
        }

        [TestMethod]
        public void MessageRightPaddingTest()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${message:padding=3}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>");

            Logger logger = LogManager.GetLogger("A");
            logger.Debug("a");
            AssertDebugLastMessage("debug", "  a");
            logger.Debug("a{0}", 1);
            AssertDebugLastMessage("debug", " a1");
            logger.Debug("a{0}{1}", 1, "2");
            AssertDebugLastMessage("debug", "a12");
            logger.Debug(CultureInfo.InvariantCulture, "a{0}", new DateTime(2005, 1, 1));
            AssertDebugLastMessage("debug", "a01/01/2005 00:00:00");
        }


        [TestMethod]
        public void MessageFixedLengthRightPaddingTest()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${message:padding=3:fixedlength=true}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>");

            Logger logger = LogManager.GetLogger("A");
            logger.Debug("a");
            AssertDebugLastMessage("debug", "  a");
            logger.Debug("a{0}", 1);
            AssertDebugLastMessage("debug", " a1");
            logger.Debug("a{0}{1}", 1, "2");
            AssertDebugLastMessage("debug", "a12");
            logger.Debug(CultureInfo.InvariantCulture, "a{0}", new DateTime(2005, 1, 1));
            AssertDebugLastMessage("debug", "a01");
        }

        [TestMethod]
        public void MessageLeftPaddingTest()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${message:padding=-3:padcharacter=x}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>");

            Logger logger = LogManager.GetLogger("A");
            logger.Debug("a");
            AssertDebugLastMessage("debug", "axx");
            logger.Debug("a{0}", 1);
            AssertDebugLastMessage("debug", "a1x");
            logger.Debug("a{0}{1}", 1, "2");
            AssertDebugLastMessage("debug", "a12");
            logger.Debug(CultureInfo.InvariantCulture, "a{0}", new DateTime(2005, 1, 1));
            AssertDebugLastMessage("debug", "a01/01/2005 00:00:00");
        }

        [TestMethod]
        public void MessageFixedLengthLeftPaddingTest()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${message:padding=-3:padcharacter=x:fixedlength=true}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>");

            Logger logger = LogManager.GetLogger("A");
            logger.Debug("a");
            AssertDebugLastMessage("debug", "axx");
            logger.Debug("a{0}", 1);
            AssertDebugLastMessage("debug", "a1x");
            logger.Debug("a{0}{1}", 1, "2");
            AssertDebugLastMessage("debug", "a12");
            logger.Debug(CultureInfo.InvariantCulture, "a{0}", new DateTime(2005, 1, 1));
            AssertDebugLastMessage("debug", "a01");
        }
    }
}