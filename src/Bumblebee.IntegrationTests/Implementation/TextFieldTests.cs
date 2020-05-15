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
	public class Given_no_wait<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		[SetUp]
		public void BeforeEach()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<TextFieldPage>(GetUrl("TextField.html"));
		}

		[TearDown]
		public void AfterEach()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		public void When_entering_text_Then_text_should_work()
		{
			const string expectedText1 = "This is the text.";
			const string expectedText2 = "This is the 2nd expected text.";

			Threaded<Session>
				.CurrentBlock<TextFieldPage>()
				.Text.EnterText(expectedText1)
				.VerifyThat(x => x.Text.Text.Should().Be(expectedText1))
				.Text.EnterText(expectedText2)
				.VerifyThat(x => x.Text.Text.Should().Be(expectedText2));
		}

		[Test]
		public void When_appending_text_Then_text_should_work()
		{
			const string expectedText1 = "First text";
			const string expectedText2 = " - Second text";

			Threaded<Session>
				.CurrentBlock<TextFieldPage>()
				.Text.AppendText(expectedText1)
				.VerifyThat(x => x.Text.Text.Should().Be(expectedText1))
				.Text.AppendText(expectedText2)
				.VerifyThat(x => x.Text.Text.Should().Be(expectedText1 + expectedText2));
		}
	}
}
