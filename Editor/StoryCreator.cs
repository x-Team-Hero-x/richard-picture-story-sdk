using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Localization;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	public class StoryCreator : ScriptableWizard
	{
		public required string id = "com.example.story";
		public required List<Locale> locales = new();

		private void OnEnable()
		{
			OnValidate();
		}

		private void OnValidate()
		{
			locales = locales.Where(locale => locale is not null).ToList();
			if (locales.Count == 0)
			{
				var englishLocaleGuid = new GUID("a2cc46532a516b6418d698f9a6c5e3f4");
				var englishLocale = AssetDatabase.LoadAssetByGUID<Locale>(englishLocaleGuid);
				locales.Add(englishLocale);
			}
		}

		private void OnWizardCreate()
		{
			// Calculate inferred properties
			var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
			var storyPaths = Paths.GetStoryPaths(id);
			
			// Check inputs
			if (AssetDatabase.IsValidFolder(storyPaths.storyFolder))
			{
				throw new ArgumentException($"Folder '{storyPaths.storyFolder}' already exists", nameof(id));
			}
			if (addressableSettings.FindGroup(id) is not null)
			{
				throw new ArgumentException($"Addressable group '{id}' already exists", nameof(id));
			}
			locales = locales.Distinct().ToList();
			
			// Create template structure
			Paths.EnsureFolderExists(storyPaths.storyFolder);
			Paths.EnsureFolderExists(storyPaths.localizationFolder);
			var addressableGroup = addressableSettings.CreateGroup(storyPaths.mainGroup, false, true, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
			var strings = LocalizationEditorSettings.CreateStringTableCollection(storyPaths.stringsTable, storyPaths.localizationFolder, locales);
			var assets = LocalizationEditorSettings.CreateAssetTableCollection(storyPaths.assetsTable, storyPaths.localizationFolder, locales);
			
			// Create main manifest asset
			var storyInfo = CreateInstance<StoryInfo>();
			storyInfo.id = id;
			NewLocalized(storyInfo.icon, assets, "info.icon");
			NewLocalized(storyInfo.title, strings, "info.title");
			NewLocalized(storyInfo.description, strings, "info.description");
			AssetDatabase.CreateAsset(storyInfo, storyPaths.storyInfoAsset);
			addressableSettings.CreateOrMoveEntry(Paths.GetAssetGuidString(storyInfo), addressableGroup, true);
		}

		private static void NewLocalized(LocalizedReference reference, LocalizationTableCollection table, string key)
		{
			var entry = table.SharedData.AddKey(key);
			reference.SetReference(table.TableCollectionNameReference, entry.Key);
		}
	}
}