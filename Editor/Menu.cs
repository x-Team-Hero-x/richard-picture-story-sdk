using UnityEditor;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	public static class Menu
	{
		private const string Prefix = "👀 Richard Picture/";
	
		[MenuItem(Prefix + "Create story")]
		private static void CreateStoryTemplate()
		{
			ScriptableWizard.DisplayWizard<StoryCreator>("Create story");
		}
	}
}