using System;
using System.Collections.Generic;
using System.Linq;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;

namespace Bumblebee.IntegrationTests.Setup
{
	//ReSharper disable InconsistentNaming

	//[TestFixture(typeof (Firefox), typeof (FirefoxDriver))]
	[TestFixture(typeof (InternetExplorer), typeof (InternetExplorerDriver))]
	[TestFixture(typeof (HeadlessChrome), typeof(HeadlessChromeDriver))]
	[TestFixture(typeof (Chrome), typeof (ChromeDriver))]
	[TestFixture(typeof (PhantomJS), typeof (PhantomJSDriver))]
	public class DriverEnvironmentTests<TDriverEnvironment, TExpectedDriver>
		where TDriverEnvironment : IDriverEnvironment, new()
		where TExpectedDriver : IWebDriver
	{
		[Test]
		public void given_driver_environment_when_creating_web_driver_should_be_correct_type()
		{
			var environment = new TDriverEnvironment();

			using (var driver = environment.CreateWebDriver())
			{
				driver.Should().BeOfType<TExpectedDriver>();
			}
		}
	}

	//[TestFixture(typeof (Firefox))]
	[TestFixture(typeof (InternetExplorer))]
	[TestFixture(typeof(HeadlessChrome))]
	[TestFixture(typeof (Chrome))]
	[TestFixture(typeof (PhantomJS))]
	public class DriverEnvironmentTests<TDriverEnvironment>
		where TDriverEnvironment : IDriverEnvironment
	{
		[Test]
		public void given_driver_environment_with_time_to_wait_when_creating_web_driver_should_not_throw()
		{
			Action action = ConstructDriverWithTimeSpan;
			action.ShouldNotThrow();
		}

		private void ConstructDriverWithTimeSpan()
		{
			var type = typeof (TDriverEnvironment);
			IList<Type> constructorSignature = new List<Type> { typeof (TimeSpan) };
			IList<object> constructorArgs = new List<object> { TimeSpan.FromSeconds(2) };
			var constructor = type.GetConstructor(constructorSignature.ToArray());

			if (constructor == null)
			{
				var message = $"The result type specified ({typeof (TDriverEnvironment)}) is not a valid SimpleDriverEnvironment<T>.  It must have a constructor overload that takes a TimeSpan.";
				throw new ArgumentException(message);
			}

			var environment = constructor.Invoke(constructorArgs.ToArray()) as IDriverEnvironment;
			using (environment.CreateWebDriver())
			{
			}
		}
	}
}
