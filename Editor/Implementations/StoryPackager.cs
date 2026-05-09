using UnityEditor;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Implementations
{
	public static class StoryPackager
	{
		public static void Package(EditorStoryInfo editorStoryInfo)
		{
			var storyId = editorStoryInfo.storyInfo.id;
			var outputPath = EditorUtility.SaveFilePanel(
				"Select output location",
				null,
				$"{storyId}.story",
				"story"
			);
			var (outputFolder, storyFileName) = Paths.SplitPath(outputPath);
			
			var build = new AssetBundleBuild
			{
				assetBundleName = storyFileName,
				assetNames = editorStoryInfo.GetAllAssetPaths(),
			};
			BuildPipeline.BuildAssetBundles(
				outputFolder,
				new[] { build },
				BuildAssetBundleOptions.StrictMode,
				EditorUserBuildSettings.activeBuildTarget
			);
			// TODO: build for more targets
		}
	}
}