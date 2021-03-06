// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.ContractsLight;
using System.Text;

#pragma warning disable 1591 // disabling warning about missing API documentation; TODO: Remove this line and write documentation!
#pragma warning disable SA1649 // File name must match first type name

namespace BuildXL.FrontEnd.Script.Ambients
{
    /// <summary>
    /// Kinds of text encoding.
    /// </summary>
    public enum TextEncoding
    {
        /// <nodoc />
        Ascii = 0,

        /// <nodoc />
        BigEndianUnicode,

        /// <nodoc />
        Unicode,

        /// <nodoc />
        Utf32,

        /// <nodoc />
        Utf7,

        /// <nodoc />
        Utf8,
    }

    /// <summary>
    /// Utility class for text encoding.
    /// </summary>
    public static class TextEncodingUtils
    {
        /// <summary>
        ///     Converts from <see cref="TextEncoding" /> to <see cref="System.Text.Encoding" />
        /// </summary>
        /// <param name="kind"><see cref="TextEncoding" /> kind.</param>
        /// <returns><see cref="System.Text.Encoding" /> encoding.</returns>
        public static Encoding Convert(TextEncoding kind)
        {
            switch (kind)
            {
                case TextEncoding.Ascii:
                    return Encoding.ASCII;
                case TextEncoding.BigEndianUnicode:
                    return Encoding.BigEndianUnicode;
                case TextEncoding.Unicode:
                    return Encoding.Unicode;
                case TextEncoding.Utf32:
                    return Encoding.UTF32;
                case TextEncoding.Utf7:
#pragma warning disable SYSLIB0001 // 'Encoding.UTF7' is obsolete
                    return Encoding.UTF7;
#pragma warning restore SYSLIB0001
                case TextEncoding.Utf8:
                    return Encoding.UTF8;
            }

            throw Contract.AssertFailure("Unreachable code");
        }
    }
}
