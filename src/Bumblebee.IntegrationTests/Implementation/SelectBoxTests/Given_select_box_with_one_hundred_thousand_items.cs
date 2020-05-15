using System;

using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

using OpenQA.Selenium;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests.Implementation.SelectBoxTests
{
	[TestFixture(typeof(HeadlessChrome))]
	public class Given_select_box_with_one_hundred_thousand_items<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<HundredThousandItemSelectPage>(GetUrl("HundredThousandItemSelect.html"));
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			Threaded<Session>
				.End();
		}

		[SetUp]
		public void TestSetUp()
		{
			Threaded<Session>
				.Refresh();
		}

		[Test]
		public void When_item_that_does_not_exist_is_selected_by_text_Then_exception_is_thrown()
		{
			Action fn = () => Threaded<Session>
				.CurrentPage<HundredThousandItemSelectPage>()
				.SelectBox.SelectByText("Hello");

			fn.ShouldThrow<NoSuchElementException>();
		}

		[Test]
		public void When_item_that_does_not_exist_is_selected_by_value_Then_exception_is_thrown()
		{
			Action fn = () => Threaded<Session>
				.CurrentPage<HundredThousandItemSelectPage>()
				.SelectBox.SelectByValue("Hello");

			fn.ShouldThrow<NoSuchElementException>();
		}

		[Test]
		public void When_item_is_selected_by_text_Then_item_is_selected()
		{
			var text = "74892";

			Threaded<Session>
				.CurrentPage<HundredThousandItemSelectPage>()
				.SelectBox.SelectByText(text)
				.Session.ExecuteJavaScript<string>("var element = document.getElementById('HundredThousandItemSelect'), index = element.selectedIndex; return (index === -1) ? null : element.options[index].text;")
				.Should().Be(text);
		}

		[Test]
		public void When_item_is_selected_by_value_Then_item_is_selected()
		{
			var value = "74892";

			Threaded<Session>
				.CurrentPage<HundredThousandItemSelectPage>()
				.SelectBox.SelectByValue(value)
				.Session.ExecuteJavaScript<string>("var element = document.getElementById('HundredThousandItemSelect'), index = element.selectedIndex; return (index === -1) ? null : element.options[index].value;")
				.Should().Be(value);
		}
	}
}
