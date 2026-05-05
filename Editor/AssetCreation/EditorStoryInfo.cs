using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Localization;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace HeroTeam.RichardPicture.StorySdk
{
    public class EditorStoryInfo : ScriptableObject
    {
        [HideInInspector] public required StoryPaths storyPaths;
        [HideInInspector] public required StoryInfo storyInfo;
        [HideInInspector] public required StringTableCollection stringTable;
        [HideInInspector] public required AssetTableCollection assetTable;
        [HideInInspector] public required AddressableAssetGroup addressableGroup;
    }
}