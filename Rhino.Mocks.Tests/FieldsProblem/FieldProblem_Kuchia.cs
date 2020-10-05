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
    
    public class FieldProblem_Kuchia : IDisposable
    {
        private MockRepository _mocks;
        private IProblem _problem;
        private IDaoFactory _daoFactory;
        private IBLFactory _blFactory;

        [Test]
        public void Method1_CallWithMocks_Returns10()
        {
            int result = Problem.Method1();
            Mocks.ReplayAll();
            Mocks.VerifyAll();
            Assert.AreEqual(10, result);

        }

        public MockRepository Mocks
        {
            get
            {
                _mocks = _mocks ?? new MockRepository();
                return _mocks;
            }
        }

        public IDaoFactory DaoFactoryMock
        {
            get
            {
                _daoFactory = _daoFactory ?? Mocks.StrictMock<IDaoFactory>();
                return _daoFactory;
            }
        }


        public IBLFactory BLFactoryMock
        {
            get
            {
                _blFactory = _blFactory ?? Mocks.StrictMock<IBLFactory>();
                return _blFactory;
            }
        }


        public IProblem Problem
        {
            get
            {
                _problem = _problem ?? new Problem(BLFactoryMock, DaoFactoryMock);
                return _problem;
            }

        }

        public void Dispose()
        {
            _problem = null;
            _blFactory = null;
            _daoFactory = null;
            _mocks = null;
        }
    }

    public interface IBLFactory
    {

    }

    public interface IDaoFactory
    {
    }

    public interface IProblem
    {
        int Method1();
    }

    public class Problem : BaseProblem, IProblem
    {
        public Problem(IBLFactory blFactory, IDaoFactory daoFactory)
            : base(blFactory, daoFactory)
        {

        }

        public int Method1()
        {
            return 10;
        }
    }

    public abstract class BaseProblem
    {
        private IBLFactory _blFactory;
        private IDaoFactory _daoFactory;

        public BaseProblem(IBLFactory blFactory, IDaoFactory daoFactory)
        {
            _blFactory = blFactory;
            _daoFactory = daoFactory;
        }
    }
}