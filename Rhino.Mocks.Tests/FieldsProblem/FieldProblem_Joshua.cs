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
using Rhino.Mocks.Constraints;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    
    public class FieldProblem_Joshua
    {
        [Test]
        public void The_value_of_a_variable_used_as_an_out_parameter_should_not_be_used_as_a_constraint_on_an_expectation()
        {
            MockRepository mockRepository = new MockRepository();
            ServiceBeingCalled service = mockRepository.StrictMock<ServiceBeingCalled>();
            const int theNumberToReturnFromTheServiceOutParameter = 20;
            
            using(mockRepository.Record())
            {
                int uninitialized;
                
                // Uncommenting the following line will make the test pass, because the expectation constraints will match up with the actual call.
                // However, the value of an out parameter cannot be used within a method value before it is set within the method value,
                // so the value going in really is irrelevant, and should therefore be ignored when evaluating constraints.
                // Even ReSharper will tell you "Value assigned is not used in any execution path" for the following line.
                
                //uninitialized = 42;

                // I understand I can do an IgnoreArguments() or Contraints(Is.Equal("key"), Is.Anything()), but I think the framework should take care of that for me

                Expect.Call(service.PopulateOutParameter("key", out uninitialized)).Return(null).OutRef(theNumberToReturnFromTheServiceOutParameter);
            }
            ObjectBeingTested testObject = new ObjectBeingTested(service);
            int returnedValue = testObject.MethodUnderTest();
            Assert.AreEqual(theNumberToReturnFromTheServiceOutParameter, returnedValue);
        }

        public class ObjectBeingTested
        {
            private ServiceBeingCalled service;

            public ObjectBeingTested(ServiceBeingCalled service)
            {
                this.service = service;
            }

            public int MethodUnderTest()
            {
                const int A_NUMBER_THAT_SHOULD_BE_IGNORED = 42;
                int thisShouldGetPopulatedByTheService = A_NUMBER_THAT_SHOULD_BE_IGNORED;
                service.PopulateOutParameter("key", out thisShouldGetPopulatedByTheService);
                return thisShouldGetPopulatedByTheService;
            }
        }

        public interface ServiceBeingCalled
        {
            object PopulateOutParameter(string anInputParameter, out int theOutputParameter);
        }
    }
}
