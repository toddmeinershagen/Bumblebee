using System.IO;

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
	public class Given_environment_and_custom_settings_When_capturing<T> : HostTestFixture
	    where T : IDriverEnvironment, new()
    {
		private string _filePath;
		private Session _session;
		private Session _returnSession;

		[OneTimeSetUp]
		public void Before()
		{
			var currentMethod = CallStack.GetCurrentMethod().GetFullName();

			var path = Path.GetTempPath();
			_filePath = Path.ChangeExtension(Path.Combine(path, currentMethod), "png");
			File.Delete(_filePath);

			var settings = new Settings
			{
				ScreenCapturePath = path
			};

			var environment = new T();
			_session = new Session(environment, settings);
			_session.NavigateTo<CheckboxPage>(GetUrl("Checkbox.html"));
			_returnSession = _session.CaptureScreen(_filePath);
		}

		[OneTimeTearDown]
		public void After()
		{
			_session.End();
			File.Delete(_filePath);
		}

		[Test]
		public void Should_save_in_path()
		{
			File.Exists(_filePath).Should().BeTrue();
		}

		[Test]
		public void Should_return_session()
		{
			_returnSession.Should().Be(_session);
		}
	}
}
