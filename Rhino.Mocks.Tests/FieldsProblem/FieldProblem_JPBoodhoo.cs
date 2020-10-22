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
    public class FieldProblem_JPBoodhoo
    {
        public class VirtualClass
        {
            public virtual DateTime virtual_property_public_read_private_write { get; private set; }
            public virtual string run_sheet_name { get; set; }
        }

        
        public class when_setting_up_a_return_value_for_a_virtual_property_on_a_class_with_a_public_getter_and_private_setter
        {
            VirtualClass target;

			public  when_setting_up_a_return_value_for_a_virtual_property_on_a_class_with_a_public_getter_and_private_setter()
            {
                target = MockRepository.GenerateStub<VirtualClass>();
                target.Stub(entry_model => entry_model.virtual_property_public_read_private_write).Return(DateTime.Now);
            }

            [Test]
            public void should_not_throw_the_exception_suggesting_to_assign_the_property_value_directly()
            {
                target.virtual_property_public_read_private_write.Equals(DateTime.Now);
            }
        }
    }
}