namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using NUnit.Framework;

	
	public class FieldProblem_Henrik
	{
		[Test]
		public void Trying_to_mock_null_instance_should_fail_with_descriptive_error_message()
		{
            Assert.Throws<ArgumentNullException> (
                () => RhinoMocksExtensions.Expect<object> (null, x => x.ToString()),
                "You cannot mock a null instance\r\nParameter name: mock");
		}
	}
}
