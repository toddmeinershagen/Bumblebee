using Bumblebee.IntegrationTests.Shared.Hosting;
using Bumblebee.IntegrationTests.Shared.Pages;
using Bumblebee.Setup;
using Bumblebee.Setup.DriverEnvironments;

using FluentAssertions;

using NUnit.Framework;

namespace Bumblebee.IntegrationTests.Implementation
{
    public class Given_slow_block_with_explicit_wait : HostTestFixture
    {
        [SetUp]
        public void TestSetUp()
        {
            Threaded<Session>
                .With<HeadlessChrome>()
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
        public void When_getting_text_of_textfield_using_wait_Should_wait()
        {
            Threaded<Session>
                .CurrentBlock<SlowBlockPage>()
                .CustomerInfoWithExplicitWait
                .FirstName
                .Text
                .Should().Be("Todd");
        }
    }
}