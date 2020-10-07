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
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    
    public class FieldProblem_LAFAY
    {
        private IDemo demo;
        private MockRepository mocks;

		public FieldProblem_LAFAY()
        {
            mocks = new MockRepository();
            demo = mocks.StrictMock(typeof (IDemo)) as IDemo;
        }

        [Test]
        public void ExpectTwoCallsReturningMarshalByRef()
        {
            MarshalByRefToReturn res1 = new MarshalByRefToReturn();
            MarshalByRefToReturn res2 = new MarshalByRefToReturn();
            Expect.Call(demo.ReturnMarshalByRefNoArgs()).Return(res1);
            Expect.Call(demo.ReturnMarshalByRefNoArgs()).Return(res2);
            mocks.ReplayAll();
            demo.ReturnMarshalByRefNoArgs();
            demo.ReturnMarshalByRefNoArgs();
        }

        #region Nested type: IDemo

        public interface IDemo
        {
            MarshalByRefToReturn ReturnMarshalByRefNoArgs();
        }

        #endregion

        #region Nested type: MarshalByRefToReturn

        public class MarshalByRefToReturn : MarshalByRefObject
        {
            public override string ToString()
            {
                return "test";
            }
        }

        #endregion
    }
}