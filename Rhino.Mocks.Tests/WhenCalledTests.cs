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


namespace Rhino.Mocks.Tests
{
	
	public class WhenCalledTests
	{
		[Test]
		public void Shortcut_to_arg_is_equal()
		{
			// minor hack to get this to work reliably, we reset the arg manager,
			// and restore on in the MockRepository ctor, so we do it this way
			new MockRepository();
			Assert.AreEqual(Arg.Is(1), Arg<int>.Is.Equal(1));
		}

		[Test]
		public void Can_use_when_called_to_exceute_code_when_exceptation_is_matched_without_stupid_delegate_sig_overhead()
		{
			var wasCalled = false;
			var stub = MockRepository.GenerateStub<IDemo>();
			stub.Stub(x => x.StringArgString(Arg.Is("")))
				.Return("blah")
				.WhenCalled(delegate { wasCalled = true; });
			Assert.AreEqual("blah", stub.StringArgString(""));
			Assert.True(wasCalled);
		}

		[Test]
		public void Can_modify_return_value()
		{
			var stub = MockRepository.GenerateStub<IDemo>();
			stub.Stub(x => x.StringArgString(Arg.Is("")))
				.Return("blah")
				.WhenCalled(invocation => invocation.ReturnValue = "arg");
			Assert.AreEqual("arg", stub.StringArgString(""));
		}

		[Test]
		public void Can_inspect_method_arguments()
		{
			var stub = MockRepository.GenerateStub<IDemo>();
			stub.Stub(x => x.StringArgString(null))
				.IgnoreArguments()
				.Return("blah")
				.WhenCalled(invocation => Assert.AreEqual("foo", invocation.Arguments[0]));
			Assert.AreEqual("blah", stub.StringArgString("foo"));
		}

	}
}
