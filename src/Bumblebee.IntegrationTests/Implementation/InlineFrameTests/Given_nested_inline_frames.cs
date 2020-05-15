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
	public class Given_nested_inline_frames<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		[SetUp]
		public void TestSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<NestedInlineFramesPage>(GetUrl("NestedInlineFrames.html"));
		}

		[TearDown]
		public void TestTearDown()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		public void When_parent_link_clicked_Then_parent_text_changes()
		{
			Threaded<Session>
				.CurrentBlock<NestedInlineFramesPage>()
				.ChildFrame
				.ChildFrame
				.ParentLink.Click()
				.VerifyThat(page => page.Text
					.Should()
					.Be("Clicked."));
		}

		[Test]
		public void When_parent_link_clicked_Then_grandparent_text_does_not_change()
		{
			Threaded<Session>
				.CurrentBlock<NestedInlineFramesPage>()
				.ChildFrame
				.ChildFrame
				.ParentLink.Click()
				.VerifyThat(page => page.ParentAs<NestedInlineFramesPage>()
					.Text
					.Should()
					.Be("Not clicked."));
		}

		[Test]
		public void When_grandparent_link_clicked_Then_grandparent_text_changes()
		{
			Threaded<Session>
				.CurrentBlock<NestedInlineFramesPage>()
				.ChildFrame
				.ChildFrame
				.GrandparentLink.Click()
				.VerifyThat(page => page.Text.Should().Be("Clicked."));
		}

		[Test]
		public void When_grandparent_link_clicked_Then_parent_text_does_not_change()
		{
			Threaded<Session>
				.CurrentBlock<NestedInlineFramesPage>()
				.ChildFrame
				.ChildFrame
				.GrandparentLink.Click()
				.VerifyThat(page => page.ChildFrame.Text.Should().Be("Not clicked."));
		}
	}
}
