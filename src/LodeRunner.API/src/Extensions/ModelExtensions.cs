// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using LodeRunner.Core.CommandLine;
using LodeRunner.Core.Models;

namespace LodeRunner.API.Middleware
{
    /// <summary>
    /// Provides Extension methods for Models.
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Validates flag values and combinations in load test configuration payload.
        /// </summary>
        /// <param name="loadTestConfig">The load test configuration.</param>
        /// <param name="payloadPropertiesChanged">Payload Properties Change list.</param>
        /// <returns>Errors when DTO doesn't pass validation.</returns>
        public static IEnumerable<string> FlagValidator(this LoadTestConfig loadTestConfig, List<string> payloadPropertiesChanged = null)
        {
            RootCommand root = LRCommandLine.BuildRootCommandMode();
            string[] args = GetArgs(loadTestConfig, payloadPropertiesChanged);
            return root.Parse(args).Errors.Select(x => x.Message);
        }

        /// <summary>
        /// Gets the arguments from properties that exist in changedProperties list.
        /// </summary>
        /// <param name="loadTestConfig">The load test configuration.</param>
        /// <param name="payloadPropertiesChanged">Changed Properties list.</param>
        /// <returns>the args.</returns>
        private static string[] GetArgs(LoadTestConfig loadTestConfig, List<string> payloadPropertiesChanged = null)
        {
            var properties = loadTestConfig.GetType().GetProperties().Where(prop => prop.IsDefined(typeof(DescriptionAttribute), false));

            List<string> argsList = new ();

            foreach (var prop in properties)
            {
                // NOTE: Only convert properties to arguments if the property exist is ChangedProperties list. This list represents the Json data sent as Payload
                if (payloadPropertiesChanged == null || payloadPropertiesChanged.Contains(prop.Name))
                {
                    var descriptionAttributes = (DescriptionAttribute[])prop.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (descriptionAttributes.Length > 0)
                    {
                        argsList.Add(descriptionAttributes[0].Description);
                        argsList.Add(loadTestConfig.FieldValue(prop.Name));
                    }
                }
            }

            return argsList.ToArray();
        }

        /// <summary>
        /// Fields the value.
        /// </summary>
        /// <param name="loadTestConfigDto">The load test configuration.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The value.</returns>
        private static string FieldValue(this LoadTestConfig loadTestConfigDto, string fieldName)
        {
            string result = string.Empty;

            Type objType = loadTestConfigDto.GetType();

            PropertyInfo[] props = objType.GetProperties();

            PropertyInfo propFound = props.FirstOrDefault(x => x.Name == fieldName);

            if (propFound != null)
            {
                if (propFound.PropertyType == typeof(List<string>))
                {
                    List<string> items = (List<string>)propFound.GetValue(loadTestConfigDto);

                    result = string.Join(" ", items);
                }
                else
                {
                    object propValue = propFound.GetValue(loadTestConfigDto);
                    result = propValue == null ? string.Empty : propValue.ToString();
                }
            }

            return result;
        }
    }
}
