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
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Exceptions;

	
	public class FieldProblem_Stefan
	{
		// This test fixture relates to ploblems when ignoring arguments on generic method calls when the type is a struct (aka value type).
		// With reference types - such as String - there is no problem.
		// It has nothing to do with ordering or not -> but if you do not use an ordered mock recorder, then the error msg is not helpful.


		[Test]
		public void ShouldIgnoreArgumentsOnGenericCallWhenTypeIsStruct()
		{
			// setup
			MockRepository mocks = new MockRepository();
			ISomeService m_SomeServiceMock = mocks.StrictMock<ISomeService>();
			SomeClient sut = new SomeClient(m_SomeServiceMock);

			using (mocks.Ordered())
			{
				Expect.Call(delegate
				{
					m_SomeServiceMock.DoSomething<string>(null, null);
				});
				LastCall.IgnoreArguments();

				Expect.Call(delegate
				{
					m_SomeServiceMock.DoSomething<DateTime>(null, default(DateTime));  // can't use null here, because it's a value type!
				});
				LastCall.IgnoreArguments();

			}
			mocks.ReplayAll();

			// test
			sut.DoSomething();

			// verification
			mocks.VerifyAll();

			// cleanup
			m_SomeServiceMock = null;
			sut = null;
		}

		[Test]
		public void UnexpectedCallToGenericMethod()
		{
			MockRepository mocks = new MockRepository();
			ISomeService m_SomeServiceMock = mocks.StrictMock<ISomeService>();
			m_SomeServiceMock.DoSomething<string>(null, "foo");
			mocks.ReplayAll();
            Assert.Throws<ExpectationViolationException> (
                () => m_SomeServiceMock.DoSomething<int> (null, 5),
                @"ISomeService.DoSomething<System.Int32>(null, 5); Expected #0, Actual #1.
ISomeService.DoSomething<System.String>(null, ""foo""); Expected #1, Actual #0.");
		}

		[Test]
		public void IgnoreArgumentsAfterDo()
		{
			MockRepository mocks = new MockRepository();
			IDemo demo = mocks.DynamicMock<IDemo>();
			bool didDo = false;
			demo.VoidNoArgs();
			LastCall
                .Do(SetToTrue(out didDo))
				.IgnoreArguments();

			mocks.ReplayAll();

			demo.VoidNoArgs();
			Assert.True(didDo, "Do has not been executed!");

			mocks.VerifyAll();
		}
		
		private delegate void PlaceHolder();

        private PlaceHolder SetToTrue(out bool didDo)
        {
			didDo = true;
            return delegate { };
        }
	}

	public interface ISomeService
	{
		void DoSomething<T>(string key, T someObj);
	}


	internal class SomeClient
	{
		private readonly ISomeService m_SomeSvc;

		public SomeClient(ISomeService someSvc)
		{
			m_SomeSvc = someSvc;
		}

		public void DoSomething()
		{
			m_SomeSvc.DoSomething<string>("string.test", "some string");

			m_SomeSvc.DoSomething<DateTime>("struct.test", DateTime.Now);

		}
	}
}