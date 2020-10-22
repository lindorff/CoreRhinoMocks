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

using System.Collections.Generic;
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_Mads
	{
		[Test, Ignore(@"YUCK! Unsolvable framework bug.
See here for the details:
http://groups.google.co.il/group/castle-project-devel/browse_thread/thread/1697e02d96c9c2df/d4e3e24f444ac712?lnk=st&q=Generic+interface+with+generic+method+with+constrained+on+generic+method+param+&rnum=1#d4e3e24f444ac712")]
		public void Unresolable_Framework_Bug_With_Generic_Method_On_Generic_Interface_With_Conditions_On_Both_Generics()
		{
			MockRepository mocks = new MockRepository();

			TestInterface<List<string>> mockedInterface =
				mocks.StrictMock<TestInterface<List<string>>>();
		}
	}

	public interface TestInterface<T> where T : IEnumerable<string>
	{
		string TestMethod<T2>(T2 obj) where T2 : T,
		                              	ICollection<string>;
	}
}