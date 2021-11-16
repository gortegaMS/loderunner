﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LodeRunner.Core.Models;

namespace LodeRunner.Data.Interfaces
{
    /// <summary>
    /// LoadTestConfig Interface.
    /// </summary>
    public interface ILoadTestConfigService
    {
        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The corresponding Enity.</returns>
        Task<LoadTestConfig> Get(string id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>all items for a given type.</returns>
        Task<IEnumerable<LoadTestConfig>> GetAll();

        /// <summary>
        /// Posts the specified load test configuration.
        /// </summary>
        /// <param name="loadTestConfig">The load test configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The Inserted LoadTestConfig entity.</returns>
        Task<LoadTestConfig> Post(LoadTestConfig loadTestConfig, CancellationToken cancellationToken);
    }
}
