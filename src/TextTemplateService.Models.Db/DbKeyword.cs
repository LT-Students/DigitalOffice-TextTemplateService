﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.TextTemplateService.Models.Db
{
  public class DbKeyword
  {
    public const string TableName = "Keywords";

    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public string Keyword { get; set; }
  }

  public class DbParseEntityConfiguration : IEntityTypeConfiguration<DbKeyword>
  {
    public void Configure(EntityTypeBuilder<DbKeyword> builder)
    {
      builder
        .ToTable(DbKeyword.TableName);

      builder
        .HasKey(pe => pe.Id);

      builder
        .Property(pe => pe.Keyword)
        .IsRequired();
    }
  }
}
