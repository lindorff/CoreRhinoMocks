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
using System.Collections.Generic;
using System.Text;

namespace Rhino.Mocks.Impl.RemotingMock
{
    /// <summary>
    /// Operation on a remoting proxy
    /// </summary>
    /// <remarks>
    /// It is not possible to directly communicate to a real proxy via transparent proxy.
    /// Transparent proxy impersonates a user type and only methods of that user type are callable.
    /// The only methods that are guaranteed to exist on any transparent proxy are methods defined
    /// in Object: namely ToString(), GetHashCode(), and Equals()).
    /// 
    /// These three methods are the only way to tell the real proxy to do something.
    /// Equals() is the most suitable of all, since it accepts an arbitrary object parameter.
    /// The RemotingProxy code is built so that if it is compared to an IRemotingProxyOperation,
    /// transparentProxy.Equals(operation) will call operation.Process(realProxy).
    /// This way we can retrieve a real proxy from transparent proxy and perform
    /// arbitrary operation on it. 
    /// </remarks>
    internal interface IRemotingProxyOperation
    {
        void Process(RemotingProxy proxy);
    }
}

#endif
