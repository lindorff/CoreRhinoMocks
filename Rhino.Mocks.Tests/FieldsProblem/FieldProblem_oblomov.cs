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
using System.Text;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using NUnit.Framework;

	public interface IMyService
	{
		void Func1();
		void Func2();
		void Func3();
	}

	
	public class FieldProblem_oblomov
	{
		MockRepository mocks;
		IMyService service;

		[SetUp]
		public void SetUp()
		{
			mocks = new MockRepository();
			service = mocks.StrictMock<IMyService>();
		}

		[TearDown]
		public void TearDown()
		{
			mocks.VerifyAll();
		}

		[Test]
		public void TestWorks()
		{
			using (mocks.Ordered())
			{
				using (mocks.Unordered())
				{
					service.Func1();
					service.Func2();
				}
				service.Func3();
			}
			mocks.ReplayAll();

			service.Func2();
			service.Func1();
			service.Func3();
		}

		[Test]
		public void TestDoesnotWork()
		{
			using (mocks.Ordered())
			{
				using (mocks.Unordered())
				{
					//service.Func1();
					//service.Func2();
				}
				service.Func3();
			}
			mocks.ReplayAll();

			//service.Func2();
			//service.Func1();
			service.Func3();
		}

		[Test]
		public void TestDoesnotWork2()
		{
			using (mocks.Ordered())
			{
				using (mocks.Ordered())
				{
					//service.Func1();
					//service.Func2();
				}
				service.Func3();
			}
			mocks.ReplayAll();

			//service.Func2();
			//service.Func1();
			service.Func3();
		}
	}
}
