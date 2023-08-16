﻿using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Dal.Services;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly IDalBaseService _baseService;
    public UserPreferenceService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task UpsertUserPreferenceAsync(
        string username, Guid itemId, int newValue)
    {
        string url = $"api/UserPreferences/upsert";
        UserPreference cup = new()
        {
            ItemId = itemId,
            Username = username,
            Value = newValue
        };
        
        if(!await _baseService.PostDalModelBoolReturnAsync(url, cup))
        {
            throw _baseService.ThrowPostConflictException(url);
        }
    }
}