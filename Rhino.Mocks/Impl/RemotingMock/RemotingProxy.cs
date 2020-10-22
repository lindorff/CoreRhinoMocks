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

using Castle.DynamicProxy;

namespace Rhino.Mocks.Impl.RemotingMock
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;
    using Rhino.Mocks.Interfaces;

    internal class RemotingProxy : RealProxy
    {
        private readonly IInterceptor _interceptor;
        private readonly IMockedObject _mockedObject;

        public RemotingProxy(Type type, IInterceptor interceptor, IMockedObject mockedObject)
            :
                base(type)
        {
            _interceptor = interceptor;
            _mockedObject = mockedObject;
        }

        public IMockedObject MockedObject
        {
            get { return _mockedObject; }
        }

		private static IMessage ReturnValue(object value, object[] outParams, IMethodCallMessage mcm)
		{
			return new ReturnMessage(value, outParams, outParams == null ? 0 : outParams.Length, mcm.LogicalCallContext, mcm);
		}

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage mcm = msg as IMethodCallMessage;
            if (mcm == null) return null;

            if (IsEqualsMethod(mcm))
            {
                return ReturnValue(HandleEquals(mcm), mcm);
            }

            if (IsGetHashCodeMethod(mcm))
            {
                return ReturnValue(GetHashCode(), mcm);
            }

            if (IsGetTypeMethod(mcm))
            {
                return ReturnValue(GetProxiedType(), mcm);
            }

            if (IsToStringMethod(mcm))
            {
                string retVal = String.Format("RemotingMock_{1}<{0}>", this.GetProxiedType().Name, this.GetHashCode());
                return ReturnValue(retVal, mcm);
            }

            RemotingInvocation invocation = new RemotingInvocation(this, mcm);
            _interceptor.Intercept(invocation);

			return ReturnValue(invocation.ReturnValue, invocation.Arguments, mcm);
        }

        private bool IsGetTypeMethod(IMethodCallMessage mcm)
        {
            if (mcm.MethodName != "GetType") return false;
            if (mcm.MethodBase.DeclaringType != typeof(object)) return false;
            Type[] args = mcm.MethodSignature as Type[];
            if (args == null) return false;
            return args.Length == 0;
        }

        private static bool IsEqualsMethod(IMethodMessage mcm)
        {
            if (mcm.MethodName != "Equals") return false;
            Type[] argTypes = mcm.MethodSignature as Type[];
            if (argTypes == null) return false;
            if (argTypes.Length == 1 && argTypes[0] == typeof(object)) return true;
            return false;
        }

        private static bool IsGetHashCodeMethod(IMethodMessage mcm)
        {
            if (mcm.MethodName != "GetHashCode") return false;
            Type[] argTypes = mcm.MethodSignature as Type[];
            if (argTypes == null) return false;
            return (argTypes.Length == 0);
        }

        private static bool IsToStringMethod(IMethodCallMessage mcm)
        {
            if (mcm.MethodName != "ToString") return false;
            Type[] args = mcm.MethodSignature as Type[];
            if (args == null) return false;
            return args.Length == 0;
        }


        private bool HandleEquals(IMethodMessage mcm)
        {
            object another = mcm.Args[0];
            if (another == null) return false;

            if (another is IRemotingProxyOperation)
            {
                ((IRemotingProxyOperation)another).Process(this);
                return false;
            }

            return ReferenceEquals(GetTransparentProxy(), another);
        }

        private static IMessage ReturnValue(object value, IMethodCallMessage mcm)
        {
            return new ReturnMessage(value, null, 0, mcm.LogicalCallContext, mcm);
        }
    }
}

#endif
