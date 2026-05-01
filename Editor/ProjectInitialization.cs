using System;
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
			EnsureFolderExists("Assets/StorySDK");
			EnsureFolderExists("Assets/StorySDK/Stories");
		}

		private static void EnsureFolderExists(string folderPath)
		{
			if (!folderPath.StartsWith("Assets/"))
			{
				throw new ArgumentException("Folder should start with 'Assets/'", nameof(folderPath));
			}
			
			var lastSlashIndex = folderPath.LastIndexOf('/');
			var parentPath = folderPath[..lastSlashIndex];
			var folderName = folderPath[(lastSlashIndex+1)..];
			if (!AssetDatabase.IsValidFolder(folderPath))
			{
				AssetDatabase.CreateFolder(parentPath, folderName);
			}
		}
		
		private static void CreateGroupNameResolver()
		{
			const string path = "Assets/StorySDK/GroupNameResolver.asset";
			if (AssetDatabase.AssetPathExists(path))
			{
				return;
			}
			
			var instance = ScriptableObject.CreateInstance<AddressableGroupRules>();
			var resolver = new PerStoryLocalizationResolver();
			instance.LocaleResolver = resolver;
			instance.AssetTablesResolver = resolver;
			instance.AssetResolver = resolver;
			instance.StringTablesResolver = resolver;

			AssetDatabase.CreateAsset(instance, path);
			AddressableGroupRules.Instance = instance;
		}
	}
}