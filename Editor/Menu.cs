using UnityEditor;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	internal static class Menu
	{
		private const string Prefix = "👀 Richard Picture/";
		private const int Priority = 0;
	
		[MenuItem($"{Prefix}/Initialize project", priority = Priority + 0)]
		private static void InitializeProject()
		{
			var doInitialize =
				EditorUtility.DisplayDialog(
					"Are you sure?",
					"Initializing a project for work with StorySDK can break stuff. " +
					"It is recommended to use it only on empty projects. " +
					"Do you want to continue? ",
					"Initialize StorySDK", "Cancel");
			if (doInitialize)
			{
				ProjectInitialization.Initialize();	
			}
		}
	
		[MenuItem($"{Prefix}/Create story", priority = Priority + 1)]
		private static void CreateStoryTemplate()
		{
			ScriptableWizard.DisplayWizard<StoryCreator>("Create story");
		}

		[MenuItem($"{Prefix}/Check story", priority = Priority + 2)]
		private static void CheckStory()
		{
			_ = StoryChecker.CheckStory();
		}
	}
}