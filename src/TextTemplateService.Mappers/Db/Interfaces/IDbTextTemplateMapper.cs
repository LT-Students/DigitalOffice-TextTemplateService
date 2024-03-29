﻿using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.TextTemplateService.Models.Db;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Requests.TemplateText;

namespace LT.DigitalOffice.TextTemplateService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbTextTemplateMapper
  {
    DbTextTemplate Map(TemplateTextRequest request, Guid? emailTemplateId = null);
  }
}
