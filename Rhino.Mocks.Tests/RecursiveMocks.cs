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
using System.Collections;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks.Impl;

namespace Rhino.Mocks.Tests
{
    
    public class RecursiveMocks
    {
        [Test]
        public void CanUseRecursiveMocks()
        {
            var session = MockRepository.GenerateMock<ISession>();
            session.Stub(x =>
                         x.CreateCriteria(typeof (Customer))
                             .List()
                ).Return(new[] {new Customer {Id = 1, Name = "ayende"}});

            Customer customer = session.CreateCriteria(typeof (Customer))
                .List()
                .Cast<Customer>()
                .First();

            Assert.AreEqual("ayende", customer.Name);
            Assert.AreEqual(1, customer.Id);
        }

        [Test]
        public void CanUseRecursiveMocksSimpler()
        {
            var mockService = MockRepository.GenerateMock<IMyService>();

            mockService.Expect(x => x.Identity.Name).Return("foo");

            Assert.AreEqual("foo", mockService.Identity.Name);
        }

		[Test, Ignore("Not supported right now as per Oren")]
        public void CanUseRecursiveMocksSimplerAlternateSyntax()
        {
            var mockService = MockRepository.GenerateMock<IMyService>();

            Expect.Call(mockService.Identity.Name).Return("foo");

            Assert.AreEqual("foo", mockService.Identity.Name);
        }

		[Test, Ignore("Not supported in replay mode")]
        public void WillGetSameInstanceOfRecursedMockForGenerateMockStatic()
        {
            var mock = MockRepository.GenerateMock<IMyService>();

            IIdentity i1 = mock.Identity;
            IIdentity i2 = mock.Identity;

            Assert.AreSame(i1, i2);
            Assert.NotNull(i1);
        }

		[Test, Ignore("Not supported in replay mode")]
        public void WillGetSameInstanceOfRecursedMockInReplayMode()
        {
            RhinoMocks.Logger = new TraceWriterExpectationLogger(true, true, true);

            MockRepository mocks = new MockRepository();
            var mock = mocks.DynamicMock<IMyService>();
            mocks.Replay(mock);

            IIdentity i1 = mock.Identity;
            IIdentity i2 = mock.Identity;

            Assert.AreSame(i1, i2);
            Assert.NotNull(i1);
        }

        [Test]
        public void WillGetSameInstanceOfRecursedMockWhenNotInReplayMode()
        {
            RhinoMocks.Logger = new TraceWriterExpectationLogger(true,true,true);

            var mock = new MockRepository().DynamicMock<IMyService>();

            IIdentity i1 = mock.Identity;
            IIdentity i2 = mock.Identity;

            Assert.AreSame(i1, i2);
            Assert.NotNull(i1);
        }

        public interface ISession
        {
            ICriteria CreateCriteria(Type type);
        }

        public interface ICriteria
        {
            IList List();
        }
        public class Customer
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }

        public interface IIdentity
        {
            string Name { get; set; }
        }

        public interface IMyService
        {
            IIdentity Identity { get; set; }
        }
    }
}
