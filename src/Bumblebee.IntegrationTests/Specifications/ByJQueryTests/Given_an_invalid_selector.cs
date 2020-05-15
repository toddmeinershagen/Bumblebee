using System;

using Bumblebee.IntegrationTests.Shared;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.JQuery;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests.Specifications.ByJQueryTests
{
	[TestFixture]
	public class Given_an_invalid_selector : HostTestFixture
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			Threaded<Session>
				.With<HeadlessChrome>()
				.NavigateTo<PageWithJQuery>(GetUrl("PageWithJQuery.html"));
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		public void When_null_selector_is_provided_Then_exception_is_thrown()
		{
			Action fn = () => new ByJQuery(null);

			fn.ShouldThrow<ArgumentNullException>();
		}

		[Test]
		public void When_empty_string_selector_is_provided_Then_exception_is_thrown()
		{
			Action fn = () => new ByJQuery(String.Empty);

			fn.ShouldThrow<ArgumentNullException>();
		}

		[Test]
		public void When_whitespace_string_selector_is_provided_Then_exception_is_thrown()
		{
			Action fn = () => new ByJQuery(" \t\r\n");

			fn.ShouldThrow<ArgumentNullException>();
		}
	}
}
