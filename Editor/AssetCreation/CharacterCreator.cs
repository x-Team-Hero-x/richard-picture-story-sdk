using HeroTeam.RichardPicture.StorySdk.InformationAssets;

namespace HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation
{
	public class CharacterCreator : StoryAssetCreatorBase<CharacterInfo>
	{
		protected override string IdExample => "test_character";

		protected override void OnBeforeCreate()
		{
			base.OnBeforeCreate();
			SetupLocalizedProperty(CreatedAsset.spritePrefab, $"characters.{id}.sprite");
			SetupLocalizedProperty(CreatedAsset.displayName, $"characters.{id}.name");
			editorStoryInfo.storyInfo.characters.Add(CreatedAsset);
		}
	}
}