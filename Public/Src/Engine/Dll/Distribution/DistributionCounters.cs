// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using BuildXL.Utilities.Tracing;

namespace BuildXL.Engine.Distribution
{
    /// <summary>
    /// Counters related to distributed builds
    /// </summary>
    public enum DistributionCounter : ushort
    {
        /// <nodoc/>
        [CounterType(CounterType.Stopwatch)]
        ReportPipsCompletedDuration,

        /// <nodoc/>
        [CounterType(CounterType.Stopwatch)]
        SendEventMessagesDuration,

        /// <summary>
        /// Tracks occurrences of Bond 'No such method' exception
        /// </summary>
        ClientNoSuchMethodErrorCount,

        /// <summary>
        /// Tracks occurrences of Bond message checksum mismatches on client
        /// </summary>
        ClientChecksumMismatchCount,

        /// <summary>
        /// Tracks occurrences of Bond message checksum mismatches on server
        /// </summary>
        ServerChecksumMismatchCount,

        /// <summary>
        /// The total size of messages that are received
        /// </summary>
        ReceivedMessageSizeBytes,

        /// <summary>
        /// The total size of messages that are sent
        /// </summary>
        SentMessageSizeBytes,

        /// <summary>
        /// The number of build request messages that fail to be sent to worker.
        /// </summary>
        FailedSendPipBuildRequestCount,

        /// <summary>
        /// Time spent serializing the execution result of a pip request on worker
        /// </summary>
        [CounterType(CounterType.Stopwatch)]
        WorkerServiceResultSerializationDuration,

        /// <summary>
        /// Time spent flushing the execution log before sending pip results
        /// </summary>
        [CounterType(CounterType.Stopwatch)]
        WorkerFlushExecutionLogDuration,

        /// <nodoc/>
        BuildResultBatchesSentToMaster,

        /// <nodoc/>
        [CounterType(CounterType.Stopwatch)]
        SendExecutionLogDuration,

        /// <nodoc/>
        FailedSendPipBuildRequestCallDurationMs,

        /// <nodoc/>
        SendPipBuildRequestCallDurationMs,

        /// Time sending notifications to the orchestrator
        [CounterType(CounterType.Stopwatch)]
        SendNotificationDuration,
    }
}
