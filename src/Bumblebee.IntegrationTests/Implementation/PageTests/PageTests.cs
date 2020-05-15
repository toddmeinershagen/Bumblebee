using System;
using System.Threading;

using Bumblebee.Extensions;
using Bumblebee.Implementation;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests.Implementation.PageTests
{
	[TestFixture(typeof(HeadlessChrome))]
	public class Given_session_when_constructing<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		private readonly ThreadLocal<Session> _session = new ThreadLocal<Session>(() => new Session(new T()));

		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			Session
				.NavigateTo<TestablePage>(GetUrl("Dialogs.html"));
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			Session
				.End();
		}

		[Test]
		public void Should_load_body_as_tag()
		{
			Session
				.CurrentBlock<TestablePage>()
				.Tag
				.VerifyThat(t => t.TagName.Should().Be("body"));
		}

		[Test]
		public void should_leave_parent_null()
		{
			Session
				.CurrentBlock<TestablePage>()
				.VerifyThat(p => p.Parent.Should().BeNull());
		}

		[Test]
		public void should_return_same_session()
		{
			Session
				.CurrentBlock<TestablePage>()
				.VerifyThat(p => p.Session.Should().Be(Session));
		}

		public Session Session => _session.Value;
    }

	[TestFixture]
	public class PageTests
	{
		[Test]
		public void Given_null_session_when_constructing_page_should_throw()
		{
			Action action = () => new TestablePage(null);
			action.ShouldThrow<ArgumentNullException>()
				.WithMessage("Value cannot be null.\r\nParameter name: session");
		}
	}

	public class TestablePage : Page
	{
		public TestablePage(Session session) : base(session)
		{
		}
	}
}
