﻿using System.Linq;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.TextTemplateService.Mappers.Models.Interfaces;
using LT.DigitalOffice.TextTemplateService.Models.Db;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Models;

namespace LT.DigitalOffice.TextTemplateService.Mappers.Models
{
  public class TemplateInfoMapper : ITemplateInfoMapper
  {
    private readonly ITemplateTextInfoMapper _mapper;

    public TemplateInfoMapper(
      ITemplateTextInfoMapper mapper)
    {
      _mapper = mapper;
    }

    public TemplateInfo Map(DbTemplate dbEmailTemplate)
    {
      if (dbEmailTemplate is null)
      {
        return null;
      }

      return new TemplateInfo
      {
        Id = dbEmailTemplate.Id,
        Type = (TemplateType)dbEmailTemplate.Type,
        IsActive = dbEmailTemplate.IsActive,
        CreatedBy = dbEmailTemplate.CreatedBy,
        CreatedAtUtc = dbEmailTemplate.CreatedAtUtc,
        Texts = dbEmailTemplate.TextsTemplates?
          .Select(_mapper.Map)
          .ToList()
      };
    }
  }
}
