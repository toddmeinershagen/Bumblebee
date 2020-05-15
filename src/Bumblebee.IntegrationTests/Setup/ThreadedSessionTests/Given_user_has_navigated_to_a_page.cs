using Bumblebee.Extensions;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests.Setup.ThreadedSessionTests
{
	[TestFixture(typeof(HeadlessChrome))]
	public class Given_user_has_navigated_to_a_page<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		[SetUp]
		public void TestSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<DefaultPage>(GetUrl("Default.html"));
		}

		[TearDown]
		public void TestTearDown()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		public void When_getting_current_page_as_correct_type_THen_should_return_page()
		{
			Threaded<Session>
				.CurrentPage<DefaultPage>()
				.VerifyThat(p => p.Should().NotBeNull())
				.VerifyThat(p => p.Should().BeOfType<DefaultPage>());
		}

		[Test]
		public void When_getting_current_page_as_incorrect_type_Then_should_return_page()
		{
			Threaded<Session>
				.CurrentPage<KeysPage>()
				.VerifyThat(p => p.Should().NotBeNull())
				.VerifyThat(p => p.Should().BeOfType<KeysPage>());
		}
	}
}
