using System;
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    
    public class FieldProblem_ikriv42
    {
        public class D : MarshalByRefObject
        {
            public D(IConvertible dep1, IDisposable dep2, ICloneable dep3)
            {
            }
        }

       [Test]
       public void GenerateMock_Works_With_MarshalByRef()
       {
           MockRepository.GenerateMock<D>();
       }

        [Test]
        public void GenerateStub_Works_With_MarshalByRef()
        {
            MockRepository.GenerateStub<D>();
        }
    }
}