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

using Castle.DynamicProxy;

namespace Rhino.Mocks.Impl
{
	using System.Diagnostics;
	using System.IO;
	using Interfaces;
	using Utilities;

	/// <summary>
	/// Writes log information as stack traces about rhino mocks activity
	/// </summary>
	public class TraceWriterWithStackTraceExpectationWriter : IExpectationLogger
	{
		/// <summary>
		/// Allows to redirect output to a different location.
		/// </summary>
		public TextWriter AlternativeWriter;

		/// <summary>
		/// Logs the expectation as is was recorded
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <param name="expectation">The expectation.</param>
		public void LogRecordedExpectation(IInvocation invocation, IExpectation expectation)
		{
			string methodCall = MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
			WriteLine("Recorded expectation: {0}", methodCall);
			WriteCurrentMethod();
		}

		private void WriteLine(string msg, params object[] args)
		{
			string result = string.Format(msg, args);
			if (AlternativeWriter != null)
			{
				AlternativeWriter.WriteLine(result);
				return;
			}
			Debug.WriteLine(result);
		}

		private void WriteCurrentMethod()
		{
			WriteLine(new StackTrace(true).ToString());
		}

		/// <summary>
		/// Logs the expectation as it was recorded
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <param name="expectation">The expectation.</param>
		public void LogReplayedExpectation(IInvocation invocation, IExpectation expectation)
		{
			string methodCall = MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
			WriteLine("Replayed expectation: {0}", methodCall);
			WriteCurrentMethod();
		}

		/// <summary>
		/// Logs the unexpected method call.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <param name="message">The message.</param>
		public void LogUnexpectedMethodCall(IInvocation invocation, string message)
		{
			string methodCall = MethodCallUtil.StringPresentation(invocation, invocation.Method, invocation.Arguments);
			WriteLine("{1}: {0}", methodCall, message);
			WriteCurrentMethod();
		}
	}
}