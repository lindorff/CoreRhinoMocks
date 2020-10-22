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

using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    
    public class FieldProblem_Greg
    {
		private MockRepository _mockRepository = new MockRepository();

        [Test]
        public void IgnoreArguments()
        {
            IFoo myFoo = _mockRepository.StrictMock<IFoo>();
            IBar<int> myBar = _mockRepository.StrictMock<IBar<int>>();

            using(_mockRepository.Record())
            using (_mockRepository.Ordered())
            {
                Expect.Call(myFoo.RunBar(myBar)).IgnoreArguments().Return(true);
            }

            using (_mockRepository.Playback())
            {
                Example<int> myExample = new Example<int>(myFoo, myBar);
                bool success = myExample.ExampleMethod();
                Assert.True(success);
            }
        }
    }

    public class Example<T>
    {
        private readonly IBar<T> _bar;
        private readonly IFoo _foo;

        public Example(IFoo foo, IBar<T> bar)
        {
            _foo = foo;
            _bar = bar;
        }

        public bool ExampleMethod()
        {
            bool success = _foo.RunBar(_bar);
            return success;
        }
    }

    public interface IFoo
    {
        bool RunBar<T>(IBar<T> barObject);
    }

    public interface IBar<T>
    {
        void BarMethod(T paramToBarMethod);
    }

    public class Foo : IFoo
    {
        //When Foo is mocked, this method returns FALSE!!!

        #region IFoo Members

        public bool RunBar<T>(IBar<T> barObject)
        {
            return true;
        }

        #endregion
    }

    public class Bar<T> : IBar<T>
    {
        #region IBar<T> Members

        public void BarMethod(T paramToBarMethod)
        {
            //nothing important
        }

        #endregion
    }
}