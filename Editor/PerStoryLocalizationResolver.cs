using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Localization.Addressables;
using UnityEngine.Localization;
using Object = UnityEngine.Object;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	public class PerStoryLocalizationResolver : GroupResolver
	{
		public override string GetExpectedGroupName(IList<LocaleIdentifier> locales, Object asset, AddressableAssetSettings aaSettings)
		{
			var path = AssetDatabase.GetAssetPath(asset);
			return asset switch
			{
				Locale => "Locales",
				_ when path.StartsWith("Assets/StorySDK/Stories/") => path.Split("/")[3],
				_ => base.GetExpectedGroupName(locales, asset, aaSettings)
			};
		}

	}
}