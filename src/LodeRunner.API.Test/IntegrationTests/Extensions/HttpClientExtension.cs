﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LodeRunner.API.Models;
using LodeRunner.Core.Models;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace LodeRunner.API.Test.IntegrationTests
{
    /// <summary>
    /// Http Client Extensions.
    /// </summary>
    public static class HttpClientExtension
    {
        /// <summary>
        /// Waits for GetClients Response to match ClientStatus for the given ClientStatusId.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="clientsUri">The clients URI.</param>
        /// <param name="clientStatusId">The client status identifier.</param>
        /// <param name="clientStatusType">Type of the client status.</param>
        /// <param name="jsonOptions">The json options.</param>
        /// <param name="output">The output.</param>
        /// <param name="maxRetries">Maximum retries.</param>
        /// <param name="timeBetweenRequestsMs">Wait time betweeen requests.</param>
        /// <returns>HttpStatusCode and Client from response.</returns>
        public static (HttpStatusCode, Client) GetClientByIdRetries(this HttpClient httpClient, string clientsUri, string clientStatusId, ClientStatusType clientStatusType, JsonSerializerOptions jsonOptions, ITestOutputHelper output, int maxRetries = 10, int timeBetweenRequestsMs = 100)
        {
            int attempts = 0;
            HttpResponseMessage httpResponse;
            Client client = null;

            do
            {
                attempts++;
                httpResponse = httpClient.GetAsync($"{clientsUri}/{clientStatusId}").Result;

                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: GET Client by ID\tResponse StatusCode: 'NotFound'\tClientStatusId: '{clientStatusId}'\tAttempts: {attempts} [{timeBetweenRequestsMs}ms between requests]");
                    Thread.Sleep(timeBetweenRequestsMs);
                }
                else if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    client = httpResponse.Content.ReadFromJsonAsync<Client>(jsonOptions).Result;

                    if (client.Status == clientStatusType)
                    {
                        output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: GET Client by ID\tResponse StatusCode: 'OK'\tClientStatusId: '{clientStatusId}'\tAttempts: {attempts} [{timeBetweenRequestsMs}ms between requests]\tStatusType criteria met [{client.Status}]");
                        break;
                    }
                    else
                    {
                        output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: GET Client by ID\tResponse StatusCode: 'OK'\tClientStatusId: '{clientStatusId}'\tAttempts: {attempts} [{timeBetweenRequestsMs}ms between requests]\tStatusType criteria not met [expected: {clientStatusType}, received: {client.Status}]");
                        Thread.Sleep(timeBetweenRequestsMs);
                    }
                }
                else
                {
                    output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: GET Client by ID\tUnhandled Response StatusCode: '{httpResponse.StatusCode}'\tClientStatusId: '{clientStatusId}'\tAttempts: {attempts} [{timeBetweenRequestsMs}ms between requests]");
                    break;
                }
            }
            while (attempts <= maxRetries);

            return (httpResponse.StatusCode, client);
        }

        /// <summary>
        /// Get all TestRun.
        /// </summary>
        /// <param name="httpClient">The httpClient.</param>
        /// <param name="testRunsUri">The TestRun Uri.</param>
        /// <param name="output">The output.</param>
        /// <returns>the task.</returns>
        /// <summary>
        /// <returns><see cref="Task"/> representing the asynchronous unit test.</returns>
        public static async Task<HttpResponseMessage> GetTestRuns(this HttpClient httpClient, string testRunsUri, ITestOutputHelper output)
        {
            HttpResponseMessage httpResponse = await httpClient.GetAsync(testRunsUri);

            if (httpResponse.StatusCode == HttpStatusCode.OK || httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: GET all TestRun\tResponse StatusCode: '{httpResponse.StatusCode}'");
            }
            else
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: GET all TestRun\tUNEXPECTED Response StatusCode: '{httpResponse.StatusCode}'");
            }

            return httpResponse;
        }

        /// <summary>
        /// Get TestRun by ID.
        /// </summary>
        /// <param name="httpClient">The httpClient.</param>
        /// <param name="testRunsUri">The TestRun Uri.</param>
        /// <param name="testRunId">The TestRun ID.</param>
        /// <param name="output">The output.</param>
        /// <returns>the task.</returns>
        public static async Task<HttpResponseMessage> GetTestRunById(this HttpClient httpClient, string testRunsUri, string testRunId, ITestOutputHelper output)
        {
            var httpResponse = await httpClient.GetAsync(testRunsUri + "/" + testRunId);

            if (httpResponse.StatusCode == HttpStatusCode.OK || httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: GET TestRun by ID\tResponse StatusCode: '{httpResponse.StatusCode}'\tTestRunId: '{testRunId}'");
            }
            else
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: GET TestRun\tUNEXPECTED Response StatusCode: '{httpResponse.StatusCode}'\tTestRunId: '{testRunId}'");
            }

            return httpResponse;
        }

        /// <summary>
        /// Post TestRuns.
        /// </summary>
        /// <param name="httpClient">The httpClient.</param>
        /// <param name="postTestRunsUri">The post TestRun Uri.</param>
        /// <param name="output">The output.</param>
        /// <returns>the task.</returns>
        public static async Task<HttpResponseMessage> PostTestRun(this HttpClient httpClient, string postTestRunsUri, ITestOutputHelper output)
        {
            TestRunPayload testRunPayload = new ();
            testRunPayload.SetMockData($"Sample TestRun - IntegrationTesting-{nameof(PostTestRun)}-{DateTime.UtcNow:yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK}");

            string jsonTestRun = JsonConvert.SerializeObject(testRunPayload);
            StringContent stringContent = new (jsonTestRun, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(postTestRunsUri, stringContent);

            if (httpResponse.StatusCode == HttpStatusCode.Created)
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: POST TestRun\tResponse StatusCode: '{httpResponse.StatusCode}'");
            }
            else
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: POST TestRun\tUNEXPECTED Response StatusCode: '{httpResponse.StatusCode}'");
            }

            return httpResponse;
        }

        /// <summary>
        /// Put Test Run by ID.
        /// </summary>
        /// <param name="httpClient">the httpClient.</param>
        /// <param name="testRunsUri">The TestRun Uri.</param>
        /// <param name="testRunId">The test run ID.</param>
        /// <param name="testRunPayload">the testRunPayload entity.</param>
        /// <param name="output">The output.</param>
        /// <returns>the task.</returns>
        public static async Task<HttpResponseMessage> PutTestRunById(this HttpClient httpClient, string testRunsUri, string testRunId, TestRunPayload testRunPayload, ITestOutputHelper output)
        {
            StringContent stringContent = new (JsonConvert.SerializeObject(testRunPayload), Encoding.UTF8, "application/json");

            // Send Request
            var httpResponse = await httpClient.PutAsync($"{testRunsUri}/{testRunId}", stringContent);

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: PUT TestRun\tResponse StatusCode: '{httpResponse.StatusCode}'\tTestRunId: '{testRunId}'");
            }
            else
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: PUT TestRun\tUNEXPECTED Response StatusCode: '{httpResponse.StatusCode}'\tTestRunId: '{testRunId}'");
            }

            return httpResponse;
        }

        /// <summary>
        /// Delete a Test Run by Id.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="testRunsUri">The base TestRun Uri.</param>
        /// <param name="testRunId">The testRun ID.</param>
        /// <param name="output">The output.</param>
        /// <returns>the successful task value.</returns>
        public static async Task<HttpResponseMessage> DeleteTestRunById(this HttpClient httpClient, string testRunsUri, string testRunId, ITestOutputHelper output)
        {
            var httpResponse = await httpClient.DeleteAsync($"{testRunsUri}/{testRunId}");

            if (httpResponse.StatusCode == HttpStatusCode.NoContent || httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: DELETE TestRun\tResponse StatusCode: '{httpResponse.StatusCode}'\tTestRunId: '{testRunId}'");
            }
            else
            {
                output.WriteLine($"UTC Time:{DateTime.UtcNow}\tAction: DELETE TestRun\tUNEXPECTED Response StatusCode: '{httpResponse.StatusCode}'\tTestRunId: '{testRunId}'");
            }

            return httpResponse;
        }
    }
}