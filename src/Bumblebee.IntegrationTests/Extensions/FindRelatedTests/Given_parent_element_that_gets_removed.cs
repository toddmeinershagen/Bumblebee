using Bumblebee.Extensions;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests.Extensions.FindRelatedTests
{
	[TestFixture]
	public class Given_parent_element_that_gets_removed : HostTestFixture
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			Threaded<Session>
				.With<HeadlessChrome>()
				.NavigateTo<JavaScriptPopUpPage>(GetUrl("JavaScriptPopUp.html"));
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		public void When_grandparent_is_searched_for_Then_no_exception_is_thrown()
		{
			Threaded<Session>
				.CurrentBlock<JavaScriptPopUpPage>()
				.Region.OpenPopUp.Click()
				.Close.Click()
				.Verify(x => x.OpenPopUp.Text == "Open Pop Up");
		}
	}
}
