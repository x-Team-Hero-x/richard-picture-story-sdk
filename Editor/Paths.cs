using System;
using System.Collections.Generic;
using UnityEditor;
using Object = UnityEngine.Object;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	public static class Paths
	{
		public const string SdkFolder = "Assets/StorySDK";
		public const string StoriesFolder = "Assets/StorySDK/Stories";
		public const string GroupNameResolver = "Assets/StorySDK/GroupNameResolver.asset";
		public const string PackagedStoriesFolder = "Library/com.unity.addressables/aa/Windows";

		public record StoryPaths(
			string StoryFolder,
			string StoryInfoAsset,
			string LocalizationFolder,
			string CharactersFolder,
			string AssetsFolder,
			string DialogFilesFolder,
			string StringsTable,
			string AssetsTable
		)
		{
			public readonly IReadOnlyList<string> AllFolders = new[] {StoryFolder, LocalizationFolder, CharactersFolder, AssetsFolder, DialogFilesFolder};
		}
		
		public static StoryPaths GetStoryPaths(string id)
		{
			var storyFolderPath = $"{StoriesFolder}/{id}";
			return new StoryPaths(
				StoryFolder: storyFolderPath,
				StoryInfoAsset: $"{storyFolderPath}/StoryInfo.asset",
				LocalizationFolder: $"{storyFolderPath}/Localization",
				CharactersFolder: $"{storyFolderPath}/Characters",
				AssetsFolder: $"{storyFolderPath}/Assets",
				DialogFilesFolder: $"{storyFolderPath}/Dialogs",
				StringsTable: $"{id}.strings",
				AssetsTable: $"{id}.assets"
			);
		}

		public static void EnsureFolderExists(string folderPath)
		{
			if (folderPath == "Assets")
			{
				return;
			}
			
			if (!folderPath.StartsWith("Assets/"))
			{
				throw new ArgumentException("Folder should start with 'Assets/'", nameof(folderPath));
			}
			
			if (AssetDatabase.IsValidFolder(folderPath))
			{
				return;
			}
			
			var lastSlashIndex = folderPath.LastIndexOf('/');
			var parentPath = folderPath[..lastSlashIndex];
			var folderName = folderPath[(lastSlashIndex+1)..];
			EnsureFolderExists(parentPath);
			AssetDatabase.CreateFolder(parentPath, folderName);
		}

		public static GUID GetAssetGuid(Object asset)
		{
			var path = AssetDatabase.GetAssetPath(asset);
			var guid = AssetDatabase.GUIDFromAssetPath(path);
			return guid;
		}

		public static string GetAssetGuidString(Object asset)
		{
			var path = AssetDatabase.GetAssetPath(asset);
			var guid = AssetDatabase.AssetPathToGUID(path);
			return guid;
		}
	}
}