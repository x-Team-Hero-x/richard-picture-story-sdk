using System.IO;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Localization;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace HeroTeam.RichardPicture.StorySdk
{
    public class EditorStoryInfo : ScriptableObject
    {
        [HideInInspector] public required StoryInfo storyInfo;
        [HideInInspector] public required StringTableCollection stringTable;
        [HideInInspector] public required AssetTableCollection assetTable;
        [HideInInspector] public required AddressableAssetGroup addressableGroup;

        public string GetAssetPath(string storyRelativePath)
        {
            var thisPath = AssetDatabase.GetAssetPath(this);
            var storyFolderPath = !string.IsNullOrEmpty(thisPath)
                ? thisPath[..thisPath.LastIndexOf('/')]
                : $"{Paths.StoriesFolder}/{storyInfo.id}"; //TODO: ask user location instead of using hardcoded
            var projectRelativePath = $"{storyFolderPath}/{storyRelativePath}";
            return projectRelativePath;
        }
    }
}