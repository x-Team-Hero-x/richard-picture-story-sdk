using System;
using UnityEditor;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Implementations
{
	public static class Paths
	{
		public const string SdkFolder = "Assets/StorySDK";
		public const string LocalizationSettingsAsset = "Assets/StorySDK/Localization Settings.asset";
		public const string GroupNameResolver = "Assets/StorySDK/GroupNameResolver.asset";
		public const string StoriesFolder = "Assets/StorySDK/Stories";
		public const string PackagedStoriesFolder = "Library/com.unity.addressables/aa/Windows";

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
	}
}