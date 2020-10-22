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

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using System.Collections.Generic;
	using NUnit.Framework;
	using Rhino.Mocks.Constraints;

	
	public class FieldProblem_Lee
	{
		[Test]
		public void IgnoringArgumentsOnGenericMethod()
		{
			MockRepository mocks = new MockRepository();
			IHaveGenericMethod mock = mocks.StrictMock<IHaveGenericMethod>();
			
			mock.Foo(15);
			LastCall.IgnoreArguments().Return(true);

			mocks.ReplayAll();

			bool result = mock.Foo(16);
			Assert.True(result);
			mocks.VerifyAll();
		}


		[Test]
		public void WithGenericMethods()
		{
			MockRepository mocks = new MockRepository();
			IFunkyList<int> list = mocks.DynamicMock<IFunkyList<int>>();
			Assert.NotNull(list);
			List<Guid> results = new List<Guid>();
			Expect.Call(list.FunkItUp<Guid>(null, null))
				.IgnoreArguments()
				.Constraints(Mocks.Constraints.Is.Equal("1"), Mocks.Constraints.Is.Equal(2))
				.Return(results);
			mocks.ReplayAll();
			Assert.AreSame(results, list.FunkItUp<Guid>("1", 2));
		}
    }

	public interface IFunkyList<T> : IList<T>
	{
		ICollection<T2> FunkItUp<T2>(object arg1, object arg2);
	}

	public interface IHaveGenericMethod
	{
		bool Foo<T>(T obj);
	}
}