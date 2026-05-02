using System;
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

		public static (string storyFolder, string localizationFolder, string storyInfoAsset, string stringsTable, string assetsTable) GetStoryPaths(string id)
		{
			var storyFolderPath = $"{StoriesFolder}/{id}";
			var localizationFolderPath = $"{storyFolderPath}/Localization";
			var storyInfoAssetPath = $"{storyFolderPath}/StoryInfo.asset";
			var stringsTableName = $"{id}.strings";
			var assetsTableName = $"{id}.assets";
			return (storyFolderPath, localizationFolderPath, storyInfoAssetPath, stringsTableName, assetsTableName);
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