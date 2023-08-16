using System.Net;
using RestSharp;
using Specflow.Sample.Contexts;
using Specflow.Sample.Support;

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
            _domainContext.RestRequest = new RestRequest("https://api.agify.io", Method.Get);
            _domainContext.RestResponse = await _domainContext.RestClient.ExecuteAsync(_domainContext.RestRequest);
        }

        [Then(@"the expected age is returned")]
        public void ThenTheExpectedAgeIsReturned()
        {
            //Build up expected response
            var expectedResponse = new AverageAge()
            {
                Name = _domainContext.Name,
                Age = _domainContext.Age,
                Count = _domainContext.Count
            };

            //Deserialize the response
            var response = _restHelpers.DeserializeJsonResponse<AverageAge>(_domainContext.RestResponse);

            //Compare and assert all properties in the response
            response.Should().BeEquivalentTo(expectedResponse);

            //Assert on individual properties in the response
            response?.Name.Should().Be(_domainContext.Name);
            response?.Age.Should().Be(_domainContext.Age);
        }

        [Then(@"the expected status code ""([^""]*)"" and error message ""([^""]*)"" are returned")]
        public void ThenTheExpectedStatusCodeAndErrorMessageAreReturned(HttpStatusCode statusCode, string errorMessage)
        {
            _domainContext.RestResponse.StatusCode.Should().Be(statusCode);
            var response = _restHelpers.DeserializeJsonResponse<ErrorModel>(_domainContext.RestResponse);
            response?.Error.Should().Be(errorMessage);
        }
    }
}