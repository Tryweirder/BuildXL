// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using BuildXL.FrontEnd.Script.Evaluator;
using BuildXL.FrontEnd.Script.Expressions;
using BuildXL.FrontEnd.Script.Values;
using TypeScript.Net.Utilities;

namespace BuildXL.FrontEnd.Script.Ambients.Map
{
    /// <summary>
    /// Exception for invalid key-value pair for map.
    /// </summary>
    public sealed class InvalidKeyValueMapException : EvaluationExceptionWithErrorContext
    {
        /// <nodoc />
        public InvalidKeyValueMapException(ErrorContext errorContext)
            : base("Invalid key-value pair (key-value pair must be a two-element array [key, value])", errorContext)
        {
        }

        /// <inheritdoc/>
        public override void ReportError(EvaluationErrors errors, ModuleLiteral environment, LineInfo location, Expression expression, Context context)
        {
            errors.ReportInvalidKeyValueMap(environment, ErrorContext, Message, location);
        }
    }
}
