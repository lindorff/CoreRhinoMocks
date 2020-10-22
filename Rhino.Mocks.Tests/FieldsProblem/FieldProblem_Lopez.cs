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
	using NUnit.Framework;

	
	public class FieldProblem_Lopez
	{
		public interface GenericContainer<T>
		{
			T Item { get; set; }
		}


		[Test]
		public void PropertyBehaviorForSinglePropertyTypeOfString()
		{
			MockRepository mocks = new MockRepository();

			GenericContainer<string> stringContainer = mocks.StrictMock<GenericContainer<string>>();

			Expect.Call(stringContainer.Item).PropertyBehavior();

			mocks.Replay(stringContainer);

			for (int i = 1; i < 49; ++i)
			{
				string newItem = i.ToString();

				stringContainer.Item = newItem;

				Assert.AreEqual(newItem, stringContainer.Item);
			}

			mocks.Verify(stringContainer);
		}


		[Test]
		public void PropertyBehaviourForSinglePropertyTypeOfDateTime()
		{
			MockRepository mocks = new MockRepository();

			GenericContainer<DateTime> dateTimeContainer = mocks.StrictMock<GenericContainer<DateTime>>();

			Expect.Call(dateTimeContainer.Item).PropertyBehavior();

			mocks.Replay(dateTimeContainer);

			for (int i = 1; i < 12; i++)
			{
				DateTime date = new DateTime(2007, i, i);

				dateTimeContainer.Item = date;

				Assert.AreEqual(date, dateTimeContainer.Item);
			}

			mocks.Verify(dateTimeContainer);
		}


		[Test]
		public void PropertyBehaviourForSinglePropertyTypeOfInteger()
		{
			MockRepository mocks = new MockRepository();

			GenericContainer<int> dateTimeContainer = mocks.StrictMock<GenericContainer<int>>();

			Expect.Call(dateTimeContainer.Item).PropertyBehavior();

			mocks.Replay(dateTimeContainer);

			for (int i = 1; i < 49; i++)
			{
				dateTimeContainer.Item = i;

				Assert.AreEqual(i, dateTimeContainer.Item);
			}

			mocks.Verify(dateTimeContainer);
		}
	}
}