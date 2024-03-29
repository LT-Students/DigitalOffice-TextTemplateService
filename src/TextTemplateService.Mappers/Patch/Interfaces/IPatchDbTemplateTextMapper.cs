﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.TextTemplateService.Models.Db;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Requests.TemplateText;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.TextTemplateService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchDbTemplateTextMapper
  {
    JsonPatchDocument<DbTextTemplate> Map(
      JsonPatchDocument<EditTemplateTextRequest> request);
  }
}
