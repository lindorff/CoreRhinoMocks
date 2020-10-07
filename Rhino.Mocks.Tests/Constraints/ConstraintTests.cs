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
using System.Data;
using NUnit.Framework;
using Rhino.Mocks.Constraints;
using Rhino.Mocks.Exceptions;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Tests.Callbacks;
using Is = Rhino.Mocks.Constraints.Is;

namespace Rhino.Mocks.Tests.Constraints
{
	
	public class ConstraintTests
	{
		private IDemo demo;
		private MockRepository mocks;

		[SetUp]
		public void SetUp()
		{
			mocks = new MockRepository();
			demo = (IDemo) this.mocks.StrictMock(typeof (IDemo));
		}

        [Test]
		public void UsingPredicate()
		{
			demo.VoidStringArg(null);
			LastCall.Constraints(
				Is.Matching<string>(delegate(string s) 
				{
					return s.Length == 2;  
				}) 
				&&
				Is.Matching<string>(delegate(string s)
				{
					return s.EndsWith("b");	
				}));
			mocks.Replay(demo);
			
			demo.VoidStringArg("ab");

			mocks.VerifyAll();
		}

		[Test]
		public void UsingPredicateConstraintWhenTypesNotMatching()
		{
			demo.VoidStringArg(null);
			LastCall.Constraints(
				Is.Matching<DataSet>(delegate(DataSet s)
				{
					return false;
				}));
			mocks.Replay(demo);

            Assert.Throws<InvalidOperationException> (
                () => demo.VoidStringArg ("ab"),
                "Predicate accept System.Data.DataSet but parameter is System.String which is not compatible");
		}

        [Test]
        public void UsingPredicateConstraintWithSubtype()
        {
            demo.VoidStringArg(null);
            LastCall.Constraints(
                Is.Matching<object>(delegate(object o)
            {
                return o.Equals("ab");
            }));
            mocks.Replay(demo);

            demo.VoidStringArg("ab");

            mocks.VerifyAll();
        }

		[Test]
		public void UsingPredicateWhenExpectationViolated()
		{
			demo.VoidStringArg(null);
			LastCall.Constraints(
				Is.Matching<string>(JustPredicate)
				);
			mocks.Replay(demo);

            Assert.Throws<ExpectationViolationException> (
                () => demo.VoidStringArg ("cc"),
                "IDemo.VoidStringArg(\"cc\"); Expected #0, Actual #1.\r\nIDemo.VoidStringArg(Predicate (ConstraintTests.JustPredicate(obj);)); Expected #1, Actual #0.");
		}
		
		public bool JustPredicate(string s)
		{
			return false;
		}

        [Test]
        public void AndSeveralConstraings()
        {
            AbstractConstraint all = Is.NotEqual("bar") & Is.TypeOf(typeof(string)) & Is.NotNull();
            Assert.True(all.Eval("foo"));
            Assert.AreEqual("not equal to bar and type of {System.String} and not equal to null", all.Message);
        }

		 [Test]
        public void AndSeveralConstraings_WithGenerics()
        {
            AbstractConstraint all = Is.NotEqual("bar") && Is.TypeOf<string>() && Is.NotNull();
            Assert.True(all.Eval("foo"));
            Assert.AreEqual("not equal to bar and type of {System.String} and not equal to null", all.Message);
        }

		[Test]
		public void AndConstraints()
		{
			AbstractConstraint start = Text.StartsWith("Ayende"), end = Text.EndsWith("Rahien");
			AbstractConstraint combine = start & end;
			Assert.True(combine.Eval("Ayende Rahien"));
			Assert.AreEqual("starts with \"Ayende\" and ends with \"Rahien\"", combine.Message);
		}

		[Test]
		public void NotConstraint()
		{
			AbstractConstraint start = Text.StartsWith("Ayende");
			AbstractConstraint negate = !start;
			Assert.True(negate.Eval("Rahien"));
			Assert.AreEqual("not starts with \"Ayende\"", negate.Message);
		}

		[Test]
		public void OrConstraints()
		{
			AbstractConstraint start = Text.StartsWith("Ayende"), end = Text.EndsWith("Rahien");
			AbstractConstraint combine = start | end;
			Assert.True(combine.Eval("Ayende"));
			Assert.True(combine.Eval("Rahien"));
			Assert.AreEqual("starts with \"Ayende\" or ends with \"Rahien\"", combine.Message);
		}

		[Test]
		public void SettingConstraintOnAMock()
		{
			demo.VoidStringArg("Ayende");
			LastCall.On(demo).Constraints(Text.Contains("World"));
			mocks.Replay(demo);
			demo.VoidStringArg("Hello, World");
			mocks.Verify(demo);
		}

		[Test]
		public void ConstraintFailingThrows()
		{
			demo.VoidStringArg("Ayende");
			LastCall.On(demo).Constraints(Text.Contains("World"));
			mocks.Replay(demo);
            Assert.Throws<ExpectationViolationException> (
                () => demo.VoidStringArg ("Hello, world"),
                "IDemo.VoidStringArg(\"Hello, world\"); Expected #0, Actual #1.\r\nIDemo.VoidStringArg(contains \"World\"); Expected #1, Actual #0.");
		}

		[Test]
		public void ConstraintWithTooMuchForArguments()
		{
			demo.VoidStringArg("Ayende");
            Assert.Throws<InvalidOperationException> (
                () => LastCall.On (demo).Constraints (Text.Contains ("World"), Is.Equal ("Rahien")),
                "The number of constraints is not the same as the number of the method's parameters!");
		}

		[Test]
		public void ConstraintWithTooFewForArguments()
		{
			demo.VoidThreeArgs(1, "Ayende", 3.14f);
            Assert.Throws<InvalidOperationException> (
                () => LastCall.On (demo).Constraints (Text.Contains ("World"), Is.Equal ("Rahien")),
                "The number of constraints is not the same as the number of the method's parameters!");
		}

		[Test]
		public void ConstraintsThatWerentCallCauseVerifyFailure()
		{
			this.demo.VoidStringArg("Ayende");
			LastCall.On(this.demo).Constraints(Text.Contains("World"));
			this.mocks.Replay(this.demo);
            Assert.Throws<ExpectationViolationException> (
                () => this.mocks.Verify (this.demo),
                "IDemo.VoidStringArg(contains \"World\"); Expected #1, Actual #0.");
		}

		[Test]
		public void AddConstraintAndThenTryToIgnoreArgs()
		{
			this.demo.VoidStringArg("Ayende");
            Assert.Throws<InvalidOperationException> (
                () => LastCall.On (this.demo).Constraints (Text.Contains ("World")).Callback<string> ("".StartsWith),
                "This method has already been set to ConstraintsExpectation.");
		}

	}
}
