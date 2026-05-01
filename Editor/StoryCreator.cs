using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Localization;
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
			var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
			
			// Check inputs
			if (AssetDatabase.IsValidFolder($"Assets/StorySDK/Stories/{id}"))
			{
				throw new ArgumentException($"Folder 'Assets/StorySDK/Stories/{id}' already exists", nameof(id));
			}
			if (addressableSettings.FindGroup(id) is not null)
			{
				throw new ArgumentException($"Addressable group '{id}' already exists", nameof(id));
			}
			locales = locales.Distinct().ToList();
			
			// create template structure
			var storyFolderGuid = AssetDatabase.CreateFolder("Assets/StorySDK/Stories", id);
			var localizationFolderGuid = AssetDatabase.CreateFolder($"Assets/StorySDK/Stories/{id}", "Localization");
			var strings = LocalizationEditorSettings.CreateStringTableCollection("strings", $"Assets/StorySDK/Stories/{id}/Localization", locales);
			var assets = LocalizationEditorSettings.CreateAssetTableCollection("assets", $"Assets/StorySDK/Stories/{id}/Localization", locales);

			// create main manifest asset
			var storyInfo = CreateInstance<StoryInfo>();
			storyInfo.id = id;
			NewLocalized(storyInfo.icon, assets, "info.icon");
			NewLocalized(storyInfo.title, strings, "info.title");
			NewLocalized(storyInfo.description, strings, "info.description");
			AssetDatabase.CreateAsset(storyInfo, $"Assets/StorySDK/Stories/{id}/StoryInfo.asset");
			
			// manage addressables
			var addressableGroup = addressableSettings.CreateGroup(id, false, true, false, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
			var path = AssetDatabase.GetAssetPath(storyInfo);
			var guid = AssetDatabase.AssetPathToGUID(path);
			addressableSettings.CreateOrMoveEntry(guid, addressableGroup, true);
		}

		private static void NewLocalized(LocalizedReference reference, LocalizationTableCollection table, string key)
		{
			var entry = table.SharedData.AddKey(key);
			reference.SetReference(table.TableCollectionNameReference, entry.Key);
		}
	}
}