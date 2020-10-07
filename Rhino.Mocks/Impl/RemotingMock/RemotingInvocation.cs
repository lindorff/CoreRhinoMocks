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

#if NETFRAMEWORK

using System;
using Castle.DynamicProxy;

namespace Rhino.Mocks.Impl.RemotingMock
{
    using System;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;

    /// <summary>
    /// Implementation of IInvocation based on remoting proxy
    /// </summary>
    /// <remarks>Some methods are marked NotSupported since they either don't make sense
    /// for remoting proxies, or they are never called by Rhino Mocks</remarks>
    internal class RemotingInvocation : IInvocation
    {
        private readonly IMethodCallMessage _message;
        private object _returnValue;
        private readonly RealProxy _realProxy;
		private object[] _args; 

        public RemotingInvocation(RealProxy realProxy, IMethodCallMessage message)
        {
            _message = message;
            _realProxy = realProxy;
			this._args = (object[])this._message.Properties["__Args"]; 
        }

        public object[] Arguments
        {
            get { return _args; }
        }

        public Type[] GenericArguments
        {
            get
            {
                MethodBase method = _message.MethodBase;
                if (!method.IsGenericMethod)
                {
                    return new Type[0];
                }

                return method.GetGenericArguments();
            }
        }

        public object GetArgumentValue(int index)
        {
            throw new NotSupportedException();
        }

        public MethodInfo GetConcreteMethod()
        {
            return (MethodInfo)_message.MethodBase;
        }

        public MethodInfo GetConcreteMethodInvocationTarget()
        {
            throw new NotSupportedException();
        }

        public object InvocationTarget
        {
            get { throw new NotSupportedException(); }
        }

        public MethodInfo Method
        {
            get { return GetConcreteMethod(); }
        }

        public MethodInfo MethodInvocationTarget
        {
            get { throw new NotSupportedException(); }
        }

        public void Proceed()
        {
            throw new InvalidOperationException("Proceed() is not applicable to remoting mocks.");
        }

        public IInvocationProceedInfo CaptureProceedInfo ()
        {
	        throw new NotSupportedException();
        }

        public object Proxy
        {
            get { return _realProxy.GetTransparentProxy(); }
        }

        public object ReturnValue
        {
            get { return _returnValue; }
            set { _returnValue = value; }
        }

        public void SetArgumentValue(int index, object value)
        {
            throw new NotSupportedException();
        }

        public Type TargetType
        {
            get { throw new NotSupportedException(); }
        }
    }
}

#endif
