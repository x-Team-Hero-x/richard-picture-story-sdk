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
			// Calculate inferred properties
			var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
			var storyPaths = Paths.GetStoryPaths(id);
			
			// Check inputs
			if (AssetDatabase.IsValidFolder(storyPaths.StoryFolder))
			{
				throw new ArgumentException($"Folder '{storyPaths.StoryFolder}' already exists", nameof(id));
			}
			if (addressableSettings.FindGroup(id) is not null)
			{
				throw new ArgumentException($"Addressable group '{id}' already exists", nameof(id));
			}
			locales = locales.Distinct().ToList();
			
			// Create template structure
			foreach (var storySubfolder in storyPaths.AllFolders)
			{
				Paths.EnsureFolderExists(storySubfolder);
			}
			var addressableGroup = addressableSettings.CreateGroup(id, false, true, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
			var strings = LocalizationEditorSettings.CreateStringTableCollection(storyPaths.StringsTable, storyPaths.LocalizationFolder, locales);
			var assets = LocalizationEditorSettings.CreateAssetTableCollection(storyPaths.AssetsTable, storyPaths.LocalizationFolder, locales);
			
			// Create main manifest asset
			var storyInfo = CreateInstance<StoryInfo>();
			storyInfo.id = id;
			NewLocalized(storyInfo.icon, assets, "info.icon");
			NewLocalized(storyInfo.title, strings, "info.title");
			NewLocalized(storyInfo.description, strings, "info.description");
			AssetDatabase.CreateAsset(storyInfo, storyPaths.StoryInfoAsset);
			var storyInfoEntry = addressableSettings.CreateOrMoveEntry(Paths.GetAssetGuidString(storyInfo), addressableGroup, true);
			storyInfoEntry.address = "StoryInfo";
			
			// Select newly created object
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = storyInfo;
			EditorGUIUtility.PingObject(storyInfo);
		}

		private static void NewLocalized(LocalizedReference reference, LocalizationTableCollection table, string key)
		{
			var entry = table.SharedData.AddKey(key);
			reference.SetReference(table.TableCollectionNameReference, entry.Key);
		}
	}
}