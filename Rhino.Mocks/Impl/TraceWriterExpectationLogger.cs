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

using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks.Utilities;

namespace Rhino.Mocks.Impl
{
    /// <summary>
    /// Write rhino mocks log info to the trace
    /// </summary>
    public class TraceWriterExpectationLogger : IExpectationLogger
    {
        private readonly bool _logRecorded = true;
        private readonly bool _logReplayed = true;
        private readonly bool _logUnexpected = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="TraceWriterExpectationLogger"/> class.
		/// </summary>
        public TraceWriterExpectationLogger()
        {}

		/// <summary>
		/// Initializes a new instance of the <see cref="TraceWriterExpectationLogger"/> class.
		/// </summary>
		/// <param name="logRecorded">if set to <c>true</c> [log recorded].</param>
		/// <param name="logReplayed">if set to <c>true</c> [log replayed].</param>
		/// <param name="logUnexpected">if set to <c>true</c> [log unexpected].</param>
        public TraceWriterExpectationLogger(bool logRecorded, bool logReplayed, bool logUnexpected)
        {
            _logRecorded = logRecorded;
            _logReplayed = logReplayed;
            _logUnexpected = logUnexpected;
        }

        #region IExpectationLogger Members

		/// <summary>
		/// Logs the expectation as is was recorded
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <param name="expectation">The expectation.</param>
        public void LogRecordedExpectation(IInvocation invocation, IExpectation expectation)
        {
            if (_logRecorded)
            {
                string methodCall =
                    MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
                Trace.WriteLine(string.Format("Recorded expectation: {0}", methodCall));
            }
        }

		/// <summary>
		/// Logs the expectation as it was recorded
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <param name="expectation">The expectation.</param>
        public void LogReplayedExpectation(IInvocation invocation, IExpectation expectation)
        {
            if (_logReplayed)
            {
                string methodCall =
                    MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
                Trace.WriteLine(string.Format("Replayed expectation: {0}", methodCall));
            }
        }

		/// <summary>
		/// Logs the unexpected method call.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <param name="message">The message.</param>
        public void LogUnexpectedMethodCall(IInvocation invocation, string message)
        {
            if (_logUnexpected)
            {
                string methodCall =
                    MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
                Trace.WriteLine(string.Format("{1}: {0}", methodCall, message));
            }
        }

        #endregion
    }
}
