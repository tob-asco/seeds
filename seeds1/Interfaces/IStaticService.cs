﻿using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;

namespace seeds1.Interfaces;

public interface IStaticService
{
    /// <summary>
    /// Calls all other Load methods of StaticService.
    /// </summary>
    public Task LoadStaticsAsync();
    /// <summary>
    /// Loads Categories, to be retrieved by GetCategories().
    /// </summary>
    public Task LoadCategoriesAsync();
    public Dictionary<string, CategoryDto> GetCategories();
    /// <summary>
    /// Loads Families, to be retrieved by GetFamilies().
    /// </summary>
    public Task LoadFamiliesAsync();
    public Dictionary<Guid, FamilyFromDb> GetFamilies();
    /// <summary>
    /// Loads Topics, to be retrieved by GetTopics().
    /// </summary>
    public Task LoadTopicsAsync();
    public Dictionary<Guid, TopicFromDb> GetTopics();
}
