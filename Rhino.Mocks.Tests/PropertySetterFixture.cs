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
using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks.Tests
{
	
	public class PropertySetterFixture
	{
		[Test]
		public void Setter_Expectation_With_Custom_Ignore_Arguments()
		{
			MockRepository mocks = new MockRepository();

			IBar bar = mocks.StrictMock<IBar>();

			using(mocks.Record())
			{
				Expect.Call(bar.Foo).SetPropertyAndIgnoreArgument();
			}

			using(mocks.Playback())
			{
				bar.Foo = 2;
			}

			mocks.VerifyAll();
		}

		[Test]
		public void Setter_Expectation_Not_Fullfilled()
		{
			MockRepository mocks = new MockRepository();

			IBar bar = mocks.StrictMock<IBar>();

			using (mocks.Record())
			{
				Expect.Call(bar.Foo).SetPropertyAndIgnoreArgument();
			}

            Assert.Throws<ExpectationViolationException> (
                () =>
                {
                    using (mocks.Playback())
                    {
                    }
                },
                "IBar.set_Foo(any); Expected #1, Actual #0.");
		}

		[Test]
		public void Setter_Expectation_With_Correct_Argument()
		{
			MockRepository mocks = new MockRepository();

			IBar bar = mocks.StrictMock<IBar>();

			using (mocks.Record())
			{
				Expect.Call(bar.Foo).SetPropertyWithArgument(1);
			}

			using (mocks.Playback())
			{
				bar.Foo = 1;
			}

			mocks.VerifyAll();
		}

		[Test]
		public void Setter_Expectation_With_Wrong_Argument()
		{
			MockRepository mocks = new MockRepository();

			IBar bar = mocks.StrictMock<IBar>();

			using (mocks.Record())
			{
				Expect.Call(bar.Foo).SetPropertyWithArgument(1);
			}

			mocks.Playback();
            Assert.Throws<ExpectationViolationException> (() => { bar.Foo = 0; }, "IBar.set_Foo(0); Expected #0, Actual #1.\r\nIBar.set_Foo(1); Expected #1, Actual #0.");
		}
	}

	public interface IBar
	{
		int Foo { get; set; }
	}
}