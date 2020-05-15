using System.Linq;

using Bumblebee.Extensions;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

namespace Bumblebee.IntegrationTests.Implementation.TableTests
{
	// ReSharper disable InconsistentNaming

	[TestFixture(typeof(HeadlessChrome))]
	public class Given_generic_table<T> : HostTestFixture
		where T : IDriverEnvironment, new()
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<GenericTablePage>(GetUrl("Table.html"));
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		public void Should_contain_three_columns()
		{
			Threaded<Session>
				.CurrentBlock<GenericTablePage>()
				.Table
				.VerifyThat(x => x.Headers
					.Count()
					.Should()
					.Be(3));
		}

		[Test]
		public void Should_contain_three_rows()
		{
			Threaded<Session>
				.CurrentBlock<GenericTablePage>()
				.Table
				.VerifyThat(x => x.Rows
					.Count()
					.Should()
					.Be(3));
		}

		[Test]
		public void Should_contain_first_row_with_item_of_wine()
		{
			Threaded<Session>
				.CurrentBlock<GenericTablePage>()
				.Table
				.VerifyThat(x => x.RowsAs<GenericTableRow>()
					.First()
					.Item
					.Should()
					.Be("Wine"));
		}

		[Test]
		public void Should_contain_first_row_with_quantity_of_four()
		{
			Threaded<Session>
				.CurrentBlock<GenericTablePage>()
				.Table
				.VerifyThat(x => x.RowsAs<GenericTableRow>()
					.First()
					.Quantity
					.Should()
					.Be(4));
		}

		[Test]
		public void Should_contain_first_row_with_price_of_65()
		{
			Threaded<Session>
				.CurrentBlock<GenericTablePage>()
				.Table
				.VerifyThat(x => x.RowsAs<GenericTableRow>()
					.First()
					.Price
					.Should()
					.Be(65.00m));
		}
	}
}
