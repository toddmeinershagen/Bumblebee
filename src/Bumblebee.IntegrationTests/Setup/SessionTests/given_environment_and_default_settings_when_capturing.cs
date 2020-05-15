using System.IO;
using System.Reflection;

using Bumblebee.Extensions;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests.Setup.SessionTests
{
	[TestFixture(typeof(HeadlessChrome))]
	public class Given_environment_and_default_settings_When_capturing<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		private string path;
		private Session session;
		private Session _returnSession;

		[OneTimeSetUp]
		public void Before()
		{
			var currentMethod = $"{MethodBase.GetCurrentMethod().GetFullName()}.png";
			var defaultSettings = new Settings();

			path = Path.Combine(defaultSettings.ScreenCapturePath, currentMethod);
			File.Delete(path);

			var environment = new T();
			session = new Session(environment);
			session.NavigateTo<CheckboxPage>(GetUrl("Checkbox.html"));

			_returnSession = session.CaptureScreen();
		}

		[OneTimeTearDown]
		public void After()
		{
			session.End();
			File.Delete(path);
		}

		[Test]
		public void should_save_in_executing_directory()
		{
			File.Exists(path).Should().BeTrue();
		}

		[Test]
		public void should_return_session()
		{
			_returnSession.Should().Be(session);
		}
	}
}
