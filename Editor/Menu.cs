using UnityEditor;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	internal static class Menu
	{
		private const string Prefix = "👀 Richard Picture/";
		private const int Priority = 0;
	
		[MenuItem(Prefix + "Initialize project", priority = Priority + 0)]
		private static void InitializeProject()
		{
			ProjectInitialization.Initialize();
		}
	
		[MenuItem(Prefix + "Create story", priority = Priority + 1)]
		private static void CreateStoryTemplate()
		{
			ScriptableWizard.DisplayWizard<StoryCreator>("Create story");
		}
	}
}