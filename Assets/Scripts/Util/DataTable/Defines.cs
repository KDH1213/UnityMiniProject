using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    Korea,
    English,
    Japanese,
}

public static class DataTableIds
{
    public static readonly string[] String =
    {
        "StringTableKr",
        "StringTableEn",
        "StringTableJp",
    };
}

public static class ItemTableIds
{
    public static readonly string[] String =
    {
        "ItemTable",
    };
}

public static class Varibalbes
{
    public static Languages currentLanguage = Languages.Korea;
}

public static class Tags
{
    public static readonly string Player = "Player";
}

public static class SortingLayer
{
    public static readonly string Defalyle = "Default";
}

public static class Layers
{
    public static readonly string Default = "Default";
    public static readonly string Ui = "Ui";
}