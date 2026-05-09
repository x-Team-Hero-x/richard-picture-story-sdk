using System;
using System.Collections.Generic;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Localization.Addressables;
using UnityEngine.Localization;
using Object = UnityEngine.Object;

namespace HeroTeam.RichardPicture.StorySdk.Editor.AssetProcessors
{
	[Serializable]
	public class PerStoryLocalizationResolver : GroupResolver
	{
		public override string GetExpectedGroupName(IList<LocaleIdentifier> locales, Object asset, AddressableAssetSettings aaSettings)
		{
			const string storyPrefix = $"{Paths.StoriesFolder}/";
			var assetPath = AssetDatabase.GetAssetPath(asset);
			if (!assetPath.StartsWith(storyPrefix))
			{
				return base.GetExpectedGroupName(locales, asset, aaSettings);
			}
			
			var assetGuid = AssetDatabase.AssetPathToGUID(assetPath);
			var existingEntry = aaSettings.FindAssetEntry(assetGuid);
			if (existingEntry is not null)
			{
				return existingEntry.parentGroup.Name;
			}
			
			var start = storyPrefix.Length;
			var end = assetPath.IndexOf('/', start);
			var storyId = end < 0 ? assetPath[start..] : assetPath[start..end];
			return storyId;

		}

	}
}