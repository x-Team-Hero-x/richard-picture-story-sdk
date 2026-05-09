using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
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

        public string GetStoryFolderPath()
        {
            var thisPath = AssetDatabase.GetAssetPath(this);
            var storyFolderPath = !string.IsNullOrEmpty(thisPath)
                ? Paths.SplitPath(thisPath).parentFolderPath
                : $"{Paths.StoriesFolder}/{storyInfo.id}"; //TODO: ask user location instead of using hardcoded
            return storyFolderPath;
        }

        public string GetAssetPath(string storyRelativePath)
        {
            var storyFolderPath = GetStoryFolderPath();
            var projectRelativePath = $"{storyFolderPath}/{storyRelativePath}";
            return projectRelativePath;
        }

        public string[] GetAllAssetPaths()
        {
            // TODO: check if it is enough
            var storyFolderPath = GetStoryFolderPath();
            return new[] { storyFolderPath };
        }
    }
}