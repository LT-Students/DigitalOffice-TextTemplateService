using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.TextTemplateService.Data.Interfaces;
using LT.DigitalOffice.TextTemplateService.Data.Provider;
using LT.DigitalOffice.TextTemplateService.Models.Db;
using LT.DigitalOffice.TextTemplateService.Models.Dto.Requests.Template;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.TextTemplateService.Data
{
  public class EmailTemplateRepository : ITemplateRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmailTemplateRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid?> CreateAsync(DbTemplate request)
    {
      if (request == null)
      {
        return null;
      }

      _provider.Templates.Add(request);
      await _provider.SaveAsync();

      return request.Id;
    }

    public async Task<bool> EditAsync(Guid emailTemplateId, JsonPatchDocument<DbTemplate> patch)
    {
      if (patch == null)
      {
        return false;
      }

      DbTemplate dbEmailTemplate = await _provider.Templates
        .FirstOrDefaultAsync(et => et.Id == emailTemplateId);

      if (dbEmailTemplate == null)
      {
        return false;
      }

      patch.ApplyTo(dbEmailTemplate);
      dbEmailTemplate.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbEmailTemplate.ModifiedAtUtc = DateTime.UtcNow;
      await _provider.SaveAsync();

      return true;
    }

    public Task<DbTemplate> GetAsync(Guid emailTemplateId)
    {
      return _provider.Templates
        .Include(et => et.TextsTemplates)
        .FirstOrDefaultAsync(et => et.Id == emailTemplateId);
    }

    public Task<DbTemplate> GetAsync(int type)
    {
      return _provider.Templates
        .Include(et => et.TextsTemplates)
        .FirstOrDefaultAsync(et => et.Type == type && et.IsActive);
    }

    public async Task<(List<DbTemplate> dbEmailTempates, int totalCount)> FindAsync(FindTemplateFilter filter)
    {
      IQueryable<DbTemplate> dbEmailTemplates = _provider.Templates.AsQueryable();

      if (!filter.IncludeDeactivated)
      {
        dbEmailTemplates = dbEmailTemplates.Where(e => e.IsActive);
      }

      return (
        await dbEmailTemplates
          .Skip(filter.SkipCount)
          .Take(filter.TakeCount)
          .Include(e => e.TextsTemplates)
          .ToListAsync(),
        await dbEmailTemplates.CountAsync());
    }
  }
}
