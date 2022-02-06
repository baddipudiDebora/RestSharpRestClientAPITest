using System;
using System.Configuration;
using TechTalk.SpecFlow;
using RestAPIBase.Reporting;

namespace RestSharpAPITest
{
    [Binding]
    public sealed class SpecFlowHooks
    {
        [AfterFeature]
        public static void AfterFeature()
        {
            ExtentReportsHandler.FlushReport();
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            bool reportingEnabled;
            bool reportingCreateOneReportFileForEachPerFeatureFile;

            try
            {
                //reportingEnabled = ConfigurationManager.AppSettings["Reporting_Enabled"].ToLower() == "true";
                //reportingCreateOneReportFileForEachPerFeatureFile = ConfigurationManager.AppSettings["Reporting_CreateOneReportFileForEachFeatureFile"].ToLower() == "true";
                reportingEnabled = true;
                reportingCreateOneReportFileForEachPerFeatureFile = true;
            }
            catch (Exception)
            {
                reportingEnabled = false;
                reportingCreateOneReportFileForEachPerFeatureFile = false;
            }

            ExtentReportsHandler.ReportingEnabled = reportingEnabled;
            ExtentReportsHandler.CreateOneReportFileForEachFeatureFile = reportingCreateOneReportFileForEachPerFeatureFile;
            ExtentReportsHandler.InitialiseReport();
        }

        [AfterScenario]
        public static void AfterScenario()
        {
        }

        [BeforeScenario]
        public static void BeforeScenario()
        {
            ExtentReportsHandler.CreateTest();
        }
    }
}