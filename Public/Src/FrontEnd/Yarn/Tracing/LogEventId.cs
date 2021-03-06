﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace BuildXL.FrontEnd.Yarn.Tracing
{
    // disable warning regarding 'missing XML comments on public API'. We don't need docs for these values
    #pragma warning disable 1591

    /// <summary>
    /// Defines event IDs corresponding to events in <see cref="Logger" />
    /// </summary>
    public enum LogEventId
    {
        None = 0,

        // reserved 12700 .. 12800 for Yarn front-end
        UsingYarnAt = 12700,
        ErrorReadingCustomProjectGraph = 12701,
        CannotSerializeGraphFile = 12702,
    }
}
