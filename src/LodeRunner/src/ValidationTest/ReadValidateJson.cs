﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using LodeRunner.Core;
using LodeRunner.Model;
using LodeRunner.Validators;
using Microsoft.Extensions.Logging;

namespace LodeRunner
{
    /// <summary>
    /// Test class (partial).
    /// </summary>
    public partial class ValidationTest
    {
        /// <summary>
        /// Reads a file from local or --base-url.
        /// </summary>
        /// <param name="file">file name.</param>
        /// <returns>file contents.</returns>
        public string ReadTestFile(string file)
        {
            string content = string.Empty;

            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (string.IsNullOrWhiteSpace(this.config.BaseUrl))
            {
                // check for file exists
                if (string.IsNullOrWhiteSpace(file) || !File.Exists(file))
                {
                    Console.WriteLine($"File Not Found: {file}");

                    return null;
                }

                // read the file
                content = File.ReadAllText(file);

                // check for empty file
                if (string.IsNullOrWhiteSpace(content))
                {
                    Console.WriteLine($"Unable to read file {file}");
                    return null;
                }
            }
            else
            {
                string path = this.config.BaseUrl + file;

                using HttpClient client = new ();

                try
                {
                    content = client.GetStringAsync(new Uri(path)).Result;

                    // check for empty file
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.WriteLine($"Unable to read file {path}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    // display helper message on request exception
                    if (ex.InnerException is HttpRequestException hre)
                    {
                        Console.WriteLine("Verify you have permission to read the URL as well as the correctness of the URL");
                    }

                    throw;
                }
            }

            return content;
        }

        /// <summary>
        /// Read a json test file.
        /// </summary>
        /// <param name="file">file path.</param>
        /// <returns>List of Request.</returns>
        public List<Request> ReadJson(string file)
        {
            string json = this.ReadTestFile(file);

            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine($"Invalid json in file: {file}");
                return null;
            }

            return this.LoadJson(json);
        }

        /// <summary>
        /// Validate all of the requests.
        /// </summary>
        /// <param name="requests">list of Request.</param>
        /// <returns>boolean.</returns>
        private static bool ValidateJson(List<Request> requests)
        {
            // validate each request
            foreach (Request r in requests)
            {
                ValidationResult result = ParameterValidator.Validate(r);
                if (result.Failed)
                {
                    Console.WriteLine($"Error: Invalid json\n\t{JsonSerializer.Serialize(r)}\n\t{string.Join("\n", result.ValidationErrors)}");
                    return false;
                }
            }

            // validated
            return true;
        }

        /// <summary>
        /// Load the requests from json files.
        /// </summary>
        /// <param name="fileList">list of files to load.</param>
        /// <returns>sorted List or Requests.</returns>
        private List<Request> LoadValidateRequests(List<string> fileList)
        {
            List<Request> list;
            List<Request> fullList = new ();

            // read each json file
            foreach (string inputFile in fileList)
            {
                list = this.ReadJson(inputFile);

                // add contents to full list
                if (list != null && list.Count > 0)
                {
                    fullList.AddRange(list);
                }
            }

            // return null if can't read and validate the json files
            if (fullList == null || fullList.Count == 0 || !ValidateJson(fullList))
            {
                return null;
            }

            // return sorted list
            return fullList;
        }

        /// <summary>
        /// Load performance targets from json.
        /// </summary>
        /// <returns>Dictionary of PerfTarget.</returns>
        private Dictionary<string, PerfTarget> LoadPerfTargets()
        {
            const string perfFileName = "perfTargets.txt";

            string content = this.ReadTestFile(perfFileName);

            if (!string.IsNullOrWhiteSpace(content))
            {
                return JsonSerializer.Deserialize<Dictionary<string, PerfTarget>>(content, App.JsonSerializerOptions);
            }

            // return empty dictionary - perf targets are not required
            return new Dictionary<string, PerfTarget>();
        }

        /// <summary>
        /// Load the json string into a List of Requests.
        /// </summary>
        /// <param name="json">json string.</param>
        /// <returns>List of Request or null.</returns>
        private List<Request> LoadJson(string json)
        {
            try
            {
                List<Request> list = null;
                InputJson data = null;
                List<Request> l2 = new ();

                try
                {
                    // try to parse the json
                    data = JsonSerializer.Deserialize<InputJson>(json, App.JsonSerializerOptions);
                }
                catch
                {
                    // try to read the array of Requests style document
                    // this is being deprecated in v1.4
                    list = JsonSerializer.Deserialize<List<Request>>(json, App.JsonSerializerOptions);
                }

                // replace placedholders with environment variables
                if (data != null && data.Requests.Count > 0)
                {
                    if (data.Variables != null && data.Variables.Count > 0)
                    {
                        foreach (string v in data.Variables)
                        {
                            json = json.Replace("${" + v + "}", System.Environment.GetEnvironmentVariable(v), StringComparison.Ordinal);
                        }

                        // reload from json
                        data = JsonSerializer.Deserialize<InputJson>(json, App.JsonSerializerOptions);
                    }

                    list = data.Requests;
                }

                if (list != null && list.Count > 0)
                {
                    foreach (Request r in list)
                    {
                        // Add the default perf targets if exists
                        if (r.PerfTarget != null && r.PerfTarget.Quartiles == null)
                        {
                            if (this.targets.ContainsKey(r.PerfTarget.Category))
                            {
                                r.PerfTarget.Quartiles = this.targets[r.PerfTarget.Category].Quartiles;
                            }
                        }

                        l2.Add(r);
                    }

                    // success
                    return l2;
                }

                Console.WriteLine("Invalid JSON file");
            }
            catch (Exception ex)
            {
                logger.LogError(new EventId((int)LogLevel.Error, nameof(LoadJson)), ex, SystemConstants.Exception);
            }

            // couldn't read the list
            return null;
        }
    }
}
