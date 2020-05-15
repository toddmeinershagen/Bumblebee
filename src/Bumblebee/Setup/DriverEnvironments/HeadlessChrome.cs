using System;

using OpenQA.Selenium.Chrome;

namespace Bumblebee.Setup.DriverEnvironments
{
	public class HeadlessChrome : SimpleDriverEnvironment<HeadlessChromeDriver>
	{
		public HeadlessChrome()
		{
		}

		public HeadlessChrome(TimeSpan timeToWait) : base(timeToWait)
		{
		}
	}

	public class HeadlessChromeDriver : ChromeDriver
    {
		public HeadlessChromeDriver() : base(GetInitialOptions())
		{ }

		private static ChromeOptions GetInitialOptions()
		{
			var options = new ChromeOptions();
			options.AddArgument("--headless");
			options.AddArgument("--disable-gpu");
			options.AddArgument("--no-sandbox");

			return options;
		}
    }
}
