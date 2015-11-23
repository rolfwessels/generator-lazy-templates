using System;
using FluentAssertions;
using MainSolutionTemplate.Core.MessageUtil;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.MessageUtil
{
	[TestFixture]
	public class MessengerTests
	{

		private Messenger _messenger;

		#region Setup/Teardown

		public void Setup()
		{
			_messenger = new Messenger();
		}

		[TearDown]
		public void TearDown()
		{

		}

		#endregion

		[Test]
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
			_messenger.Should().NotBeNull();
		}

		[Test]
		public void Send_Given_Object_ShouldBeRecieved()
		{
			// arrange
			Setup();
			var o = new object();
			string recieved = null;
			_messenger.Register<SampleMessage>(o,m => recieved = m.Message);
			// action
			_messenger.Send(new SampleMessage("String"));
			// assert
			recieved.Should().NotBeNull();
		}
		[Test]
		public void Send_GivenObject_ShouldBeRecievedOnOtherListner()
		{
			// arrange
			Setup();
			var o = new object();
			object recieved = null;
			_messenger.Register(typeof(SampleMessage),o,m => recieved = m);
			// action
			_messenger.Send(new SampleMessage("String"));
			// assert
			recieved.Should().NotBeNull();
		}
		
		[Test]
		public void Send_GivenRegisteredAndThenUnregister_ShouldNotRelieveMessage()
		{
			// arrange
			Setup();
			var o = new object();
			string recieved = null;
			_messenger.Register<SampleMessage>(o,m => recieved = m.Message);
			_messenger.Unregister<SampleMessage>(o);
			// action
			_messenger.Send(new SampleMessage("String"));
			// assert
			recieved.Should().BeNull();
		}
		
		[Test]
		public void Send_GivenRegisteredAndThenUnregisterAll_ShouldNotRelieveMessage()
		{
			// arrange
			Setup();
			var o = new object();
			string recieved = null;
			_messenger.Register<SampleMessage>(o,m => recieved = m.Message);
			_messenger.Unregister(o);
			// action
			_messenger.Send(new SampleMessage("String"));
			// assert
			recieved.Should().BeNull();
		}

		[Test]
		[Ignore("Cant force GC to do its work")]
		public void Send_GivenWeakItem_ShouldAlsoNotBeCalled()
		{
			
			// arrange
			Setup();
			string recieved;
			using (var o = new SampleMessage("what?"))
			{
				recieved = null;
				_messenger.Register<SampleMessage>(o,m => recieved = m.Message);
			}
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			// action
			_messenger.Send(new SampleMessage("String"));
			// assert
			recieved.Should().BeNull();
		}

		public class SampleMessage : IDisposable
		{
			private string _message;

			public SampleMessage(string message)
			{
				_message = message;
			}

			public string Message
			{
				get { return _message; }
			}

			#region Implementation of IDisposable

			public void Dispose()
			{
				_message = null;
			}

			#endregion
		}
	}

	
}