﻿using seeds.Dal.Model;

namespace seeds1.MauiModels;

/// <summary>
/// Maui Model that is directly needed for binding in views that display all Tags.
/// </summary>
public class FamilyOrPreference
{
    /// <summary>
    /// Either the Family's, or the Preference's Tag's CategoryKey.
    /// </summary>
    public string CategoryKey { get; set; } = "NoC";
    /// <summary>
    /// IsFamily is telling the DataTemplateSelector whether to display for a Family or a Preference.
    /// </summary>
    public bool IsFamily { get; set; } = false;
    public Family Family { get; set; }
    public CatagPreference Preference { get; set; }
}