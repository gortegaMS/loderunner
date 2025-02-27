// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using LodeRunner.API.Middleware.Validation;

namespace LodeRunner.API.Middleware
{
    /// <summary>
    /// Validates string parameters for the given TEntity used from different controllers.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class ParametersValidator<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Validate Id.
        /// </summary>
        /// <param name="entityId">id to validate.</param>
        /// <returns>List of error messages.</returns>
        public static List<string> ValidateEntityId(string entityId)
        {
            List<string> errors = new ();

            string entityIdFieldName = $"{typeof(TEntity).Name}Id";

            if (!Guid.TryParse(entityId, out _))
            {
                errors.Add($"{entityIdFieldName} - {ValidationError.GetErrorMessage(entityIdFieldName)}");
            }

            return errors;
        }
    }
}
