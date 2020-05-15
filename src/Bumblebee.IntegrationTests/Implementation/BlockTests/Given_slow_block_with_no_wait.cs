using System;

using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

using OpenQA.Selenium;

namespace Bumblebee.IntegrationTests.Implementation
{
    [TestFixture]
    public class Given_slow_block_with_no_wait : HostTestFixture
    {
        [SetUp]
        public void TestSetUp()
        {
            Threaded<Session>
                .With(new HeadlessChrome(TimeSpan.Zero))
                .NavigateTo<SlowBlockPage>(GetUrl("SlowBlock.html"));
        }

        [TearDown]
        public void TestDispose()
        {
            Threaded<Session>
                .CurrentBlock<SlowBlockPage>()
                .Session.End();
        }

        [Test]
        public void When_getting_text_of_textfield_Should_throw()
        {
            Action action = () =>
                Threaded<Session>
                    .CurrentBlock<SlowBlockPage>()
                    .CustomerInfoWithNoWait
                    .FirstName
                    .Text
                    .Should().Be("Todd");

            action
                .ShouldThrow<WebDriverTimeoutException>()
                .Where(x => x.Message.StartsWith("Timed out after"));
        }
    }
}