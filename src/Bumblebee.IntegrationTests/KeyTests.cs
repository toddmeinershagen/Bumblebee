using Bumblebee.Extensions;
using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bumblebee.IntegrationTests
{
	[TestFixture(typeof (HeadlessChrome))]
	public class Given_keys<T> : HostTestFixture
		where T : IDriverEnvironment, new()
	{
		private static readonly Key[] Data =
		{
			Key.D0,
			Key.D1,
			Key.D2,
			Key.D3,
			Key.D4,
			Key.D5,
			Key.D6,
			Key.D7,
			Key.D8,
			Key.D9,
			Key.A,
			Key.B,
			Key.C,
			Key.D,
			Key.E,
			Key.F,
			Key.G,
			Key.H,
			Key.I,
			Key.J,
			Key.K,
			Key.L,
			Key.M,
			Key.N,
			Key.O,
			Key.P,
			Key.Q,
			Key.R,
			Key.S,
			Key.T,
			Key.U,
			Key.V,
			Key.W,
			Key.X,
			Key.Y,
			Key.Z,
			Key.Alt,
			Key.Apostrophe,
			Key.Backslash,
			Key.Backspace,
			Key.Comma,
			Key.Period,
			Key.Slash,
			Key.Command,
			Key.Control,
			Key.Enter,
			Key.Equal,
			Key.Escape,
			Key.Grave,
			Key.LeftBracket,
			Key.RightBracket,
			Key.Semicolon,
			Key.Shift,
			Key.Space,
			Key.Tab,
			Key.Function.F1,
			Key.Function.F2,
			Key.Function.F3,
			Key.Function.F4,
			Key.Function.F5,
			Key.Function.F6,
			Key.Function.F7,
			Key.Function.F8,
			Key.Function.F9,
			Key.Function.F10,
			Key.Function.F11,
			Key.Function.F12,
			Key.Arrows.Up,
			Key.Arrows.Down,
			Key.Arrows.Left,
			Key.Arrows.Right,
			Key.Other.Insert,
			Key.Other.Delete,
			Key.Other.Home,
			Key.Other.End,
			Key.Other.PageUp,
			Key.Other.PageDown,
			Key.Other.Pause,
			Key.Numpad.NumberPad0,
			Key.Numpad.NumberPad1,
			Key.Numpad.NumberPad2,
			Key.Numpad.NumberPad3,
			Key.Numpad.NumberPad4,
			Key.Numpad.NumberPad5,
			Key.Numpad.NumberPad6,
			Key.Numpad.NumberPad7,
			Key.Numpad.NumberPad8,
			Key.Numpad.NumberPad9,
			Key.Numpad.Add,
			Key.Numpad.Decimal,
			Key.Numpad.Divide,
			Key.Numpad.Multiply,
			Key.Numpad.Subtract,
			Key.Control + Key.A,
			Key.Control + Key.C,
			Key.Control + Key.V,
			Key.Control + Key.X,
			Key.Control + Key.Space,
			Key.Control + Key.Alt + Key.F,
			Key.Control + Key.Alt + Key.Shift + Key.J
		};

		[OneTimeSetUp]
		public void TestSetUp()
		{
			Threaded<Session>
				.With<T>()
				.NavigateTo<KeysPage>(GetUrl("Keys.html"));
		}

		[OneTimeTearDown]
		public void TestTearDown()
		{
			Threaded<Session>
				.End();
		}

		[Test]
		[TestCaseSource(nameof (Data))]
		public void When_key_is_pressed_correct_key_event_is_fired(Key key)
		{
			Threaded<Session>
				.CurrentBlock<KeysPage>()
				.KeysText.Press(key)
				.VerifyThat(x => x.KeyPressed
					.Should()
					.Be(key.ToString()));
		}
	}
}
