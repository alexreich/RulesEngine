﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Newtonsoft.Json;
using RulesEngine.Models;
using RulesEngine.UnitTest.ActionTests.MockClass;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace RulesEngine.UnitTest.ActionTests
{
    [ExcludeFromCodeCoverage]
    public class CustomActionTest
    {
        [Fact]
        public async Task CustomActionOnRuleMustHaveContextValues()
        {
            var workflows = GetWorkflows();
            var re = new RulesEngine(workflows, null, reSettings: new ReSettings {
                CustomActions = new Dictionary<string, System.Func<Actions.ActionBase>> {

                    { "ReturnContext", () => new ReturnContextAction() }
                }
            });

            var result = await re.ExecuteAllRulesAsync("successReturnContextAction", true);
        }


        [Fact]
        public async Task CustomAction_WithSystemTextJsobOnRuleMustHaveContextValues()
        {
            var workflows = GetWorkflows();
            var workflowStr = JsonConvert.SerializeObject(workflows);
            var serializationOptions = new System.Text.Json.JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
            var workflowViaTextJson = System.Text.Json.JsonSerializer.Deserialize<Workflow[]>(workflowStr,serializationOptions);


            var re = new RulesEngine(workflows, null, reSettings: new ReSettings {
                CustomActions = new Dictionary<string, System.Func<Actions.ActionBase>> {

                    { "ReturnContext", () => new ReturnContextAction() }
                }
            });



            var result = await re.ExecuteAllRulesAsync("successReturnContextAction", true);
        }

        private Workflow[] GetWorkflows()
        {
            return new Workflow[] {
                new Workflow {
                    WorkflowName = "successReturnContextAction",
                    Rules = new Rule[] {
                        new Rule {
                            RuleName = "trueRule",
                            Expression = "input1 == true",
                            Actions = new RuleAction() {
                                OnSuccess = new ActionInfo {
                                    Name = "ReturnContext",
                                    Context =  new Dictionary<string, object> {
                                        {"stringContext", "hello"},
                                        {"intContext",1 },
                                        {"objectContext", new { a = "hello", b = 123 } }
                                    }
                                }

                            }

                        },
                        

                    }
                }

            };
        }


    }
}
