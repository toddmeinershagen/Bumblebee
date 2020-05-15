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
	public class Given_checkbox<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		[SetUp]
		public void TestSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<CheckboxPage>(GetUrl("Checkbox.html"));
		}

		[TearDown]
		public void TestTearDown()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		public void When_page_loaded_Then_checked_checkbox_is_initially_checked()
		{
			Threaded<Session>
				.CurrentBlock<CheckboxPage>()
				.CheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeTrue());
		}

		[Test]
		public void When_checked_checkbox_is_checked_Then_it_remains_checked()
		{
			Threaded<Session>
				.CurrentBlock<CheckboxPage>()
				.CheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeTrue())
				.Check()
				.CheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeTrue());
		}

		[Test]
		public void When_checked_checkbox_is_unchecked_Then_it_gets_unchecked()
		{
			Threaded<Session>
				.CurrentBlock<CheckboxPage>()
				.CheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeTrue())
				.Uncheck()
				.CheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeFalse());
		}

		[Test]
		public void When_checked_checkbox_is_toggled_Then_it_gets_unchecked()
		{
			Threaded<Session>
				.CurrentBlock<CheckboxPage>()
				.CheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeTrue())
				.Toggle()
				.CheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeFalse());
		}

		[Test]
		public void When_page_loaded_unchecked_checkbox_is_initially_unchecked()
		{
			Threaded<Session>
				.CurrentBlock<CheckboxPage>()
				.UncheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeFalse());
		}

		[Test]
		public void When_unchecked_checkbox_is_checked_Then_it_gets_checked()
		{
			Threaded<Session>
				.CurrentBlock<CheckboxPage>()
				.UncheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeFalse())
				.Check()
				.UncheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeTrue());
		}

		[Test]
		public void When_unchecked_checkbox_is_unchecked_Then_it_remains_unchecked()
		{
			Threaded<Session>
				.CurrentBlock<CheckboxPage>()
				.UncheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeFalse())
				.Uncheck()
				.UncheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeFalse());
		}

		[Test]
		public void When_unchecked_checkbox_is_toggled_Then_it_gets_checked()
		{
			Threaded<Session>
				.CurrentBlock<CheckboxPage>()
				.UncheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeFalse())
				.Toggle()
				.UncheckedCheckbox
				.VerifyThat(x => x.Selected.Should().BeTrue());
		}
	}
}
