using System;
using System.Collections.Generic;
using System.Linq;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Localization;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation
{
	public class StoryCreator : StoryAssetCreatorBase<StoryInfo>
	{
		public List<Locale> locales = new();
		
		protected override string IdExample => "com.example.story";
		protected override string AssetPath => editorStoryInfo.storyPaths.storyInfoAsset;
		protected override string AddressableName => "StoryInfo";
		
		private static AddressableAssetSettings EditorAddressables => AddressableAssetSettingsDefaultObject.Settings;

		protected override void OnValidate()
		{
			base.OnValidate();
			locales = locales.Where(locale => locale is not null).ToList();
			if (locales.Count == 0)
			{
				locales.Add(ProjectInitialization.DefaultLocale);
			}
		}

		protected override void CheckInputs()
		{
			base.CheckInputs();
			if (EditorAddressables.FindGroup(id) is not null)
			{
				throw new ArgumentException($"Addressable group '{id}' already exists", nameof(id));
			}
			locales = locales.Distinct().ToList();
		}

		protected override void OnWizardCreate()
		{
			// Create editor asset
			editorStoryInfo = CreateInstance<EditorStoryInfo>();
			editorStoryInfo.storyPaths = new StoryPaths(id);

			base.OnWizardCreate();
			
			// Save editor asset
			editorStoryInfo.storyInfo = CreatedAsset;
			AssetDatabase.CreateAsset(editorStoryInfo, editorStoryInfo.storyPaths.editorStoryInfoAsset);
		}

		protected override void BeforeSave()
		{
			// Create folder structure
			foreach (var storySubfolder in editorStoryInfo.storyPaths.AllFolders)
			{
				Paths.EnsureFolderExists(storySubfolder);
			}
			
			// Create addressable stuff
			editorStoryInfo.addressableGroup = EditorAddressables.CreateGroup(id, false, true, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
			editorStoryInfo.stringTable = LocalizationEditorSettings.CreateStringTableCollection(editorStoryInfo.storyPaths.stringsTable, editorStoryInfo.storyPaths.localizationFolder, locales);
			editorStoryInfo.assetTable = LocalizationEditorSettings.CreateAssetTableCollection(editorStoryInfo.storyPaths.assetsTable, editorStoryInfo.storyPaths.localizationFolder, locales);
			
			// Fill properties
			base.BeforeSave();
			SetupLocalizedProperty(CreatedAsset.icon, "info.icon");
			SetupLocalizedProperty(CreatedAsset.title, "info.title");
			SetupLocalizedProperty(CreatedAsset.description, "info.description");
		}
	}
}