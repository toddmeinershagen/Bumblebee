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

	public class Given_environment_and_default_settings_When_capturing_from_generic_method<T> : HostTestFixture
		where T : IDriverEnvironment, new()
	{
		[Test]
		public void Should_save_in_executing_directory()
		{
			var environment = new T();
			var session = new Session(environment);
			session.NavigateTo<CheckboxPage>(GetUrl("Checkbox.html"));

			var path = GenericMethod(session);

			File.Exists(path).Should().BeTrue();

			session.End();
			File.Delete(path);
		}

		private string GenericMethod<TSession>(TSession session) where TSession : Session
		{
			var currentMethod = $"{CallStack.GetCurrentMethod().GetFullName()}.png";
			var defaultSettings = new Settings();

			var path = Path.Combine(defaultSettings.ScreenCapturePath, currentMethod);
			File.Delete(path);

			session.CaptureScreen();

			return path;
		}
	}
}
