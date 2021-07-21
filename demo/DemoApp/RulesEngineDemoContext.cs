﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RulesEngine.Models;

namespace DemoApp
{
    public class RulesEngineDemoContext : DbContext
    {
        public DbSet<WorkflowRules> WorkflowRules { get; set; }
        public DbSet<ActionInfo> ActionInfos { get; set; }

        public DbSet<RuleActions> RuleActions { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<ScopedParam> ScopedParams { get; set; }

        public string DbPath { get; private set; }

        public RulesEngineDemoContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}RulesEngineDemo.db";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
          => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActionInfo>()
                .Property(b => b.Context)
                .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => JsonConvert.DeserializeObject<Dictionary<string, object>>(v));

            modelBuilder.Entity<ActionInfo>()
              .HasKey(k => k.Name);

            modelBuilder.Entity<ScopedParam>()
              .HasKey(k => k.Name);

            modelBuilder.Entity<WorkflowRules>(entity => {
                entity.HasKey(k => k.WorkflowName); 
            });

            modelBuilder.Entity<RuleActions>(entity => {
                entity.HasNoKey();
                entity.HasOne(o => o.OnSuccess).WithMany();
                entity.HasOne(o => o.OnFailure).WithMany();
            });

            modelBuilder.Entity<Rule>(entity => {
                entity.HasKey(k => k.RuleName);

                //EF translates an empty IEnumerable to a new Object with Count of 0 not a null (like JSON)
                //Using HasConversion has nesting issues
                // Message=The property 'Rule.Rules' is of type 'IEnumerable<Rule>' which is not supported by the current database provider. Either change the property CLR type, or ignore the property using the '[NotMapped]' attribute or by using 'EntityTypeBuilder.Ignore' in 'OnModelCreating'.
                //
                //entity.Property(b => b.Rules)
                //.HasConversion(
                //    v => v,
                //    v => v.Count() == 0 ? null : v);
               
                entity.Property(b => b.Properties)
                .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => JsonConvert.DeserializeObject<Dictionary<string, object>>(v));
                entity.Ignore(e => e.Actions);
            });

            modelBuilder.Entity<WorkflowRules>()
               .Ignore(b => b.WorkflowRulesToInject);

            modelBuilder.Entity<Rule>()
              .Ignore(b => b.WorkflowRulesToInject);
        }
    }

}