using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.TextTemplateService.Business.Commands.TemplateText.Interfaces;
using LT.DigitalOffice.TextTemplateService.Data.Interfaces;
using LT.DigitalOffice.TextTemplateService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Requests.TemplateText;
using LT.DigitalOffice.TextTemplateService.Validation.Validators.TemplateText.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.TextTemplateService.Business.Commands.TemplateText
{
  public class EditTemplateTextCommand : IEditTemplateTextCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ITextTemplateRepository _repository;
    private readonly IEditTemplateTextValidator _validator;
    private readonly IPatchDbTemplateTextMapper _mapper;
    private readonly IResponseCreator _responseCreator;

    public EditTemplateTextCommand(
      IAccessValidator accessValidator,
      ITextTemplateRepository repository,
      IEditTemplateTextValidator validator,
      IPatchDbTemplateTextMapper mapper,
      IResponseCreator responseCreator)
    {
      _validator = validator;
      _repository = repository;
      _accessValidator = accessValidator;
      _mapper = mapper;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid emailTemplateTextId,
      JsonPatchDocument<EditTemplateTextRequest> patch)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailsTemplates))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(patch, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          errors);
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(emailTemplateTextId, _mapper.Map(patch));

      if (!response.Body)
      {
        response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
