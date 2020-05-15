using System;

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
	public class Given_date_field<T> : HostTestFixture
		where T : IDriverEnvironment, new()
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<DateFieldPage>(GetUrl("DateField.html"));
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		public void When_entering_date_Then_text_and_value_work()
		{
			Threaded<Session>
				.CurrentBlock<DateFieldPage>()
				.Date.EnterDate(DateTime.Today)
				.VerifyThat(x => x.Date.Value
					.Should().Be(DateTime.Today))
				.VerifyThat(x => x.Date.Text
					.Should().Be(DateTime.Today.ToString("yyyy-MM-dd")));
		}
	}
}
