using System.IO;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation
{
	public class BackgroundCreator : StoryAssetCreatorBase<BackgroundInfo>
	{
		protected override string IdExample => "test_background";

		protected override void OnBeforeCreate()
		{
			base.OnBeforeCreate();
			SetupLocalizedProperty(CreatedAsset.backgroundSprite, $"backgrounds.{id}");
		}
	}
}