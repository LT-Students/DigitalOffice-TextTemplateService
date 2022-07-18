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
using LT.DigitalOffice.TextTemplateService.Mappers.Db.Interfaces;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Requests.TemplateText;
using LT.DigitalOffice.TextTemplateService.Validation.Validators.TemplateText.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.TextTemplateService.Business.Commands.TemplateText
{
  public class CreateTemplateTextCommand : ICreateTemplateTextCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ICreateTemplateTextValidator _validator;
    private readonly IDbTextTemplateMapper _mapper;
    private readonly ITextTemplateRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public CreateTemplateTextCommand(
      IAccessValidator accessValidator,
      ICreateTemplateTextValidator validator,
      IDbTextTemplateMapper mapper,
      ITextTemplateRepository repository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _accessValidator = accessValidator;
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(TemplateTextRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailsTemplates))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(
          HttpStatusCode.BadRequest,
          errors);
      }

      OperationResultResponse<Guid?> response = new();

      response.Body = await _repository.CreateAsync(_mapper.Map(request));

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      if (response.Body == null)
      {
        response = _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
