using System;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests.Setup.SessionTests
{
	[TestFixture(typeof(HeadlessChrome))]
	public class Given_session_with_default_settings<T> : HostTestFixture
		where T : IDriverEnvironment, new()
	{
		private static readonly TestCaseData[] TestCases =
		{
			new TestCaseData("screenshot.png", ImageFormat.Png, "image/x-png"),
			new TestCaseData("screenshot.jpg", ImageFormat.Jpeg, "image/pjpeg"),
			new TestCaseData("screenshot.jpeg", ImageFormat.Jpeg, "image/pjpeg"),
			new TestCaseData("screenshot.bmp", ImageFormat.Bmp, "image/bmp"),
			new TestCaseData("screenshot.gif", ImageFormat.Gif, "image/gif")
		};

		private Session _session;

		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			_session = new Session(new T());
		}

		[OneTimeTearDown]
		public void TestFixtureTearDown()
		{
			_session.End();

			_session = null;
		}

		[TestCaseSource(nameof (TestCases))]
		public void When_CaptureScreen_is_called_Then_takes_screenshot_of_correct_format(string path, ImageFormat format, string expected)
		{
			File.Delete(path);

			_session.CaptureScreen(path);

			var actual = MimeProvider.GetMimeFromFile(path);

			actual.Should().Be(expected);

			File.Delete(path);
		}

		[Test]
		public void When_CaptureScreen_is_called_with_invalid_file_extension_Then_an_exception_is_thrown()
		{
			Action fn = () => _session.CaptureScreen("screenshot.txt");

			fn.ShouldThrow<ArgumentException>();
		}
	}

	public static class MimeProvider
	{
		[DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
		private static extern int FindMimeFromData(IntPtr pBC,
			[MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
			[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)]
			byte[] pBuffer,
			int cbSize,
			[MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
			int dwMimeFlags,
			out IntPtr ppwzMimeOut,
			int dwReserved);

		public static string GetMimeFromFile(string filename)
		{
			if (File.Exists(filename) == false)
			{
				throw new FileNotFoundException($"File '{filename}' not found");
			}

			int numberOfBytesRead;
			byte[] buffer = new byte[256];

			using (var fs = new FileStream(filename, FileMode.Open))
			{
				var length = (int) Math.Min(256, fs.Length);

				numberOfBytesRead = fs.Read(buffer, 0, length);
			}

			string result;

			try
			{
				IntPtr mimetype;

				FindMimeFromData(IntPtr.Zero, null, buffer, numberOfBytesRead, null, 0, out mimetype, 0);

				result = Marshal.PtrToStringUni(mimetype);

				Marshal.FreeCoTaskMem(mimetype);
			}
			catch (Exception e)
			{
				throw new ApplicationException($"Unable to determine type of file '{filename}'.", e);
			}

			return result;
		}
	}
}
