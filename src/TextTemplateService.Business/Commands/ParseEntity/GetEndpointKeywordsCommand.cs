using System;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.TextTemplateService.Business.Commands.ParseEntity.Interfaces;
using LT.DigitalOffice.TextTemplateService.Data.Interfaces;
using LT.DigitalOffice.TextTemplateService.Mappers.Models.Interfaces;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Models;

namespace LT.DigitalOffice.TextTemplateService.Business.Commands.ParseEntity
{
  public class GetEndpointKeywordsCommand : IGetEndpointKeywordsCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IKeywordRepository _repository;
    private readonly IEndpointKeywordsInfoMapper _mapper;
    private readonly IResponseCreator _responseCreator;

    public GetEndpointKeywordsCommand(
      IAccessValidator accessValidator,
      IKeywordRepository repository,
      IEndpointKeywordsInfoMapper mapper,
      IResponseCreator responseCreator)
    {
      _accessValidator = accessValidator;
      _repository = repository;
      _mapper = mapper;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<EndpointKeywordsInfo>> ExecuteAsync(Guid endpointId)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailsTemplates))
      {
        return _responseCreator.CreateFailureResponse<EndpointKeywordsInfo>(HttpStatusCode.Forbidden);
      }

      return new()
      {
        Body = _mapper.Map(await _repository.GetAsync(endpointId))
      };
    }
  }
}

