// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

/// <summary>
/// Special marker attribute used by ErrorProne.NET analyzer to warn when <see cref="System.Threading.Tasks.Task.ConfigureAwait"/> method is used.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Assembly)]
public class DoNotUseConfigureAwaitAttribute : System.Attribute { }
