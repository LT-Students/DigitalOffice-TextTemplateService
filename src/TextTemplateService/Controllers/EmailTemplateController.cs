﻿using System;
using System.Threading.Tasks;
using LT.DigitalOffice.TextTemplateService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Models;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.TextTemplateService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class EmailTemplateController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateEmailTemplateCommand command,
      [FromBody] EmailTemplateRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditEmailTemplateCommand command,
      [FromQuery] Guid emailTemplateId,
      [FromBody] JsonPatchDocument<EditEmailTemplateRequest> patch)
    {
      return await command.ExecuteAsync(emailTemplateId, patch);
    }

    [HttpGet("find")]
    public async Task<FindResultResponse<EmailTemplateInfo>> FindAsync(
      [FromServices] IFindEmailTemplateCommand command,
      [FromQuery] FindEmailTemplateFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }
  }
}
