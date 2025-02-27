// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Reflection;
using LodeRunner.API;
using Microsoft.AspNetCore.Builder;

namespace LodeRunner.API.Middleware
{
    /// <summary>
    /// Registers aspnet middleware handler that handles /version.
    /// </summary>
    public static class VersionExtension
    {
        // cached values
        private static byte[] responseBytes;
        private static string name = string.Empty;

        /// <summary>
        /// Gets the app version.
        /// </summary>
        public static string Version => LodeRunner.Core.Version.AssemblyVersion;

        /// <summary>
        /// Gets the app name.
        /// </summary>
        public static string Name => name;

        /// <summary>
        /// Cache version and application name values with reflection.
        /// </summary>
        public static void Init()
        {
            // cache the application name
            if (Attribute.GetCustomAttribute(Assembly.GetEntryAssembly(), typeof(AssemblyTitleAttribute)) is AssemblyTitleAttribute n)
            {
                name = n.Title;
            }
        }

        /// <summary>
        /// Middleware extension method to handle /version request.
        /// </summary>
        /// <param name="builder">this IApplicationBuilder.</param>
        /// <returns>IApplicationBuilder.</returns>
        public static IApplicationBuilder UseVersion(this IApplicationBuilder builder)
        {
            Init();

            responseBytes = System.Text.Encoding.UTF8.GetBytes(Version);

            // implement the middleware
            builder.Use(async (context, next) =>
            {
                string path = "/version";

                // matches /version
                if (context.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase))
                {
                    // return the version info
                    context.Response.ContentType = "text/plain";

                    await context.Response.Body.WriteAsync(responseBytes).ConfigureAwait(false);
                }
                else
                {
                    // not a match, so call next middleware handler
                    await next().ConfigureAwait(false);
                }
            });

            return builder;
        }
    }
}
