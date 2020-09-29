namespace Rhino.Mocks.Tests
{
	using System.IO;
	using NUnit.Framework;
	using Rhino.Mocks.Impl;

	
	public class TraceWriterWithStackTraceExpectationWriterFixture
	{
		[Test]
		public void WillPrintLogInfoWithStackTrace()
		{
			TraceWriterWithStackTraceExpectationWriter expectationWriter = new TraceWriterWithStackTraceExpectationWriter();
			StringWriter writer = new StringWriter();
			expectationWriter.AlternativeWriter = writer;

			RhinoMocks.Logger = expectationWriter;

			MockRepository mocks = new MockRepository();
			IDemo mock = mocks.StrictMock<IDemo>();
			mock.VoidNoArgs();
			mocks.ReplayAll();
			mock.VoidNoArgs();
			mocks.VerifyAll();

			StringAssert.Contains("WillPrintLogInfoWithStackTrace",
				writer.GetStringBuilder().ToString());
		}
	}
}