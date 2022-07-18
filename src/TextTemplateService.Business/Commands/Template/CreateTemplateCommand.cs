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
using LT.DigitalOffice.TextTemplateService.Mappers.Db.Interfaces;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Requests.Template;
using LT.DigitalOffice.TextTemplateService.Validation.Validators.Template.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.TextTemplateService.Business.Commands.Template
{
  public class CreateTemplateCommand : ICreateTemplateCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ICreateTemplateValidator _validator;
    private readonly IDbTemplateMapper _mapper;
    private readonly ITemplateRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public CreateTemplateCommand(
      IAccessValidator accessValidator,
      ICreateTemplateValidator validator,
      IDbTemplateMapper mapper,
      ITemplateRepository repository,
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

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(TemplateRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailsTemplates))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest, errors);
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

