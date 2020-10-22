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

using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Exceptions;

	
	public class FieldProblem_Sam
	{
		[Test]
		public void Test()
		{
			MockRepository mocks = new MockRepository();
			SimpleOperations myMock = mocks.StrictMock<SimpleOperations>();
			Expect.Call(myMock.AddTwoValues(1, 2)).Return(3);
			mocks.ReplayAll();
			Assert.AreEqual(3, myMock.AddTwoValues(1, 2));
			mocks.VerifyAll();
		}

		[Test]
		public void WillRememberExceptionInsideOrderRecorderEvenIfInsideCatchBlock()
		{
			MockRepository mockRepository = new MockRepository();
			IInterfaceWithThreeMethods interfaceWithThreeMethods = mockRepository.StrictMock<IInterfaceWithThreeMethods>();

			using (mockRepository.Ordered())
			{
				interfaceWithThreeMethods.A();
				interfaceWithThreeMethods.C();
			}

			mockRepository.ReplayAll();

			interfaceWithThreeMethods.A();
			try
			{
				interfaceWithThreeMethods.B();
			}
			catch { /* valid for code under test to catch all */ }
			interfaceWithThreeMethods.C();

            Assert.Throws<ExpectationViolationException> (
                () => mockRepository.VerifyAll(),
                "Unordered method call! The expected call is: 'Ordered: { IInterfaceWithThreeMethods.C(); }' but was: 'IInterfaceWithThreeMethods.B();'");
		}
	}

	public interface IInterfaceWithThreeMethods
	{
		void A();
		void B();
		void C();
	}

	public class SimpleOperations
	{
		public virtual int AddTwoValues(int x, int y)
		{
			return x + y;
		}
	}
}