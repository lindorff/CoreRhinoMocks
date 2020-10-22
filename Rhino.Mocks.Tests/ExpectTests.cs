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

namespace Rhino.Mocks.Tests
{
	using System;
	using NUnit.Framework;


	public class ExpectTests
	{
		private MockRepository mocks;
		private IDemo demo;

        [SetUp]
        public void SetUp()
		{
			mocks = new MockRepository();
			demo = mocks.StrictMock(typeof(IDemo)) as IDemo;
		}

        [TearDown]
        public void TearDown()
		{
			mocks.VerifyAll();
		}

		[Test]
		public void CanExpect()
		{
			Expect.On(demo).Call(demo.Prop).Return("Ayende");
			mocks.ReplayAll();
			Assert.AreEqual("Ayende", demo.Prop);
		}

		[Test]
		public void PassNonMock()
		{
			try
			{
                Assert.Throws<InvalidOperationException> (
                    () => Expect.On (new object()),
                    "The object 'System.Object' is not a mocked object.");
			}
			finally
			{
				mocks.ReplayAll(); //for the tear down
			}

		}

		[Test]
		public void CanUseAnonymousDelegatesToCallVoidMethods()
		{
			Expect.Call(delegate { demo.VoidNoArgs(); }).Throw(new ArgumentNullException());
			mocks.ReplayAll();
			Assert.Throws<ArgumentNullException>(demo.VoidNoArgs);
		}

		[Test]
		public void CanUseAnonymousDelegatesToCallVoidMethods_WithoutAnonymousDelegate()
		{
			Expect.Call(demo.VoidNoArgs).Throw(new ArgumentNullException());
			mocks.ReplayAll();
			Assert.Throws<ArgumentNullException>(demo.VoidNoArgs);
		}

		[Test]
		public void ExpectCallNormal()
		{
			Expect.Call(demo.Prop).Return("ayende");
			mocks.ReplayAll();
			Assert.AreEqual("ayende", demo.Prop);
		}

		[Test]
		public void ExpectWhenNoCallMade()
		{
            Assert.Throws<InvalidOperationException> (
                () => Expect.Call<object> (null),
                "The object is not a mock object that belong to this repository.");
			mocks.Replay(demo); //for the tear down
		}

		[Test]
		public void ExpectOnReplay()
		{
			Expect.Call(demo.Prop).Return("ayende");
			mocks.ReplayAll();
			Assert.AreEqual("ayende", demo.Prop);
            Assert.Throws<InvalidOperationException> (
                () => Expect.Call<object> (null),
                "Invalid call, the last call has been used or no call has been made (make sure that you are calling a virtual (C#) / Overridable (VB) method).");
		}
	}
}