using System.Text.Json;
using RestSharp;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SpecFlowProject.StepDefinitions
{
    public class AverageAge
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [Binding]
    public sealed class AverageAgeSteps
    {
        [When(@"I request the average age of a persons name")]
        public async Task WhenIRequestTheAverageOfAPersonsName()
        {
            //Given
            var client = new RestClient();
            var request = new RestRequest("https://api.agify.io?name=Dave", Method.Get);

            //When
            var response = await client.GetAsync(request);

            //Then
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseJson = JsonSerializer.Deserialize<AverageAge>(response.Content ?? "", options);

            responseJson?.Name.Should().Be("Dave");
            responseJson?.Age.Should().Be(67);
        }
    }
}