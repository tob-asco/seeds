﻿using seeds.Dal.Dto.ForMaui;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToDb;
using seeds.Dal.Interfaces;

namespace seeds.Dal.Services;

public class IdeasService : IIdeasService
{
    private readonly IDalBaseService _baseService;
    public IdeasService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<IdeaFromDb> GetIdeaAsync(int id)
    {
        string url = $"api/Ideas/{id}";
        return await _baseService.GetDalModelAsync<IdeaFromDb>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }
    public async Task<List<IdeaFromDb>> GetIdeasPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime), bool isDescending = true)
    {
        string url = $"api/ideas/page/{pageIndex}?pageSize={pageSize}" +
            $"&orderByColumn={orderByColumn}&isDescending={isDescending}";
        return await _baseService.GetDalModelAsync<List<IdeaFromDb>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }
    public async Task<List<Feedentry>> GetFeedentriesPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime), bool isDescending = true)
    {
        string url = $"api/ideas/feedentryPage/{pageIndex}?pageSize={pageSize}" +
            $"&orderByColumn={orderByColumn}&isDescending={isDescending}";
        return await _baseService.GetDalModelAsync<List<Feedentry>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }
    public async Task<IdeaFromDb> PostIdeaAsync(IdeaToDb idea)
    {
        string url = "api/Ideas";
        return await _baseService.PostDalModelAsync<IdeaToDb, IdeaFromDb>(url, idea);
    }
}
