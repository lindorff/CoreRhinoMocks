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
using Rhino.Mocks.Constraints;
using Rhino.Mocks.Interfaces;
using Is = Rhino.Mocks.Constraints.Is;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_leftend : IDisposable
	{
		private MockRepository mocks;
		private IAddAlbumPresenter viewMock;
		private IAlbum albumMock;
		private IEventRaiser saveRaiser;

		public FieldProblem_leftend()
		{
			mocks = new MockRepository();
			viewMock =
				(IAddAlbumPresenter) mocks.DynamicMock(typeof (IAddAlbumPresenter));
			albumMock = mocks.StrictMock<IAlbum>();

			viewMock.Save += null;
			LastCall.IgnoreArguments().Constraints(Is.NotNull());
			saveRaiser = LastCall.GetEventRaiser();
		}

		public void Dispose()
		{
			mocks.VerifyAll();
		}

		[Test]
		public void VerifyAttachesToViewEvents()
		{
			mocks.ReplayAll();
			new AddAlbumPresenter(viewMock);
		}

		[Test]
		public void SaveEventShouldSetViewPropertiesCorrectly()
		{
			Expect.Call(viewMock.AlbumToSave).Return(albumMock);
			albumMock.Save();//create expectation
			viewMock.ProcessSaveComplete();//create expectation
			mocks.ReplayAll();

			AddAlbumPresenter presenter = new
				AddAlbumPresenter(viewMock);
			saveRaiser.Raise(null, null);
		}

		public interface IAlbum
		{
			string Name { get; set; }
			void Save();
		}

		public class Album : IAlbum
		{
			private string mName;

			public string Name
			{
				get { return mName; }
				set { mName = value; }
			}

			public Album()
			{
			}

			public void Save()
			{
				//code to save to db
			}
		}

		public interface IAddAlbumPresenter
		{
			IAlbum AlbumToSave { get; }
			event EventHandler<EventArgs> Save;
			void ProcessSaveComplete();
		}

		public class AddAlbumPresenter
		{
			private IAddAlbumPresenter mView;

			public AddAlbumPresenter(IAddAlbumPresenter view)
			{
				mView = view;
				Initialize();
			}

			private void Initialize()
			{
				mView.Save += new
					EventHandler<EventArgs>(mView_Save);
			}

			private void mView_Save(object sender, EventArgs e)
			{
				IAlbum newAlbum = mView.AlbumToSave;
				try
				{
					newAlbum.Save();
					mView.ProcessSaveComplete();
				}
				catch
				{
					//handle exception
				}
			}
		}
	}
}