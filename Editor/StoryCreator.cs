using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	public class StoryCreator : ScriptableWizard
	{
		public required string id = "com.example.story";
		public required List<Locale> locales = new();

		private static void NewLocalized(LocalizedReference reference, LocalizationTableCollection table, string key)
		{
			var entry = table.SharedData.AddKey(key);
			reference.SetReference(table.TableCollectionNameReference, entry.Key);
		}

		private void OnWizardCreate()
		{
			if (AssetDatabase.IsValidFolder($"Assets/{id}"))
			{
				throw new ArgumentException($"Folder 'Assets/{id}' already exists", nameof(id));
			}
			
			locales = locales.Distinct().ToList();
			
			var storyFolderGuid = AssetDatabase.CreateFolder("Assets", id);
			var storyInfo = CreateInstance<StoryInfo>();
			
			var localizationFolderGuid = AssetDatabase.CreateFolder($"Assets/{id}", "Localization");
			var strings = LocalizationEditorSettings.CreateStringTableCollection("strings", $"Assets/{id}/Localization", locales);
			var assets = LocalizationEditorSettings.CreateAssetTableCollection("assets", $"Assets/{id}/Localization", locales);

			NewLocalized(storyInfo.icon, assets, "info.icon");
			NewLocalized(storyInfo.title, strings, "info.title");
			NewLocalized(storyInfo.description, strings, "info.description");
			AssetDatabase.CreateAsset(storyInfo, $"Assets/{id}/StoryInfo.asset");
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

		private void OnEnable()
		{
			OnValidate();
		}
	}
}