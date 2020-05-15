using System.Linq;

using Bumblebee.Extensions;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

namespace Bumblebee.IntegrationTests.Implementation
{
	// ReSharper disable InconsistentNaming

	[TestFixture(typeof(HeadlessChrome))]
	public class RadioButtonTests<T> : HostTestFixture
		where T : IDriverEnvironment, new()
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<RadioButtonsPage>(GetUrl("RadioButtons.html"));
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			Threaded<Session>
				.End();
		}

		[TestCase("Water", false)]
		[TestCase("Beer", false)]
		[TestCase("Wine", true)]
		public void Given_option_exists_by_text_When_checking_if_selected_Then_returns_expected(string text, bool expected)
		{
			Threaded<Session>
				.CurrentBlock<RadioButtonsPage>()
				.VerifyThat(p => p.Beverages
					.Options.WithText(text)
					.First()
					.Selected
					.Should().Be(expected));
		}

		[Test]
		public void Given_option_does_not_exist_When_selecting_by_text_Then_should_not_find_any()
		{
			Threaded<Session>
				.CurrentBlock<RadioButtonsPage>()
				.VerifyThat(p => p.Beverages
					.Options.WithText("Test")
					.Any()
					.Should().BeFalse());
		}
	}
}
