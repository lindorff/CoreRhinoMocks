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

namespace Rhino.Mocks.Tests.MethodProblems
{
    public class MethodProblem_JPBoodhoo
    {
        public interface InterfaceWithAMethodThatHasANameThatShouldNotBeRecognizedAsAnEvent
        {
            void add_MethodThatShouldNotBeSeenAsAnEvent(object item);
        }

        public class GenericClass
        {
            InterfaceWithAMethodThatHasANameThatShouldNotBeRecognizedAsAnEvent dependency;

            public GenericClass(InterfaceWithAMethodThatHasANameThatShouldNotBeRecognizedAsAnEvent dependency)
            {
                this.dependency = dependency;
            }

            public void do_something(object item)
            {
                dependency.add_MethodThatShouldNotBeSeenAsAnEvent(item);
            }
        }

        
        public class when_stubbing_a_call_to_a_method_that_matches_the_naming_prefix_for_an_event_but_is_not_an_event
        {
            InterfaceWithAMethodThatHasANameThatShouldNotBeRecognizedAsAnEvent dependency;
            GenericClass system_under_test;
            object item;


			public when_stubbing_a_call_to_a_method_that_matches_the_naming_prefix_for_an_event_but_is_not_an_event()
            {
                item = new object();
                dependency = MockRepository.GenerateStub<InterfaceWithAMethodThatHasANameThatShouldNotBeRecognizedAsAnEvent>();
                system_under_test = new GenericClass(dependency);
                system_under_test.do_something(item);
            }

            [Test]
            public void should_not_try_to_treat_it_as_an_event()
            {
                dependency.AssertWasCalled(generic_parameter => generic_parameter.add_MethodThatShouldNotBeSeenAsAnEvent(item));
            }
        }
    }
}