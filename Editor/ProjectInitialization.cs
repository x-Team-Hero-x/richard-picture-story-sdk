using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Localization;
using UnityEditor.Localization.Addressables;
using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	internal static class ProjectInitialization
	{
		internal static void Initialize()
		{
			CreateAddressableSettings();
			CreateAssetFolders();
			CreateGroupNameResolver();
			RegisterLocales();
		}
		
		private static void CreateAddressableSettings()
		{
			_ = AddressableAssetSettingsDefaultObject.GetSettings(true);
		}
		
		private static void RegisterLocales()
		{
			var localesFolderPath = AssetDatabase.GUIDToAssetPath("68646bc326eee864b97b17756c74aada");
			var localeGuids = AssetDatabase.FindAssetGUIDs($"t:{nameof(Locale)}", new[] { localesFolderPath });
			foreach (var localeGuid in localeGuids)
			{
				var locale = AssetDatabase.LoadAssetByGUID<Locale>(localeGuid);
				LocalizationEditorSettings.AddLocale(locale);
			}
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}
		
		private static void CreateAssetFolders()
		{
			Paths.EnsureFolderExists(Paths.SdkFolder);
			Paths.EnsureFolderExists(Paths.StoriesFolder);
		}
		
		private static void CreateGroupNameResolver()
		{
			if (AssetDatabase.AssetPathExists(Paths.GroupNameResolver))
			{
				return;
			}
			
			var instance = ScriptableObject.CreateInstance<AddressableGroupRules>();
			var resolver = new PerStoryLocalizationResolver();
			instance.LocaleResolver = resolver;
			instance.AssetTablesResolver = resolver;
			instance.AssetResolver = resolver;
			instance.StringTablesResolver = resolver;

			AssetDatabase.CreateAsset(instance, Paths.GroupNameResolver);
			AddressableGroupRules.Instance = instance;
		}
	}
}