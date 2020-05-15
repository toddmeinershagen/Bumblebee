using System.Linq;

using Bumblebee.Extensions;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

using OpenQA.Selenium;

namespace Bumblebee.IntegrationTests.Implementation
{
	// ReSharper disable InconsistentNaming

	[TestFixture(typeof(HeadlessChrome))]
	public class Given_inline_frame<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<InlineFramesPage>(GetUrl("InlineFrames.html"));
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		public void When_tag_is_used_Child_elements_can_be_found()
		{
			Threaded<Session>
				.CurrentBlock<InlineFramesPage>()
				.Child.Tag.FindElements(By.Id(ChildFrame.TheLinkId))
				.Count()
				.Should().BeGreaterThan(0);
		}

		[Test]
		public void When_FindElements_is_used_Child_elements_can_be_found()
		{
			Threaded<Session>
				.CurrentBlock<InlineFramesPage>()
				.Child.FindElements(By.Id(ChildFrame.TheLinkId))
				.Count()
				.Should().BeGreaterThan(0);
		}

		[Test]
		public void When_parent_link_is_clicked_Then_parent_text_changes()
		{
			Threaded<Session>
				.CurrentBlock<InlineFramesPage>()
				.Child.TheLink.Click()
				.VerifyThat(page => page.Text.Should().Be("Clicked."));
		}
	}
}
