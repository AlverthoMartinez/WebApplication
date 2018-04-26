using System;
using Xunit;
using Xbehave;

using WebApplication.Site.Libs;

namespace WebApplication.Test
{
    public class TestServiceSpec
    {
        private TestService _testService { get; set; }

        public TestServiceSpec () {
            _testService = new TestService();
        }
        [Fact]
        public void Test1()
        {
            var result = _testService.Test();

            Assert.True(result == "Hello World");

        }

        [Scenario]
        public void Addicion(int x, int y, TestService testService, int answer) {
            "Given the number 1"
                .x(() => x = 1);

            "And the number 2"
                .x(() => y = 2);

            "And the service with Add method"
                .x(() => testService = new TestService());

            "When I add the numbers together"
                .x(() => answer = testService.Add(x, y));

            "Then the answer is 3"
                .x(() => Xunit.Assert.Equal(3, answer));
        }
    }
}
