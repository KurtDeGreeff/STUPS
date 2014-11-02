﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 7/13/2014
 * Time: 2:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Tmx.Server.Modules
{
    using System;
	using System.Text;
    using System.Linq;
	using System.Xml.Linq;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Responses.Negotiation;
    using Tmx.Core;
	using Tmx.Interfaces;
	using Tmx.Interfaces.Server;
	using Tmx;
	using Tmx.Interfaces.TestStructure;
    
    /// <summary>
    /// Description of TestResultsModule.
    /// </summary>
    public class TestResultsModule : NancyModule
    {
        public TestResultsModule() : base(UrnList.TestResults_Root)
        {
            Post[UrnList.TestResultsPostingPoint_relPath] = parameters => importTestResultsToTestRun(parameters.id);
            
            Get[UrnList.TestResultsPostingPoint_relPath] = parameters => exportTestResultsFromTestRun(parameters.id);
            
            // 20141031
            // postponed
//            Post[UrnList.TestStructure_Suites] = _ => {
//                var testSuite = this.Bind<TestSuite>();
//                TmxHelper.NewTestSuite(testSuite.Name, testSuite.Id, testSuite.PlatformId, testSuite.Description, testSuite.BeforeScenario, testSuite.AfterScenario);
//                TestData.SetSuiteStatus(true);
//				return TmxHelper.OpenTestSuite(testSuite.Name, testSuite.Id, testSuite.PlatformId) ? HttpStatusCode.Created : HttpStatusCode.InternalServerError;
//            };
//        	
//        	Post[UrnList.TestStructure_Scenarios] = _ => {
//                // var testScenario = this.Bind<TestScenario>();
//                var testScenario = this.Bind<TestScenario>("DbId", "TestResults", "Timestamp", "BeforeTest", "AfterTest", "BeforeTestParameters", "AfterTestParameters", "TestCases", "TimeSpent", "Statistics", "enStatus");
//                
//        		var dataObjectAdd = new AddScenarioCmdletBaseDataObject {
//					AfterTest = testScenario.AfterTest,
//					BeforeTest = testScenario.BeforeTest,
//					Description = testScenario.Description,
//					Id = testScenario.Id,
//        			Name = testScenario.Name,
//        			TestPlatformId = testScenario.PlatformId,
//        			TestSuiteId = testScenario.SuiteId
//        		};
//        		TmxHelper.AddTestScenario(dataObjectAdd);
//        		TestData.SetScenarioStatus(true);
//        		
//        		// TODO: investigate into the code below
////        		var dataObjectOpen = new OpenScenarioCmdletBaseDataObject {
////        			Name = testScenario.Name,
////        			Id = testScenario.Id,
////        			TestPlatformId = testScenario.PlatformId
////        		};
////        		return TmxHelper.OpenTestScenario(dataObjectOpen) ? HttpStatusCode.Created : HttpStatusCode.InternalServerError;
//        		
//        		return HttpStatusCode.Created;
//        	};
//            
//        	Post[UrnList.TestStructure_Results] = _ => {
//ITestResult testResult = null;
//try {
//	testResult = this.Bind<TestResult>(); // "DbId", "TestResults", "Timestamp", "BeforeTest", "AfterTest", "BeforeTestParameters", "AfterTestParameters", "TestCases", "TimeSpent", "Statistics", "enStatus");
//        		
//}
//catch (Exception eeee) {
//	Console.WriteLine(eeee.Message);
//}
//
////        		var dataObjectAdd = new AddScenarioCmdletBaseDataObject {
////					AfterTest = testResult.AfterTest,
////					BeforeTest = testResult.BeforeTest,
////					Description = testResult.Description,
////					Id = testResult.Id,
////        			Name = testResult.Name,
////        			TestPlatformId = testResult.PlatformId,
////        			TestSuiteId = testResult.SuiteId
////        		};
////        		TmxHelper.AddTestScenario(dataObjectAdd);
////        		TestData.SetScenarioStatus(true);
////        		
////        		var dataObjectOpen = new OpenScenarioCmdletBaseDataObject {
////        			Name = testResult.Name,
////        			Id = testResult.Id,
////        			TestPlatformId = testResult.PlatformId
////        		};
////        		return TmxHelper.OpenTestScenario(dataObjectOpen) ? HttpStatusCode.Created : HttpStatusCode.InternalServerError;
////        		
////var result = TmxHelper.OpenTestScenario(dataObjectOpen);
////Console.WriteLine(result);
//        		return HttpStatusCode.Created;
//        	};
        }

        HttpStatusCode importTestResultsToTestRun(Guid testRunId)
        // HttpStatusCode importTestResults()
        {
// 20141101 temp
Console.WriteLine(testRunId);
            try {
                var actualBytes = new byte[Request.Body.Length];
                Request.Body.Read(actualBytes, 0, (int)Request.Body.Length);
                var actual = Encoding.UTF8.GetString(actualBytes);
                var xDoc = XDocument.Parse(actual);
// 20141101 temp
//Console.WriteLine("======================== IMPORTED ========================");
//Console.WriteLine(xDoc);
// Console.WriteLine(xDoc.Root);
                // 20141031
                // TmxHelper.ImportTestResultsFromXdocumentAndStore(xDoc);
                var currentTestRun = TestRunQueue.TestRuns.First(testRun => testRun.Id == testRunId);
                var testResultsImporter = new TestResultsImportExport();
                currentTestRun.TestSuites.AddRange(testResultsImporter.ImportTestResultsFromXdocument(xDoc));
//Console.WriteLine("======================== IMPORTED ========================");
//foreach (var suite in currentTestRun.TestSuites) {
//    Console.WriteLine("id = {0}, name = {1}, status = {2}", suite.Id, suite.Name, suite.Status);
//    foreach (var scenario in suite.TestScenarios) {
//        Console.WriteLine("\tid = {0}, name = {1}, status = {2}", scenario.Id, scenario.Name, scenario.Status);
//        foreach (var result in scenario.TestResults) {
//            Console.WriteLine("\t\tid = {0}, name = {1}, status = {2}", result.Id, result.Name, result.Status);
//        }
//    }
//}
                
                // maybe, there's no such need? // TODO: set current test suite, test scenario, test result?
                return HttpStatusCode.Created;
            } catch (Exception) {
                return HttpStatusCode.ExpectationFailed;
            }
        }
        
        // Negotiator exportTestResults()
        Negotiator exportTestResultsFromTestRun(Guid testRunId)
        {
            // temporary!
            // 20141101
            // TestData.TestSuites = TestRunQueue.TestRuns.First(testRun => testRun.Id == testRunId).TestSuites;
            
// 20141101 temp
//Console.WriteLine("0000000000000000000000000000002");
// Console.WriteLine(TestData.TestSuites.Count);
//Console.WriteLine(TestRunQueue.TestRuns.FirstOrDefault(testRun => testRun.Id == testRunId).TestSuites.Count);
//Console.WriteLine("======================== TO EXPORT ========================");
// foreach (var suite in TestData.TestSuites) {
//foreach (var suite in TestRunQueue.TestRuns.FirstOrDefault(testRun => testRun.Id == testRunId).TestSuites) {
//    Console.WriteLine("id = {0}, name = {1}, status = {2}", suite.Id, suite.Name, suite.Status);
//    foreach (var scenario in suite.TestScenarios) {
//        Console.WriteLine("\tid = {0}, name = {1}, status = {2}", scenario.Id, scenario.Name, scenario.Status);
//        foreach (var result in scenario.TestResults) {
//            Console.WriteLine("\t\tid = {0}, name = {1}, status = {2}", result.Id, result.Name, result.Status);
//        }
//    }
//}
//            var xDoc = TmxHelper.GetTestResultsAsXdocument(new SearchCmdletBaseDataObject {
//                                                               Descending = false,
//                                                               FilterAll = true,
//                                                               FilterFailed = false,
//                                                               OrderById = true
//                                                           });
            var testResultsExporter = new TestResultsImportExport();
            var xDoc = testResultsExporter.GetTestResultsAsXdocument(
                           new SearchCmdletBaseDataObject {
                    Descending = false,
                    FilterAll = true,
                    FilterFailed = false,
                    OrderById = true
                },
                           TestRunQueue.TestRuns.FirstOrDefault(testRun => testRun.Id == testRunId).TestSuites);
// 20141101 temp
//Console.WriteLine("0000000000000000000000000000003");
//if (null == xDoc) {
//    Console.WriteLine("null == xDoc");
//} else {
//    Console.WriteLine("null != xDoc");
//}
//Console.WriteLine("======================== EXPORTED ========================");
//Console.WriteLine(xDoc);
//Console.WriteLine(xDoc.Root);
            return null == xDoc ? Negotiate.WithStatusCode(HttpStatusCode.NotFound) : Negotiate.WithModel(xDoc).WithStatusCode(HttpStatusCode.OK);
        }
    }
}
