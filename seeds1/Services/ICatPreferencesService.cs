﻿using seeds.Dal.Dto.ToApi;
using seeds1.MauiModels;

namespace seeds1.Services;

public interface ICatPreferencesService
{
    public Task<IEnumerable<CatPreference>> GetCatPreferencesAsync();
    public int StepCatPreference(int oldPreference);
}
