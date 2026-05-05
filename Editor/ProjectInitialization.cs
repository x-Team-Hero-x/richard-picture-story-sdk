using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Localization;
using UnityEditor.Localization.Addressables;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	internal static class ProjectInitialization
	{
		internal static readonly Locale DefaultLocale = AssetDatabase.LoadAssetByGUID<Locale>(new GUID("a2cc46532a516b6418d698f9a6c5e3f4"));
		private static readonly string DialogOptOutKey = typeof(Menu).FullName!;
		
		internal static void AskAndInitialize()
		{
			var doInitialize =
				EditorUtility.DisplayDialog(
					"Are you sure?",
					"Initializing a project for work with StorySDK can break stuff. " +
					"It is recommended to use it only on empty projects. " +
					"Do you want to continue? ",
					"Initialize StorySDK", "Cancel",
					DialogOptOutDecisionType.ForThisSession, DialogOptOutKey);
			if (doInitialize)
			{
				ProjectInitialization.Initialize();	
			}
		}
		
		private static void Initialize()
		{
			CreateAssetFolders();
			CreateAddressableSettings();
			CreateLocalizationSettings();
			RegisterLocales();
			CreateGroupNameResolver();
		}
		
		private static void CreateAssetFolders()
		{
			Paths.EnsureFolderExists(Paths.SdkFolder);
			Paths.EnsureFolderExists(Paths.StoriesFolder);
		}
		
		private static void CreateAddressableSettings()
		{
			_ = AddressableAssetSettingsDefaultObject.GetSettings(true);
		}
		
		private static void CreateLocalizationSettings()
		{
			if (LocalizationEditorSettings.ActiveLocalizationSettings is not null)
			{
				return;
			}
			var settings = ScriptableObject.CreateInstance<LocalizationSettings>();
			settings.SetSelectedLocale(DefaultLocale);
			AssetDatabase.CreateAsset(settings, Paths.LocalizationSettingsAsset);
			LocalizationEditorSettings.ActiveLocalizationSettings = settings;
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