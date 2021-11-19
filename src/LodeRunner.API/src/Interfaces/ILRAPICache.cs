﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using LodeRunner.API.Middleware;
using LodeRunner.API.Models;
using LodeRunner.Core.Interfaces;
using LodeRunner.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace LodeRunner.API.Interfaces
{
    /// <summary>
    /// Data Access Layer for Cache Interface.
    /// </summary>
    public interface ILRAPICache : IAppCache
    {
        /// <summary>
        /// Processes client status item from changefeed.
        /// </summary>
        /// <param name="doc">Changefeed item.</param>
        void ProcessClientStatusChange(Document doc);

        /// <summary>
        /// Handles cache results.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="results">Results from the cache.</param>
        /// <returns>Action result.</returns>
        Task<ActionResult> HandleCacheResult<TEntity>(TEntity results);

        /// <summary>
        /// Handles the cache result.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="results">The results.</param>
        /// <returns>Action result.</returns>
        Task<ActionResult> HandleCacheResult<TEntity>(IEnumerable<TEntity> results);

        /// <summary>
        /// Gets clients from cache.
        /// </summary>
        /// <returns>Enumerable of clients.</returns>
        Task<ActionResult> GetClients();

        /// <summary>
        /// Gets client by clientStatusId from cache.
        /// </summary>
        /// <param name="clientStatusId">Client status ID.</param>
        /// <returns>Client.</returns>
        Task<ActionResult> GetClientByClientStatusId(string clientStatusId);
    }
}
