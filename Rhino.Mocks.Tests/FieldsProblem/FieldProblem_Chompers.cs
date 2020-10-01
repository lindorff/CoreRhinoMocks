using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_Chompers
	{
		[Test]
		public void MockingPropertyThatReturnsStream()
		{
			MockRepository mocks = new MockRepository();
			IBaseMessagePart messagePart = mocks.StrictMock<IBaseMessagePart>();
			MemoryStream stream = new MemoryStream();
			Expect.Call(messagePart.Data).Return(stream).Repeat.Any();
			mocks.ReplayAll();
			messagePart.Data.WriteByte(127);
			stream.Seek(0, SeekOrigin.Begin);
			Assert.AreEqual(127,  stream.ReadByte());
		}
	}

	public interface IBaseMessagePart
	{
		Stream Data { get; set; }
	}
}
