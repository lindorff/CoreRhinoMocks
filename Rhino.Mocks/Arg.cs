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
using System.Linq.Expressions;
using Rhino.Mocks.Constraints;

namespace Rhino.Mocks
{
	/// <summary>
	/// Defines constraints and return values for arguments of a mock.
	/// Only use Arg inside a method call on a mock that is recording.
	/// Example: 
	///   ExpectCall( 
	///     mock.foo(
	///       Arg&lt;int&gt;.Is.GreaterThan(2),
	///       Arg&lt;string&gt;.Is.Anything
	///     ));
	/// Use Arg.Text for string specific constraints
	/// Use Arg&lt;ListClass&gt;.List for list specific constraints
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class Arg<T>
	{
		/// <summary>
		/// Register the predicate as a constraint for the current call.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns>default(T)</returns>
		/// <example>
		/// Allow you to use code to create constraints
		/// <code>
		/// demo.AssertWasCalled(x => x.Bar(Arg{string}.Matches(a => a.StartsWith("b") &amp;&amp; a.Contains("ba"))));
		/// </code>
		/// </example>
		public static T Matches(Expression<Predicate<T>> predicate)
		{
			ArgManager.AddInArgument(new LambdaConstraint(predicate));
			return default(T);
		}

		/// <summary>
		/// Define a simple constraint for this argument. (Use Matches in simple cases.)
		/// Example: 
		///   Arg&lt;int&gt;.Is.Anthing
		///   Arg&lt;string&gt;.Is.Equal("hello")
		/// </summary>
		public static IsArg<T> Is { get { return new IsArg<T>(); } }

		/// <summary>
		/// Define a complex constraint for this argument by passing several constraints
		/// combined with operators. (Use Is in simple cases.)
		/// Example: Arg&lt;string&gt;.Matches(Is.Equal("Hello") || Text.EndsWith("u"));
		/// </summary>
		/// <param name="constraint">Constraints using Is, Text and List</param>
		/// <returns>Dummy to satisfy the compiler</returns>
		public static T Matches(AbstractConstraint constraint)
		{
			ArgManager.AddInArgument(constraint);
			return default(T);
		}

		/// <summary>
		/// Define a Ref argument.
		/// </summary>
		/// <param name="constraint">Constraints for this argument</param>
		/// <param name="returnValue">value returned by the mock</param>
		/// <returns></returns>
		public static OutRefArgDummy<T> Ref(AbstractConstraint constraint, T returnValue)
		{
			ArgManager.AddRefArgument(constraint, returnValue);
			return new OutRefArgDummy<T>();
		}

		/// <summary>
		/// Define a out parameter. Use it together with the keyword out and use the
		/// Dummy field available by the return value.
		/// Example:  mock.foo( out Arg&lt;string&gt;.Out("hello").Dummy );
		/// </summary>
		/// <param name="returnValue"></param>
		/// <returns></returns>
		public static OutRefArgDummy<T> Out(T returnValue)
		{
			ArgManager.AddOutArgument(returnValue);
			return new OutRefArgDummy<T>();
		}

		/// <summary>
		/// Define Constraints on list arguments.
		/// </summary>
		public static ListArg<T> List
		{
			get
			{
				return new ListArg<T>();
			}
		}

	}

	/// <summary>
	/// Use the Arg class (without generic) to define Text constraints
	/// </summary>
	public static class Arg
	{
		/// <summary>
		/// Define constraints on text arguments.
		/// </summary>
		public static TextArg Text { get { return new TextArg(); } }

		/// <summary>
		/// Evaluate an equal constraint for <see cref="IComparable"/>.
		/// </summary>
		/// <param name="arg">The object the parameter should equal to</param>
		public static T Is<T>(T arg)
		{
			return Arg<T>.Is.Equal(arg);
		}
	}
}

