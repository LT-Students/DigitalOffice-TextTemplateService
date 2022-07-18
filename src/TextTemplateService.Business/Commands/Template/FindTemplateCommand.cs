using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.TextTemplateService.Business.Commands.Template.Interfaces;
using LT.DigitalOffice.TextTemplateService.Data.Interfaces;
using LT.DigitalOffice.TextTemplateService.Mappers.Models.Interfaces;
using LT.DigitalOffice.TextTemplateService.Models.Db;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Models;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Requests.Template;

namespace LT.DigitalOffice.TextTemplateService.Business.Commands.Template
{
  public class FindTemplateCommand : IFindTemplateCommand
  {
    private readonly IBaseFindFilterValidator _baseFindValidator;
    private readonly ITemplateRepository _repository;
    private readonly ITemplateInfoMapper _mapper;
    private readonly IResponseCreator _responseCreator;

    public FindTemplateCommand(
      IBaseFindFilterValidator baseFindValidator,
      ITemplateRepository repository,
      ITemplateInfoMapper mapper,
      IResponseCreator responseCreator)
    {
      _baseFindValidator = baseFindValidator;
      _repository = repository;
      _mapper = mapper;
      _responseCreator = responseCreator;
    }

    public async Task<FindResultResponse<TemplateInfo>> ExecuteAsync(FindTemplateFilter filter)
    {
      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreator.CreateFailureFindResponse<TemplateInfo>(
          HttpStatusCode.BadRequest,
          errors);
      }

      (List<DbTemplate> dbTempates, int totalCount) repositoryResponse =
        await _repository.FindAsync(filter);

      return new()
      {
        Body = repositoryResponse.dbTempates?.Select(_mapper.Map).ToList(),
        TotalCount = repositoryResponse.totalCount,
      };
    }
  }
}

