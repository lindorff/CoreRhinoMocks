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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public interface IBase
    {
        String BaseString { get; set; }
    }

    public interface IChild : IBase
    {
        String ChildString { get; set; }
    }

    
    public class StubDemoTestFixture : IDisposable
    {

        #region Variables

        private MockRepository _mocks;
        private IBase _mockBase;
        private IChild _mockChild;

        #endregion

        #region Setup and Teardown

		public StubDemoTestFixture()
        {
            _mocks = new MockRepository();

            _mockBase = _mocks.Stub<IBase>();
            _mockChild = _mocks.Stub<IChild>();

            _mocks.ReplayAll();
        }

        public void Dispose()
        {
            _mocks.VerifyAll();
        }

        #endregion

        #region Tests

        [Test]
        public void BaseStubSetsBasePropertiesCorrectly()
        {
            String str = "Base stub";

            _mockBase.BaseString = str;

            Assert.AreEqual(str, _mockBase.BaseString);
        }

        [Test]
        public void ChildStubSetsChildPropertiesCorrectly()
        {
            String str = "Child stub";

            _mockChild.ChildString = str;

            Assert.AreEqual(str, _mockChild.ChildString);
        }

        [Test]
        public void ChildStubSetsBasePropertiesCorrectly()
        {
            String str = "Child's base stub";

            _mockChild.BaseString = str;

            Assert.AreEqual(str, _mockChild.BaseString);
        }

        #endregion

    }
}
