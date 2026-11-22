using System.IO;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation
{
	public class EffectCreator : StoryAssetCreatorBase<EffectInfo>
	{
		protected override string IdExample => "test_effect";

		protected override void OnBeforeCreate()
		{
			base.OnBeforeCreate();
			SetupLocalizedProperty(CreatedAsset.effectPrefab, $"effects.{id}");
		}
	}
}