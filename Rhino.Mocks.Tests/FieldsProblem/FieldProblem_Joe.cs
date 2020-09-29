#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_Joe
	{
		[Test]
		public void MockingConcreteForm()
		{
			MockRepository mocks = new MockRepository();
			Form frm = mocks.PartialMock<Form>();
			Assert.NotNull(frm);
		}
	}
}

#endif
