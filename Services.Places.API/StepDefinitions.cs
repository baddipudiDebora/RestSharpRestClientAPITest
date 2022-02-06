using FluentAssertions;
using TechTalk.SpecFlow;

namespace RestSharpAPITest
{
    [Binding]
    public sealed class StepDefinitions
    {
        readonly RESTfulAPIRequestHandler _requestHandler = new RESTfulAPIRequestHandler();

        #region SetupMethods
        [Given(@"I generate a '(.*)' verb request for an action called '(.*)'")]
        public void GivenIGenerateAVerbRequestForAnActionCalled(string verb, string actionMethod)
        {
            _requestHandler.SetupRESTfulRequestInformation(verb, actionMethod);
        }
        [Given(@"I setup the following query parameters")]
        public void GivenISetupTheFollowingQueryParameters(Table table)
        {
            _requestHandler.SetupQueryParameters(table);
        }
        [Given(@"I setup the '(.*)' json file with the following parameters")]
        public void GivenISetupTheJsonFileWithTheFollowingParameters(string jsonFileName, Table table)
        {
            _requestHandler.SetupJsonFile(jsonFileName, table);
        }
        [When(@"I send the generated request")]
        public void WhenISendTheGeneratedRequest()
        {
            _requestHandler.SendRequestMessage();
        }
        [Then(@"I validate the Response Status Code is '(.*)'")]
        public void ThenIValidateTheResponseStatusCodeIs(int statusCode)
        {
            _requestHandler.ValidateResponseStatusCode(statusCode);
        }
        [Then(@"I validate the Response contains (.*) records")]
        public void ThenIValidateTheResponseContainsRecords(int numRecords)
        {
            int count = _requestHandler.jsonResultObj["count"];
            count.Should().Be(numRecords);
        }

        [Then(@"I store the '(.*)' value")]
        public void ThenIStoreTheValue(string name)
        {
            _requestHandler.StoreFeatureContextVariable(name);
        }

        [Then(@"I validate the '(.*)' value in the Response is '(.*)'")]
        public void ThenIValidateTheValueInResponseIs(string propertyName, string expectedValue)
        {
            expectedValue = string.IsNullOrEmpty(expectedValue) ? null : expectedValue;
            string value = _requestHandler.jsonResultObj[propertyName];
            value.Should().Be(expectedValue);
        }
        [Then(@"I validate the following response is generated")]
        public void ThenIValidateTheFollowingResponseIsGenerated(Table table)
        {
            _requestHandler.ValidateResponseMessage(table);
        }

        #endregion
    }
}
