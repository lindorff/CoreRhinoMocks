using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_Roger
	{
		[Test]
		public void VerifyMockCanBeSetupWhenExternalInterfaceUsingInnerClassWithInternalScope()
		{
			MockRepository mocks = new MockRepository();
			ISomeInterface<InnerClass> target = mocks.StrictMock<ISomeInterface<InnerClass>>();
		}
	}

	internal class InnerClass
	{
	}

	public interface ISomeInterface<T> { }

}
