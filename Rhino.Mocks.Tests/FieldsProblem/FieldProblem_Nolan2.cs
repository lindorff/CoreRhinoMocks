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

namespace Rhino.Mocks.Tests.FieldsProblem.FieldProblem_Nolan2
{
    public interface IDemo
    {
        int SomeInt { get; set; }
        DateTime SomeDate { get; set; }
        DateTime? SomeNullableDate { get; set; }
        DateTime? SomeNulledDate { get; set; }
        object SomeObject { get; set; }

        bool TimeToGoHome();
    }

    
    public class When_mocking_properties_with_RhinoMocks_stub
    {
        protected IDemo _demo;
        protected DateTime _newDate = new DateTime(2008, 12, 2, 22, 17, 0);
        protected int _newInt = 7;

        protected DateTime? _newNullableDate = new DateTime(2008, 12, 2, 22,
                                                            17, 0);

        protected DateTime? _newNulledDate;
        protected Object _newObject = new object();

        protected void SetValuesOnMock()
        {
            _demo.SomeInt = _newInt;
            _demo.SomeDate = _newDate;
            _demo.SomeNullableDate = _newNullableDate;
            _demo.SomeNulledDate = _newNulledDate;
            _demo.SomeObject = _newObject;
        }

		public When_mocking_properties_with_RhinoMocks_stub()
        {
            _demo = MockRepository.GenerateStub<IDemo>();
            SetValuesOnMock();
        }

        [Test]
        public void Should_mock_value_int_property()
        {
            Assert.AreEqual(_newInt, _demo.SomeInt);
        }

        [Test]
        public void Should_mock_value_date_property()
        {
            Assert.AreEqual(_newDate, _demo.SomeDate);
        }

        [Test]
        public void Should_mock_nullable_value_date_property()
        {
            Assert.AreEqual(_newNullableDate, _demo.SomeNullableDate);
        }

        [Test]
        public void Should_mock_nulled_value_date_property()
        {
            Assert.AreEqual(_newNulledDate, _demo.SomeNulledDate);
        }

        [Test]
        public void Should_mock_reference_property()
        {
            Assert.AreEqual(_newObject, _demo.SomeObject);
        }
    }

    public class
        When_mocking_properties_with_RhinoMocks_stub_and_setting_expectations_afterward :
            When_mocking_properties_with_RhinoMocks_stub
    {
		public When_mocking_properties_with_RhinoMocks_stub_and_setting_expectations_afterward()
        {
            _demo = MockRepository.GenerateStub<IDemo>();
            SetValuesOnMock();

            _demo.Expect(d => d.TimeToGoHome())
                .Repeat.Any()
                .Return(false);
        }
    }

    
    public class
        When_mocking_properties_with_RhinoMocks_stub_and_setting_expectations_beforehand :
            When_mocking_properties_with_RhinoMocks_stub
    {
		public When_mocking_properties_with_RhinoMocks_stub_and_setting_expectations_beforehand()
        {
            _demo = MockRepository.GenerateStub<IDemo>();

            _demo.Expect(d => d.TimeToGoHome())
                .Repeat.Any()
                .Return(false);

            SetValuesOnMock();
        }
    }
	}