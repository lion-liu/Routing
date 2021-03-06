﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Routing.Matching
{
    public struct CandidateState
    {
        internal CandidateState(MatcherEndpoint endpoint, int score)
        {
            Endpoint = endpoint;
            Score = score;

            IsValidCandidate = true;
            Values = null;
        }

        public MatcherEndpoint Endpoint { get; }

        public int Score { get; }

        public bool IsValidCandidate { get; set; }

        public RouteValueDictionary Values { get; set; }
    }
}
