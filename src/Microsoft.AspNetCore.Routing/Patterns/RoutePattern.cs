// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Routing.Template;

namespace Microsoft.AspNetCore.Routing.Patterns
{
    /// <summary>
    /// Represents a parsed route template with default values and constraints.
    /// Use <see cref="RoutePatternFactory"/> to create <see cref="RoutePattern"/>
    /// instances. Instances of <see cref="RoutePattern"/> are immutable.
    /// </summary>
    [DebuggerDisplay("{DebuggerToString()}")]
    public sealed class RoutePattern
    {
        private const string SeparatorString = "/";

        internal RoutePattern(
            string rawText,
            Dictionary<string, object> defaults,
            Dictionary<string, IReadOnlyList<RoutePatternConstraintReference>> constraints,
            RoutePatternParameterPart[] parameters,
            RoutePatternPathSegment[] pathSegments)
        {
            Debug.Assert(defaults != null);
            Debug.Assert(constraints != null);
            Debug.Assert(parameters != null);
            Debug.Assert(pathSegments != null);

            RawText = rawText;
            Defaults = defaults;
            Constraints = constraints;
            Parameters = parameters;
            PathSegments = pathSegments;

            InboundPrecedence = RoutePrecedence.ComputeInbound(this);
            OutboundPrecedence = RoutePrecedence.ComputeOutbound(this);
        }

        /// <summary>
        /// Gets the set of default values for the route pattern.
        /// The keys of <see cref="Defaults"/> are the route parameter names.
        /// </summary>
        public IReadOnlyDictionary<string, object> Defaults { get; }

        /// <summary>
        /// Gets the set of constraint references for the route pattern.
        /// The keys of <see cref="Constraints"/> are the route parameter names.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyList<RoutePatternConstraintReference>> Constraints { get; }

        /// <summary>
        /// Gets the precedence value of the route pattern for URL matching.
        /// </summary>
        /// <remarks>
        /// Precedence is a computed value based on the structure of the route pattern
        /// used for building URL matching data structures.
        /// </remarks>
        public decimal InboundPrecedence { get; }

        /// <summary>
        /// Gets the precedence value of the route pattern for URL generation.
        /// </summary>
        /// <remarks>
        /// Precedence is a computed value based on the structure of the route pattern
        /// used for building URL generation data structures.
        /// </remarks>
        public decimal OutboundPrecedence { get; }

        /// <summary>
        /// Gets the raw text supplied when parsing the route pattern. May be null.
        /// </summary>
        public string RawText { get; }

        /// <summary>
        /// Gets the list of route parameters.
        /// </summary>
        public IReadOnlyList<RoutePatternParameterPart> Parameters { get; }

        /// <summary>
        /// Gets the list of path segments.
        /// </summary>
        public IReadOnlyList<RoutePatternPathSegment> PathSegments { get; }

        /// <summary>
        /// Gets the parameter matching the given name.
        /// </summary>
        /// <param name="name">The name of the parameter to match.</param>
        /// <returns>The matching parameter or <c>null</c> if no parameter matches the given name.</returns>
        public RoutePatternParameterPart GetParameter(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            for (var i = 0; i < Parameters.Count; i++)
            {
                var parameter = Parameters[i];
                if (string.Equals(parameter.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return parameter;
                }
            }

            return null;
        }

        private string DebuggerToString()
        {
            return RawText ?? string.Join(SeparatorString, PathSegments.Select(s => s.DebuggerToString()));
        }
    }
}
