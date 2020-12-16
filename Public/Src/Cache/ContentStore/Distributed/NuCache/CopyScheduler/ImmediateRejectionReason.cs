﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.


#nullable enable

namespace BuildXL.Cache.ContentStore.Distributed.NuCache.CopyScheduling
{
    /// <nodoc />
    public enum ImmediateRejectionReason
    {
        /// <nodoc />
        NotRejected,

        /// <nodoc />
        QueueTooLong,
    }
}