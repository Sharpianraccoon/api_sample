using System.Text.Json;
using RestSharp;
using Specflow.Sample.Contexts;
using Specflow.Sample.Support;
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
        private readonly DomainContext _domainContext;
        private readonly RestHelpers _restHelpers;

        public AverageAgeSteps(
            DomainContext domainContext,
            RestHelpers restHelpers)
        {
            _domainContext = domainContext;
            _restHelpers = restHelpers;
        }

        [Given(@"I want to get the average age of the name '([^']*)'")]
        public void GivenIWantToGetTheAverageAgeOfTheName(string name)
        {
            _domainContext.Name = name;
        }

        [Given(@"I expect the average to be '([^']*)'")]
        public void GivenIExpectTheAverageToBe(int age)
        {
            _domainContext.Age = age;
        }

        [When(@"I request the average age of a persons name")]
        public async Task WhenIRequestTheAverageOfAPersonsName()
        {
            _domainContext.RestClient = new RestClient();
            _domainContext.RestRequest = new RestRequest("https://api.agify.io?name=" + _domainContext.Name, Method.Get);
            _domainContext.RestResponse = await _domainContext.RestClient.GetAsync(_domainContext.RestRequest);
        }

        [Then(@"the expected age is returned")]
        public void ThenTheExpectedAgeIsReturned()
        {
            var responseJson = _restHelpers.DeserializeJsonResponse<AverageAge>(_domainContext.RestResponse);

            responseJson?.Name.Should().Be(_domainContext.Name);
            responseJson?.Age.Should().Be(_domainContext.Age);
        }
    }
}