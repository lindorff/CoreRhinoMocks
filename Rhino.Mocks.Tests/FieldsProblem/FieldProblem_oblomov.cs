using System;
using System.Collections.Generic;
using System.Text;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using NUnit.Framework;

	public interface IMyService
	{
		void Func1();
		void Func2();
		void Func3();
	}

	
	public class FieldProblem_oblomov
	{
		MockRepository mocks;
		IMyService service;

		[SetUp]
		public void SetUp()
		{
			mocks = new MockRepository();
			service = mocks.StrictMock<IMyService>();
		}

		[TearDown]
		public void TearDown()
		{
			mocks.VerifyAll();
		}

		[Test]
		public void TestWorks()
		{
			using (mocks.Ordered())
			{
				using (mocks.Unordered())
				{
					service.Func1();
					service.Func2();
				}
				service.Func3();
			}
			mocks.ReplayAll();

			service.Func2();
			service.Func1();
			service.Func3();
		}

		[Test]
		public void TestDoesnotWork()
		{
			using (mocks.Ordered())
			{
				using (mocks.Unordered())
				{
					//service.Func1();
					//service.Func2();
				}
				service.Func3();
			}
			mocks.ReplayAll();

			//service.Func2();
			//service.Func1();
			service.Func3();
		}

		[Test]
		public void TestDoesnotWork2()
		{
			using (mocks.Ordered())
			{
				using (mocks.Ordered())
				{
					//service.Func1();
					//service.Func2();
				}
				service.Func3();
			}
			mocks.ReplayAll();

			//service.Func2();
			//service.Func1();
			service.Func3();
		}
	}
}
