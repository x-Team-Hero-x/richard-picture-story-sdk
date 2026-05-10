using System;
using System.Collections.Generic;
using System.Linq;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation
{
	public class StoryCreator : StoryAssetCreatorBase<StoryInfo>
	{
		public List<Locale> locales = new();
		public DefaultAsset parentFolder = null!;
		
		protected override string IdExample => "com.example.story";
		protected override string RelativeAssetPath => "StoryInfo.asset";

		protected override void OnValidate()
		{
			base.OnValidate();
			
			locales = locales.Where(locale => locale is not null).ToList();
			if (locales.Count == 0)
			{
				locales.Add(ProjectInitialization.DefaultLocale);
			}

			var parentPath = AssetDatabase.GetAssetPath(parentFolder);
			if (!AssetDatabase.IsValidFolder(parentPath))
			{
				parentFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>("Assets");
			}
		}

		protected override void OnBeforeCreate()
		{
			// Create editor asset
			editorStoryInfo = CreateInstance<EditorStoryInfo>();
			editorStoryInfo.storyInfo = CreatedAsset;
			editorStoryInfo.parentFolder = parentFolder;
			
			base.OnBeforeCreate();
			
			// Validate localization tables (strings)
			var stringTableKey = $"{id}.strings";
			var existingStringTable = LocalizationEditorSettings.GetStringTableCollection(stringTableKey);
			if (existingStringTable is not null)
			{
				throw new ArgumentException($"Localization table '{stringTableKey}' already exists", nameof(id));
			}
			
			// Validate localization tables (assets)
			var assetTableKey = $"{id}.assets";
			var existingAssetTable = LocalizationEditorSettings.GetAssetTableCollection(assetTableKey);
			if (existingAssetTable is not null)
			{
				throw new ArgumentException($"Localization table '{assetTableKey}' already exists", nameof(id));
			}
			
			// Create localization tables
			locales = locales.Distinct().ToList();
			var localizationFolderPath = editorStoryInfo.GetAssetPath("Localization");
			Paths.EnsureFolderExists(localizationFolderPath);
			editorStoryInfo.stringTable = LocalizationEditorSettings.CreateStringTableCollection(stringTableKey, localizationFolderPath, locales);
			editorStoryInfo.assetTable = LocalizationEditorSettings.CreateAssetTableCollection(assetTableKey, localizationFolderPath, locales);
			
			// Fill properties
			SetupLocalizedProperty(CreatedAsset.icon, "info.icon");
			SetupLocalizedProperty(CreatedAsset.title, "info.title");
			SetupLocalizedProperty(CreatedAsset.description, "info.description");
			
			// Save editor asset
			AssetDatabase.CreateAsset(editorStoryInfo, editorStoryInfo.GetAssetPath("EditorStoryInfo.asset"));
		}
	}
}