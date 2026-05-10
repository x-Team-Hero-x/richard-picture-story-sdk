using System.IO;
using UnityEditor;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Implementations
{
	public static class Paths
	{
		public const string LocalizationSettingsAsset = "Assets/LocalizationSettings.asset";
		public const string PackagedStoriesFolder = "Library/io.github.x-team-hero-x.richard-picture-story-sdk";

		public static void EnsureFolderExists(string folderPath)
		{
			if (!folderPath.StartsWith("Assets/"))
			{
				Directory.CreateDirectory(folderPath);
				return;
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