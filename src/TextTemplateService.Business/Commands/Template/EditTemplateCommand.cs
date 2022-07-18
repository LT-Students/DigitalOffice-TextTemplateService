using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.TextTemplateService.Business.Commands.Template.Interfaces;
using LT.DigitalOffice.TextTemplateService.Data.Interfaces;
using LT.DigitalOffice.TextTemplateService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Requests.Template;
using LT.DigitalOffice.TextTemplateService.Validation.Validators.Template.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.TextTemplateService.Business.Commands.Template
{
  public class EditTemplateCommand : IEditTemplateCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ITemplateRepository _repository;
    private readonly IEditTemplateValidator _validator;
    private readonly IPatchDbTemplateMapper _mapper;
    private readonly IResponseCreator _responseCreator;

    public EditTemplateCommand(
      IAccessValidator accessValidator,
      ITemplateRepository repository,
      IEditTemplateValidator validator,
      IPatchDbTemplateMapper mapper,
      IResponseCreator responseCreator)
    {
      _validator = validator;
      _repository = repository;
      _accessValidator = accessValidator;
      _mapper = mapper;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid emailTemplateId,
      JsonPatchDocument<EditTemplateRequest> patch)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailsTemplates))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(patch, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(emailTemplateId, _mapper.Map(patch));

      if (!response.Body)
      {
        response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}

