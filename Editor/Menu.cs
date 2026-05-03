using UnityEditor;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	internal static class Menu
	{
		private const string Prefix = "👀 Richard Picture/";
		private const int Priority = 0;
	
		[MenuItem($"{Prefix}/Initialize project", priority = Priority + 0)]
		private static void InitializeProject()
		{
			ProjectInitialization.Initialize();
		}
	
		[MenuItem($"{Prefix}/Create story", priority = Priority + 1)]
		private static void CreateStoryTemplate()
		{
			ScriptableWizard.DisplayWizard<StoryCreator>("Create story");
		}
	
		[MenuItem($"{Prefix}/Check story", priority = Priority + 2)]
		private static async void CheckStory()
		{
			var path = EditorUtility.OpenFilePanel("Select story bundle", Paths.PackagedStoriesFolder, "json,bin");
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			Debug.Log($"Checking story at '{path}'...");
			using var storyInfo = await StoryInfo.FromFile(path);
			Debug.Log($"Story checked, got id '{storyInfo.id}' and {storyInfo.characters.Count} characters.");
			var character0 = storyInfo.characters[0];
			var id0 = character0.id;
			var name0 = await character0.displayName.GetLocalizedStringAsync().Task;
			Debug.Log($"0th character id={id0} and name={name0}");
		}
	}
}