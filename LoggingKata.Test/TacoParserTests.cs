using System;
using Xunit;

namespace LoggingKata.Test
{
    public class TacoParserTests
    {
        [Fact]
        public void ShouldDoSomething()
        {
            var tacoParser = new TacoParser();
            var actual = tacoParser.Parse("34.073638, -84.677017, Taco Bell Acwort...");
            Assert.NotNull(actual);
        }

        [Theory]
        [InlineData("34.073638,-84.677017, Taco Bell Acwort...", -84.677017)]
        [InlineData("33.635282,-86.684056,Taco Bell Birmingham...", -86.684056)]
        [InlineData("33.59453,-86.694742,Taco Bell Birmingham...", -86.694742)]
        [InlineData("33.49763,-86.875722,Taco Bell Birmingham...", -86.875722)]
        [InlineData("33.470013,-86.816966,Taco Bell Birmingham...", -86.816966)]
        [InlineData("34.201107,-86.151229, Taco Bell Boa...", -86.151229)]
        [InlineData("34.095209,-84.011894, Taco Bell Bufor...", -84.011894)]
        [InlineData("33.145076,-86.749777, Taco Bell Caler...", -86.749777)]
        [InlineData("34.176666,-84.420356, Taco Bell Canto...", -84.420356)]
        [InlineData("34.2223,-84.503673, Taco Bell Canton...", -84.503673)]
        [InlineData("33.587217,-85.057114, Taco Bell Carrollto...", -85.057114)]
        [InlineData("33.544403,-85.073656, Taco Bell Carrollton...", -85.073656)]
        [InlineData("34.170417,-84.782808, Taco Bell Cartersvill...", -84.782808)]
        [InlineData("34.271508,-84.798907, Taco Bell Cartersville...", -84.798907)]
        [InlineData("34.016057,-85.254238, Taco Bell Cedartow...", -85.254238)]
        [InlineData("34.161747,-85.692739, Taco Bell Centr...", -85.692739)]
        [InlineData("34.784434,-84.771556, Taco Bell Chatswort...", -84.771556)]
        [InlineData("34.996237,-85.291147, Taco Bell Chattanooga...", -85.291147)]
        [InlineData("34.888408,-85.267909, Taco Bell Chickamaug...", -85.267909)]
        [InlineData("32.801186,-86.576412, Taco Bell Clanto...", -86.576412)]
        public void ShouldParseLongitude(string line, double expected)
        {
            var longituder = new TacoParser();
            var longual = longituder.Parse(line).Location.Longitude;
            Assert.Equal(expected, longual);
        }

        [Theory]
        [InlineData("34.073638,-84.677017, Taco Bell Acwort...", 34.073638)]
        [InlineData("33.635282,-86.684056,Taco Bell Birmingham...", 33.635282)]
        [InlineData("33.59453,-86.694742,Taco Bell Birmingham...", 33.59453)]
        [InlineData("33.49763,-86.875722,Taco Bell Birmingham...", 33.49763)]
        [InlineData("33.470013,-86.816966,Taco Bell Birmingham...", 33.470013)]
        [InlineData("34.201107,-86.151229, Taco Bell Boa...",34.201107)]
        [InlineData("34.095209,-84.011894, Taco Bell Bufor...", 34.095209)]
        [InlineData("33.145076,-86.749777, Taco Bell Caler...", 33.145076)]
        [InlineData("34.176666,-84.420356, Taco Bell Canto...", 34.176666)]
        [InlineData("34.2223,-84.503673, Taco Bell Canton...", 34.2223)]
        [InlineData("33.587217,-85.057114, Taco Bell Carrollto...", 33.587217)]
        [InlineData("33.544403,-85.073656, Taco Bell Carrollton...", 33.544403)]
        [InlineData("34.170417,-84.782808, Taco Bell Cartersvill...", 34.170417)]
        [InlineData("34.271508,-84.798907, Taco Bell Cartersville...", 34.271508)]
        [InlineData("34.016057,-85.254238, Taco Bell Cedartow...", 34.016057)]
        [InlineData("34.161747,-85.692739, Taco Bell Centr...", 34.161747)]
        [InlineData("34.784434,-84.771556, Taco Bell Chatswort...", 34.784434)]
        [InlineData("34.996237,-85.291147, Taco Bell Chattanooga...", 34.996237)]
        [InlineData("34.888408,-85.267909, Taco Bell Chickamaug...", 34.888408)]
        [InlineData("32.801186,-86.576412, Taco Bell Clanto...", 32.801186)]
        public void ShouldParseLatitude(string line, double expected)
        {
            var latituder = new TacoParser();
            var latual = latituder.Parse(line).Location.Latitude;
            Assert.Equal(expected, latual);
        }

    }
}
