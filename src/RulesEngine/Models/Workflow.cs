﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RulesEngine.Models
{
    [Obsolete("Workflow is now Workflow - Workflow will be removed in next major version")]
    [ExcludeFromCodeCoverage]
    public class WorkflowRule : Workflow {
    }

    /// <summary>
    /// Workflow rules class for deserialization  the json config file
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Workflow
    {
        /// <summary>
        /// Gets the workflow Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the workflow name.
        /// </summary>
        public string WorkflowName { get; set; }

        /// <summary>Gets or sets the workflow rules to inject.</summary>
        /// <value>The workflow rules to inject.</value>
        public IEnumerable<string> WorkflowsToInject { get; set; }

        /// <summary>
        /// Gets or Sets the global params which will be applicable to all rules
        /// </summary>
        public IEnumerable<ScopedParam> GlobalParams { get; set; }

        /// <summary>
        /// list of rules.
        /// </summary>
        public IEnumerable<Rule> Rules { get; set; }
    }
}