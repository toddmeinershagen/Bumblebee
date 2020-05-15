using System;

using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

using OpenQA.Selenium;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests.Implementation.PageTests
{
	public class Given_slow_page_with_no_wait : HostTestFixture
    {
		[SetUp]
		public void TestSetUp()
		{
			Threaded<Session>
				.With(new HeadlessChrome(TimeSpan.Zero))
				.NavigateTo<SlowPageWithNoWait>(GetUrl("SlowPage.html"));
		}

		[TearDown]
		public void TestDispose()
		{
			Threaded<Session>
				.CurrentBlock<SlowPageWithNoWait>()
				.Session.End();
		}

		[Test]
		public void When_getting_text_of_textfield_Should_throw()
		{
			Action action = () =>
				Threaded<Session>
					.CurrentBlock<SlowPageWithNoWait>()
					.FirstName
					.Text
					.Should().Be("Todd");

			action
				.ShouldThrow<WebDriverTimeoutException>()
				.WithMessage("Timed out after 0 seconds");
		}

		[Test]
		public void When_getting_checked_indicator_for_checkbox_should_throw()
		{
			Action action = () =>
				Threaded<Session>
					.CurrentBlock<SlowPageWithNoWait>()
					.Checkbox
					.Selected
					.Should().BeTrue();

			action
				.ShouldThrow<WebDriverTimeoutException>()
				.WithMessage("Timed out after 0 seconds");
		}
	}
}
