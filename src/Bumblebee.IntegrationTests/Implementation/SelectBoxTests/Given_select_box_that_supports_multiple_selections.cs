using System;
using System.Linq;

using Bumblebee.Extensions;
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
	public class Given_select_box_that_supports_multiple_selections<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		[OneTimeSetUp]
		public void SetUpFixture()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<SelectPage>(GetUrl("MultiSelect.html"));
		}

		[OneTimeTearDown]
		public void DisposeFixture()
		{
			Threaded<Session>
				.End();
		}

		[SetUp]
		public void TestSetUp()
		{
			Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings.DeselectAll();
		}

		[Test]
		public void Then_IsMultiSelect_is_true()
		{
			Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings
				.IsMultiSelect.Should().BeTrue();
		}

		[Test]
		public void When_nothing_is_selected_Then_SelectedOption_is_null()
		{
			Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings.SelectedOption
				.Should().Be(null);
		}

		[Test]
		public void When_nothing_is_selected_Then_SelectedOptions_is_empty()
		{
			Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings.SelectedOptions
				.Should().BeEmpty();
		}

		[Test]
		public void When_item_that_does_not_exist_is_selected_by_text_Then_exception_is_thrown()
		{
			Action fn = () => Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings.SelectByText("Hello");

			fn.ShouldThrow<NoSuchElementException>();
		}

		[Test]
		public void When_item_that_does_not_exist_is_selected_by_value_Then_exception_is_thrown()
		{
			Action fn = () => Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings.SelectByValue("Hello");

			fn.ShouldThrow<NoSuchElementException>();
		}

		[Test]
		public void When_item_that_does_not_exist_is_selected_by_index_Then_exception_is_thrown()
		{
			Action fn = () => Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings.SelectByIndex(-1);

			fn.ShouldThrow<NoSuchElementException>();
		}

		// TODO: deselect items that do not exist

		[Test]
		public void When_item_is_selected_by_text_Then_item_is_selected()
		{
			var text = "onions";

			Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings.SelectByText(text)
				.Toppings.SelectedOption
				.Text.Should().Be(text);
		}

		[Test]
		public void When_item_is_selected_by_value_Then_item_is_selected()
		{
			var value = "2";

			Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings.SelectByValue(value)
				.Toppings.SelectedOption
				.GetAttribute("value").Should().Be(value);
		}

		[Test]
		public void When_item_is_selected_by_index_Then_item_is_selected()
		{
			var index = 0;

			Threaded<Session>
				.CurrentPage<SelectPage>()
				.Toppings.SelectByIndex(index)
				.Toppings.SelectedOption
				.GetAttribute("value").Should().Be("1");
		}

		[Test, Ignore("Not working with HeadlessChrome")]
		public void When_selecting_multiple_values_Then_selection_occurs()
		{
			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.Options.First().Click()
				.Toppings.Options.Last().Click()
				.VerifyThat(p => p.Toppings.Options
					.Count(x => x.Selected)
					.Should().Be(2))
				.VerifyThat(p => p.Toppings.Options
					.First().Selected
					.Should().BeTrue())
				.VerifyThat(p => p.Toppings.Options
					.Last().Selected
					.Should().BeTrue());
		}

		[Test, Ignore("Not working with HeadlessChrome")]
		public void When_selecting_and_deselecting_a_value_Then_nothing_is_selected()
		{
			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.Options.First().Click()
				.VerifyThat(p => p.Toppings.Options
					.Count(x => x.Selected)
					.Should().Be(1))
				.Toppings.Options.First().Click()
				.VerifyThat(p => p.Toppings.Options
					.Count(x => x.Selected)
					.Should().Be(0));
		}

		[Test]
		public void When_selecting_and_deselecting_by_text_Then_nothing_is_selected()
		{
			var text = "onions";

			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.SelectByText(text)
				.VerifyThat(p => p.Toppings.Options
					.Count(x => x.Selected)
					.Should().Be(1))
				.Toppings.DeselectByText(text)
				.VerifyThat(p => p.Toppings.Options
					.Count(x => x.Selected)
					.Should().Be(0));
		}

		[Test]
		public void When_selecting_and_deselecting_by_value_Then_nothing_is_selected()
		{
			var value = "3";

			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.SelectByValue(value)
				.VerifyThat(p => p.Toppings.Options
					.Count(x => x.Selected)
					.Should().Be(1))
				.Toppings.DeselectByValue(value)
				.VerifyThat(p => p.Toppings.Options
					.Count(x => x.Selected)
					.Should().Be(0));
		}

		[Test]
		public void When_selecting_and_deselecting_by_index_Then_nothing_is_selected()
		{
			var index = 2;

			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.SelectByIndex(index)
				.VerifyThat(p => p.Toppings.Options
					.Count(x => x.Selected)
					.Should().Be(1))
				.Toppings.DeselectByIndex(index)
				.VerifyThat(p => p.Toppings.Options
					.Count(x => x.Selected)
					.Should().Be(0));
		}

		// TODO: select an item that is already selected

		[Test]
		public void When_selecting_an_item_that_is_already_selected_by_text_Then_one_item_is_selected()
		{
			var text = "onions";

			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.SelectByText(text)
				.Toppings.SelectByText(text)
				.VerifyThat(p => p.Toppings.SelectedOptions
					.Count()
					.Should().Be(1));
		}

		[Test]
		public void When_selecting_an_item_that_is_already_selected_by_value_Then_one_item_is_selected()
		{
			var value = "3";

			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.SelectByValue(value)
				.Toppings.SelectByValue(value)
				.VerifyThat(p => p.Toppings.SelectedOptions
					.Count()
					.Should().Be(1));
		}

		[Test]
		public void When_selecting_an_item_that_is_already_selected_by_index_Then_one_item_is_selected()
		{
			var index = 2;

			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.SelectByIndex(index)
				.Toppings.SelectByIndex(index)
				.VerifyThat(p => p.Toppings.SelectedOptions
					.Count()
					.Should().Be(1));
		}

		[Test]
		public void When_deselecting_an_item_that_is_not_selected_by_text_Then_nothing_is_selected()
		{
			var text = "onions";

			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.DeselectByText(text)
				.VerifyThat(p => p.Toppings.SelectedOptions
					.Count()
					.Should().Be(0));
		}

		[Test]
		public void When_deselecting_an_item_that_is_not_selected_by_value_Then_nothing_is_selected()
		{
			var value = "3";

			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.DeselectByValue(value)
				.VerifyThat(p => p.Toppings.SelectedOptions
					.Count()
					.Should().Be(0));
		}

		[Test]
		public void When_deselecting_an_item_that_is_not_selected_by_index_Then_nothing_is_selected()
		{
			var index = 2;

			Threaded<Session>
				.CurrentBlock<SelectPage>()
				.Toppings.DeselectByIndex(index)
				.VerifyThat(p => p.Toppings.SelectedOptions
					.Count()
					.Should().Be(0));
		}
	}
}
