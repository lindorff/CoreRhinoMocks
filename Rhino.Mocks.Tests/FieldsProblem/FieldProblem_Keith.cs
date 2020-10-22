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

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using Interfaces;
	using NUnit.Framework;

	
	public class FieldProblem_Keith
	{
		public class Model
		{
			private string userName;
			public string UserName 
			{ 
				get { return userName; } 
				set { userName = value; } 
			}
		}

		public interface IView
		{
			Model Model { get; set; }
			event EventHandler ClickButton;
		}

		public class Controller
		{
			Model _model = null;

			public Controller(IView view)
			{
				// The controller owns the model, in this example its only
				// used by one view but in real live the reference to the
				// Model can be used by mutiple views. Given this I dont want
				// to send it in via the constructor.
				_model = new Model();
				view.Model = _model;

				view.ClickButton += new EventHandler(View_ClickButton);
			}

			void View_ClickButton(object sender, EventArgs e)
			{
				_model.UserName = "Keith here :)";
			}
		}

		[Test]
		public void Test_View_Events_WiredUp()
		{
			MockRepository mocks = new MockRepository();

			IView view = mocks.StrictMock<IView>();

			// expect that the model is set on the view
			// NOTE: if I move this Expect.Call above
			// the above Expect.Call, Rhino mocks blows up on with an
			// "This method has already been set to ArgsEqualExpectation."
			// not sure why. Its a side issue.
			Expect.Call(view.Model = Arg<Model>.Is.NotNull);

			// expect the event ClickButton to be wired up
			IEventRaiser clickButtonEvent =
					Expect.Call(delegate
					{
						view.ClickButton += null;
					}).IgnoreArguments().GetEventRaiser();

			// Q: How do i set an expectation that checks that the controller
			// correctly updates the model in the event handler.
			// i.e. above we know that the controller executes
			// _model.UserName = "Keith here :)"
			// but how can I verify it?
			// The following wont work, because Model is null:
			// Expect.Call(view.Model.UserName = Arg<String>.Is.Anything);

			mocks.ReplayAll();

			Controller controller = new Controller(view);
			clickButtonEvent.Raise(null, null);

			mocks.VerifyAll();
		}
	}
}