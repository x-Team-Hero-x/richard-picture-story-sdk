using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Localization.Addressables;
using UnityEngine.Localization;
using Object = UnityEngine.Object;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Implementations
{
	[Serializable]
	public class PerStoryLocalizationResolver : GroupResolver
	{
		public override string GetExpectedGroupName(IList<LocaleIdentifier> locales, Object asset, AddressableAssetSettings aaSettings)
		{
			if (asset is Locale)
			{
				return "Locales";
			}
			
			var path = AssetDatabase.GetAssetPath(asset);
			const string storyPrefix = $"{Paths.StoriesFolder}/";
			if (path.StartsWith(storyPrefix))
			{
				var start = storyPrefix.Length;
				var end = path.IndexOf('/', start);
				var storyId = end < 0 ? path[start..] : path[start..end];
				return storyId;
			}

			return base.GetExpectedGroupName(locales, asset, aaSettings);
		}

	}
}