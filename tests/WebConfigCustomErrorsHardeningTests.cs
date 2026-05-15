using Xunit;

namespace WebGoat.Tests
{
    public class WebConfigCustomErrorsHardeningTests
    {
        [Fact]
        public void WebConfigDiff_IndicatesCustomErrorsRemoteOnly()
        {
            const string diff = "<customErrors mode=\"RemoteOnly\" />";
            Assert.Contains("mode=\"RemoteOnly\"", diff);
        }
    }
}
