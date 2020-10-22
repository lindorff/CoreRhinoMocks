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
    using Rhino.Mocks.Interfaces;

    /// <summary>
    /// Generates remoting proxies and provides utility functions
    /// </summary>
    internal class RemotingMockGenerator
    {
        ///<summary>
        /// Create the proxy using remoting
        ///</summary>
        public object CreateRemotingMock(Type type, IInterceptor interceptor, IMockedObject mockedObject)
        {
            if (type.IsInterface == false && !typeof(MarshalByRefObject).IsAssignableFrom(type))
            {
                throw new InvalidCastException(
                    String.Format("Cannot create remoting proxy. '{0}' is not derived from MarshalByRefObject", type.Name));
            }

            return new RemotingProxy(type, interceptor, mockedObject).GetTransparentProxy();
        }

        /// <summary>
        /// Check whether an object is a transparent proxy with a RemotingProxy behind it
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>true if the object is a transparent proxy with a RemotingProxy instance behind it, false otherwise</returns>
        /// <remarks>We use Equals() method to communicate with the real proxy behind the object.
        /// See IRemotingProxyOperation for more details</remarks>
        public static bool IsRemotingProxy(object obj)
        {
            if (obj == null) return false;
            RemotingProxyDetector detector = new RemotingProxyDetector();
            obj.Equals(detector);
            return detector.Detected;
        }

        /// <summary>
        /// Retrieve a mocked object from a transparent proxy
        /// </summary>
        /// <param name="proxy">Transparent proxy with a RemotingProxy instance behind it</param>
        /// <returns>Mocked object associated with the proxy</returns>
        /// <remarks>We use Equals() method to communicate with the real proxy behind the object.
        /// See IRemotingProxyOperation for more details</remarks>
        public static IMockedObject GetMockedObjectFromProxy(object proxy)
        {
            if (proxy == null) return null;
            RemotingProxyMockedObjectGetter getter = new RemotingProxyMockedObjectGetter();
            proxy.Equals(getter);
            return getter.MockedObject;
        }
    }
}

#endif
