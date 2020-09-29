using System;
using System.Collections.Generic;
using System.Text;
using ADODB;
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_dyowee
	{
		[Test]
		public void MockingRecordSet()
		{
			MockRepository mr = new MockRepository();
			Recordset mock = mr.StrictMock<ADODB.Recordset>();
			Assert.NotNull(mock);
			Expect.Call(mock.ActiveConnection).Return("test");
			mr.ReplayAll();
			Assert.AreEqual("test", mock.ActiveConnection);
			mr.VerifyAll();
		}
	}
}
