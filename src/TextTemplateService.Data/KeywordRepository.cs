using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.TextTemplateService.Data.Interfaces;
using LT.DigitalOffice.TextTemplateService.Data.Provider;
using LT.DigitalOffice.TextTemplateService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.TextTemplateService.Data
{
  public class KeywordRepository : IKeywordRepository
  {
    private readonly IDataProvider _provider;

    public KeywordRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> CreateAsync(List<DbKeyword> dbKeywords)
    {
      if (dbKeywords is null || !dbKeywords.Any())
      {
        return false;
      }

      _provider.Keywords.AddRange(dbKeywords);
      await _provider.SaveAsync();

      return true;
    }

    public Task<List<DbKeyword>> GetAsync(Guid endpointId)
    {
      return _provider.Keywords
        .Where(k => k.EndpointId == endpointId)
        .ToListAsync();
    }

    public Task<List<DbKeyword>> GetAsync(List<Guid> endpointsIds)
    {
      return _provider.Keywords
        .Where(k => endpointsIds.Contains(k.EndpointId))
        .ToListAsync();
    }
  }
}
