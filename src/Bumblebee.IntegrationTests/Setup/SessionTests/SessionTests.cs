using Bumblebee.Extensions;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

using OpenQA.Selenium.IE;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests.Setup.SessionTests
{
	[TestFixture]
	public class SessionTests : HostTestFixture
	{
		[Test]
		public void Given_driver_environment_instance_When_instantiating_Should_set_driver_based_on_driver_environment()
		{
			var session = new Session(new HeadlessChrome());
			session.Driver.Should().BeOfType<HeadlessChromeDriver>();
			session.End();
		}

		[Test]
		public void Given_driver_environment_type_When_instantiating_Should_set_driver_based_on_driver_environment()
		{
			var session = new Session<HeadlessChrome>();
			session.Driver.Should().BeOfType<HeadlessChromeDriver>();
			session.End();
		}

		[Test]
		public void given_session_when_navigating_to_url_should_redirect_to_url()
		{
			var url = GetUrl("Default.html");
			var session = new Session<HeadlessChrome>();
			session
				.NavigateTo<DefaultPage>(url)
				.VerifyThat(p => p
					.Session
					.Driver
					.Url
					.Should().Be(url))
				.Session.End();
		}

		[Test]
		public void given_session_when_navigating_to_url_with_arguments_should_redirect_to_formatted_url()
		{
			var urlFormat = GetUrl("Default.html?id={0}&firstName={1}&lastName={2}");
			var session = new Session<HeadlessChrome>();
			session
				.NavigateTo<DefaultPage>(urlFormat, 1, "Todd", "Meinershagen")
				.VerifyThat(p => p
					.Session
					.Driver
					.Url
					.Should().Be(GetUrl("Default.html?id=1&firstName=Todd&lastName=Meinershagen")))
				.Session.End();
		}

		[Test]
		public void Given_clicked_to_new_page_When_getting_current_block_Then_should_return_new_page()
		{
			var url = GetUrl("CurrentBlock-Default.html");
			var session = new Session(new HeadlessChrome());
			session
				.NavigateTo<CurrentBlockDefaultPage>(url)
				.LinkToNavigateToPage.Click();

			session
				.CurrentBlock<CurrentBlockNavigateToPage>()
				.VerifyThat(p => p.Session.Driver.Title.Should().Be("CurrentBlock - NavigateTo"));

			session.End();
		}

		[Test]
		public void Given_scoped_block_When_getting_current_block_for_scoped_block_Then_should_return_scoped_block()
		{
			var session = new Session(new HeadlessChrome());
			session
				.NavigateTo<CurrentBlockDefaultPage>(GetUrl("CurrentBlock-Default.html"))
				.InnerSection
				.VerifyThat(b => b.SpanText.Should().Be("Span 2"));

			session
				.CurrentBlock<InnerSection>()
				.VerifyThat(b => b.SpanText.Should().Be("Span 2"));

			session.End();
		}

		[Test]
		public void Given_scoped_block_When_getting_current_block_for_parent_of_scoped_block_Then_should_return_scoped_block()
		{
			var session = new Session(new HeadlessChrome());
			session
				.NavigateTo<CurrentBlockDefaultPage>(GetUrl("CurrentBlock-Default.html"))
				.InnerSection
				.InnerInnerSection
				.TextBox
				.VerifyThat(e => e.Text.Should().Be("Todd"));

			session
				.CurrentBlock<InnerSection>()
				.VerifyThat(b => b.SpanText.Should().Be("Span 2"));

			session.End();
		}
	}
}
