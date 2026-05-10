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
        [HideInInspector] public required DefaultAsset parentFolder;
        public string StoryFolderPath => $"{AssetDatabase.GetAssetPath(parentFolder)}/{storyInfo.id}";

        public string GetAssetPath(string storyRelativePath)
        {
            var projectRelativePath = $"{StoryFolderPath}/{storyRelativePath}";
            return projectRelativePath;
        }

        public string[] GetAssetPathsToBuild()
        {
            // TODO: exclude all editor assets to prevent warnings
            return new[] { StoryFolderPath };
        }
    }
}