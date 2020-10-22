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
	using System.Data;
	using Exceptions;
	using NUnit.Framework;
	using Rhino.Mocks;

	
	public class FieldProblem_Andrew
	{
		[Test]
		public void Will_get_unexpect_error()
		{
			var stubConnection = MockRepository.GenerateStub<IDbConnection>();
			var mockCommand = MockRepository.GenerateMock<IDbCommand>();
			mockCommand.Expect(c => c.Connection = stubConnection);
			mockCommand.Expect(c => c.Connection = null);
			mockCommand.Stub(c => c.ExecuteNonQuery()).Throw(new TestException());

			var executor = new Executor(stubConnection);
			try
			{
				executor.ExecuteNonQuery(mockCommand);
				Assert.False(true, "exception was expected");
			}
			catch (TestException)
			{
			}

            Assert.Throws<ExpectationViolationException> (() => mockCommand.VerifyAllExpectations(), "IDbCommand.set_Connection(null); Expected #1, Actual #0.");
		}
	}

	public class TestException : Exception
	{
	}


	public class Executor
	{
		private IDbConnection _connection;

		public Executor(IDbConnection connection)
		{
			this._connection = connection;
		}

		public int ExecuteNonQuery(IDbCommand command)
		{
			try
			{
				command.Connection = this._connection;
				return command.ExecuteNonQuery();
			}
			finally
			{
				//command.Connection = null;
				if (this._connection.State != ConnectionState.Closed) this._connection.Close();
			}
		}
	}
}