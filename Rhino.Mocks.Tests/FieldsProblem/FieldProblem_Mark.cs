#region license
// Copyright (c) 2020 rubicon IT GmbH, www.rubicon.eu
// Copyright (c) 2005 - 2009 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_Mark
	{
		[Test]
		public void GoodExplanationForUsingRepeatNeverAndReturn()
		{
			MockRepository mocks = new MockRepository();
			ILogWriter eventLogMock = (ILogWriter)mocks.DynamicMock(typeof(ILogWriter));
			Log log = new Log(null, eventLogMock, "MOCK", true, false);

			Expect.Call(eventLogMock.WriteLog(EventLogEntryType.SuccessAudit, "MOCK", null, null, 0)).Return(true);

            Assert.Throws<InvalidOperationException> (
                () => Expect.Call (eventLogMock.WriteLog (EventLogEntryType.FailureAudit, "MOCK", null, null, 0))
                    .Repeat.Never()
                    .Return (true),
                "After specifying Repeat.Never(), you cannot specify a return value, exception to throw or an action to execute");
		}

		//This is exactly like the one above, but the calls to repeat and return are reverse
		[Test]
		public void GoodExplanationForUsingReturnAndRepeatNever()
		{
			MockRepository mocks = new MockRepository();
			ILogWriter eventLogMock = (ILogWriter)mocks.DynamicMock(typeof(ILogWriter));
			Log log = new Log(null, eventLogMock, "MOCK", true, false);

			Expect.Call(eventLogMock.WriteLog(EventLogEntryType.SuccessAudit, "MOCK", null, null, 0)).Return(true);
            Assert.Throws<InvalidOperationException> (
                () => Expect.Call (eventLogMock.WriteLog (EventLogEntryType.FailureAudit, "MOCK", null, null, 0))
                    .Return (true)
                    .Repeat.Never(),
                "After specifying Repeat.Never(), you cannot specify a return value, exception to throw or an action to execute");
		}
	}

	public class Log
	{
		private ILogWriter traceWriter;
		private ILogWriter eventLogWriter;
		private string systemLogging;
		private bool logSuccesAudit;
		private bool logFailureAudit;

		public Log(ILogWriter traceWriter, ILogWriter eventLogWriter, string system, bool logSuccesAudit, bool logFailureAudit)
		{
			this.traceWriter = traceWriter;
			this.eventLogWriter = eventLogWriter;
			systemLogging = system;
			this.logSuccesAudit = logSuccesAudit;
			this.logFailureAudit = logFailureAudit;
		}

		public void Audit(AuditOptions audit, string system, string component, string text, int eventId)
		{
			EventLogEntryType translatedEntryType;
			if (audit == AuditOptions.Succes)
				translatedEntryType = EventLogEntryType.SuccessAudit;
			else
				translatedEntryType = EventLogEntryType.FailureAudit;
			eventLogWriter.WriteLog(translatedEntryType, system, component, text, eventId);
		}
	}

	public enum AuditOptions
	{
		Succes,
		Failure
	}

	public interface ILogWriter
	{
		bool WriteLog(EventLogEntryType entryType, string system, string component, string text, int eventId);
	}
}

#endif
