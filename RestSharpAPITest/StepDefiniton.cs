using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace RestSharpAPITest
{[Binding]
    public sealed class StepDefiniton
    {
        readonly RestfulAPIRequestHandler _requestHandler = new RestfulAPIRequestHandler();

        [Given(@"I generate the request with the verb '([^']*)' for the enpoint called '([^']*)'")]
        public void GivenIGenerateTheRequestWithTheVerbForTheEnpointCalledS(string verb, string endpointName)
        {
            _requestHandler.SetupRestFulRequetInformation(verb, endpointName);
        }

        [Given(@"I setup the query paramaters")]
        public void GivenISetupThheQueryParamaters(Table table)
        {
            _requestHandler.SetUpQueryParameters(table);
        }

        [When(@"I send the API Request")]
        public void WhenWhenISendTheAPIRequest()
        {
            _requestHandler.SendRequestMessage();
        }

        [Then(@"I validate the Response is '([^']*)'")]
        public void ThenIValidateTheResponseIs(string expectedStatusCode)
        {
            _requestHandler.ValidateResponceStatusCode(expectedStatusCode);
        }

        //[Then(@"I verify the fields in the response")]
        //public void ThenIVerifyTheFieldsInTheResponse()
        //{
        //    _requestHandler.ValidateResponseMessage(table);
        //}


    }
}
