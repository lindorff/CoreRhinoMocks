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
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_Chad
	{
		[Test]
		public void SetupResult_For_writeable_property_on_stub_should_be_ignored()
		{
			MockRepository mocks = new MockRepository();
			TestClass test = mocks.Stub<TestClass>();
			SetupResult.For(test.ReadOnly).Return("foo");

			const string expected =
				@"You are trying to set an expectation on a property that was defined to use PropertyBehavior.
Instead of writing code such as this: mockObject.Stub(x => x.SomeProperty).Return(42);
You can use the property directly to achieve the same result: mockObject.SomeProperty = 42;";

            Assert.Throws<InvalidOperationException> (() => SetupResult.For (test.ReadWrite).PropertyBehavior(), expected);
		}
		public class TestClass
		{
			public virtual string ReadOnly { get { return ""; } }
			public virtual string ReadWrite { get { return null; } set { } }
		}
	}
}