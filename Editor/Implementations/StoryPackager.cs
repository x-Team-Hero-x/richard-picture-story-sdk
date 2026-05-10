using System;
using System.IO;
using UnityEditor;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Implementations
{
	public static class StoryPackager
	{
		public static void Package(EditorStoryInfo editorStoryInfo)
		{
			var userSelectedPath = EditorUtility.SaveFilePanel(
				"Select output location",
				null,
				$"{editorStoryInfo.storyInfo.id}.story",
				"story"
			);
			if (userSelectedPath == "")
			{
				throw new OperationCanceledException("No story file selected");
			}
			var (_, storyFileName) = Paths.SplitPath(userSelectedPath);
			
			var build = new AssetBundleBuild
			{
				assetBundleName = storyFileName,
				assetNames = editorStoryInfo.GetAssetPathsToBuild(),
			};
			Paths.EnsureFolderExists(Paths.PackagedStoriesFolder);
			BuildPipeline.BuildAssetBundles(
				Paths.PackagedStoriesFolder,
				new[] { build },
				BuildAssetBundleOptions.StrictMode,
				EditorUserBuildSettings.activeBuildTarget // TODO: build for more targets?
			);
			var buildCachePath = $"{Paths.PackagedStoriesFolder}/{storyFileName}";
			File.Copy(buildCachePath, userSelectedPath, true);
		}
	}
}