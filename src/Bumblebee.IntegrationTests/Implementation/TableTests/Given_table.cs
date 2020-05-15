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
	public class Given_table<T> : HostTestFixture
		where T : IDriverEnvironment, new()
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<TablePage>(GetUrl("Table.html"));
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
				.CurrentBlock<TablePage>()
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
				.CurrentBlock<TablePage>()
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
				.CurrentBlock<TablePage>()
				.Table
				.VerifyThat(x => x.Rows
					.First()["Item"]
					.Should()
					.Be("Wine"));
		}

		[Test]
		public void Should_contain_first_row_with_first_column_of_wine()
		{
			Threaded<Session>
				.CurrentBlock<TablePage>()
				.Table
				.VerifyThat(x => x.Rows
					.First()[0]
					.Should()
					.Be("Wine"));
		}

		[Test]
		public void Should_contain_first_row_with_quantity_of_four()
		{
			Threaded<Session>
				.CurrentBlock<TablePage>()
				.Table
				.VerifyThat(x => x.Rows
					.First()["Quantity"]
					.Should()
					.Be("4"));
		}

		[Test]
		public void Should_contain_first_row_with_second_column_of_four()
		{
			Threaded<Session>
				.CurrentBlock<TablePage>()
				.Table
				.VerifyThat(x => x.Rows
					.First()[1]
					.Should()
					.Be("4"));
		}

		[Test]
		public void Should_contain_first_row_with_price_of_65()
		{
			Threaded<Session>
				.CurrentBlock<TablePage>()
				.Table
				.VerifyThat(x => x.Rows
					.First()["Price"]
					.Should()
					.Be("65.00"));
		}

		[Test]
		public void Should_contain_first_row_with_third_column_of_65()
		{
			Threaded<Session>
				.CurrentBlock<TablePage>()
				.Table
				.VerifyThat(x => x.Rows
					.First()[2]
					.Should()
					.Be("65.00"));
		}
	}
}
