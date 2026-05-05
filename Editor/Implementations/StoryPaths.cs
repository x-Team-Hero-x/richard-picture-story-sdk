using System;
using System.Collections.Generic;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Implementations
{
	[Serializable]
	public class StoryPaths
	{
		public string storyFolder;
		public string storyInfoAsset;
		public string localizationFolder;
		public string charactersFolder;
		public string assetsFolder;
		public string dialogFilesFolder;
		public string editorStoryInfoAsset;
		public string stringsTable;
		public string assetsTable;
		public IReadOnlyList<string> AllFolders => new[] {storyFolder, localizationFolder, charactersFolder, assetsFolder, dialogFilesFolder};
		
		public StoryPaths(string id)
		{
			storyFolder = $"{Paths.StoriesFolder}/{id}";
			storyInfoAsset = $"{storyFolder}/StoryInfo.asset";
			localizationFolder = $"{storyFolder}/Localization";
			charactersFolder = $"{storyFolder}/Characters";
			assetsFolder = $"{storyFolder}/Assets";
			dialogFilesFolder = $"{storyFolder}/Dialogs";
			editorStoryInfoAsset = $"{storyFolder}/EditorStoryInfo.asset";
			stringsTable = $"{id}.strings";
			assetsTable = $"{id}.assets";
		}
	}
}