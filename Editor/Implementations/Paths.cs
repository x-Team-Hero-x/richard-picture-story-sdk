using System;
using UnityEditor;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Implementations
{
	public static class Paths
	{
		public const string SdkFolder = "Assets/StorySDK";
		public const string LocalizationSettingsAsset = "Assets/StorySDK/Localization Settings.asset";
		public const string StoriesFolder = "Assets/StorySDK/Stories";

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
			
			var (parentPath, folderName) = SplitPath(folderPath);
			EnsureFolderExists(parentPath);
			AssetDatabase.CreateFolder(parentPath, folderName);
		}
		
		public static (string parentFolderPath, string name) SplitPath(string path)
		{
			var lastSlashIndex = path.LastIndexOf('/');
			var parentPath = path[..lastSlashIndex];
			var name = path[(lastSlashIndex+1)..];
			return (parentPath, name);
		}
	}
}