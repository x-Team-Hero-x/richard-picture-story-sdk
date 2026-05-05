using HeroTeam.RichardPicture.StorySdk.InformationAssets;

namespace HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation
{
	public class CharacterCreator : StoryAssetCreatorBase<CharacterInfo>
	{
		protected override string IdExample => "test_character";
		protected override string AssetPath => $"{editorStoryInfo.storyPaths.charactersFolder}/{id}.asset";
		protected override string AddressableName => $"Character-{id}";

		protected override void BeforeSave()
		{
			base.BeforeSave();
			SetupLocalizedProperty(CreatedAsset.spritePrefab, $"characters.{id}.sprite");
			SetupLocalizedProperty(CreatedAsset.displayName, $"characters.{id}.name");
			editorStoryInfo.storyInfo.characters.Add(CreatedAsset);
		}
	}
}