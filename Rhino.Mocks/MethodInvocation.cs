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
using System.Reflection;
using Castle.DynamicProxy;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks
{
	/// <summary>
	/// This is a data structure that is used by 
	/// <seealso cref="IMethodOptions{T}.WhenCalled"/> to pass
	/// the current method to the relevant delegate
	/// </summary>
	public class MethodInvocation
	{
		private readonly IInvocation invocation;

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodInvocation"/> class.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		internal MethodInvocation(IInvocation invocation)
		{
			this.invocation = invocation;
		}

		/// <summary>
		/// Gets the args for this method invocation
		/// </summary>
		public object[] Arguments
		{
			get { return invocation.Arguments; }
		}

        /// <summary>
        /// Get the method that was caused this invocation
        /// </summary>
	    public MethodInfo Method
	    {
	        get
	        {
	            return invocation.Method;
	        }
	    }

		/// <summary>
		/// Gets or sets the return value for this method invocation
		/// </summary>
		/// <value>The return value.</value>
		public object ReturnValue
		{
			get { return invocation.ReturnValue; }
			set { invocation.ReturnValue = value; }
		}


	}
}