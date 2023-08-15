using System.Text.Json;
using RestSharp;
using Specflow.Sample.Contexts;
using Specflow.Sample.Support;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SpecFlowProject.StepDefinitions
{
    //This class is used to deserialize the response
    public class AverageAge
    {
        public int Count { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    //This is needed to deserialize the error message in the response
    public class ErrorModel
    {
        public string? Error { get; set; }
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
        public void GivenIWantToGetTheAverageAgeOfTheName(string? name)
        {
            _domainContext.Name = name;
        }

        [Given(@"I expect the average to be '([^']*)'")]
        public void GivenIExpectTheAverageToBe(int age)
        {
            _domainContext.Age = age;
        }

        [Given(@"I expect the number of people with this name to be '([^']*)'")]
        public void GivenIExpectTheNumberOfPeopleWithThisNameToBe(int count)
        {
            _domainContext.Count = count;
        }

        [When(@"I request the average age of a persons name")]
        public async Task WhenIRequestTheAverageOfAPersonsName()
        {
            _domainContext.RestClient = new RestClient();
            _domainContext.RestRequest = new RestRequest("https://api.agify.io?name=" + _domainContext.Name, Method.Get);
            _domainContext.RestResponse = await _domainContext.RestClient.ExecuteAsync(_domainContext.RestRequest);
        }

        [When(@"I request the average age of a persons name without specifying a name")]
        public async Task WhenIRequestTheAverageAgeOfAPersonsNameWithoutSpecifyingAName()
        {
            _domainContext.RestClient = new RestClient();
            _domainContext.RestRequest = new RestRequest("https://api.agify.io?" + _domainContext.Name, Method.Get);
            _domainContext.RestResponse = await _domainContext.RestClient.ExecuteAsync(_domainContext.RestRequest);
        }

        [Then(@"the expected age is returned")]
        public void ThenTheExpectedAgeIsReturned()
        {
            //Build up expected response if you want to compare the whole response object
            var expectedResponse = new AverageAge()
            {
                Name = _domainContext.Name,
                Age = _domainContext.Age,
                Count = _domainContext.Count
            };

            //Deserialize the response so it can be asserted on easily in a nice format
            var actualResponse = _restHelpers.DeserializeJsonResponse<AverageAge>(_domainContext.RestResponse);

            //Assert on individual properties in the response
            actualResponse?.Name.Should().Be(_domainContext.Name);
            actualResponse?.Age.Should().Be(_domainContext.Age);

            //Compare and assert all properties in the response
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Then(@"the expected error code and message are returned")]
        public void ThenTheExpectedErrorCodeAndMessageIsReturned()
        {
            var actualResponse = _restHelpers.DeserializeJsonResponse<ErrorModel>(_domainContext.RestResponse);
            //Assert expected error message and status code here
            actualResponse?.Error.Should().Be("Missing 'name' parameter");
        }
    }
}