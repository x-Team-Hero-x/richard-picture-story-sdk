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
		protected override string RelativeAssetPath => "StoryInfo.asset";
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
			editorStoryInfo.storyInfo = CreatedAsset;

			base.OnWizardCreate();
			
			// Save editor asset
			AssetDatabase.CreateAsset(editorStoryInfo, editorStoryInfo.GetAssetPath("EditorStoryInfo.asset"));
		}

		protected override void BeforeSave()
		{
			// Create folder structure
			//TODO: ask user location instead of using hardcoded
			Paths.EnsureFolderExists($"{Paths.StoriesFolder}/{id}");
			Paths.EnsureFolderExists($"{Paths.StoriesFolder}/{id}/Assets");
			Paths.EnsureFolderExists($"{Paths.StoriesFolder}/{id}/Localization");
			Paths.EnsureFolderExists($"{Paths.StoriesFolder}/{id}/Characters");
			Paths.EnsureFolderExists($"{Paths.StoriesFolder}/{id}/Dialogs");
			
			// Call base method
			base.BeforeSave();
			
			// Create addressable stuff
			editorStoryInfo.addressableGroup = EditorAddressables.CreateGroup(id, false, true, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
			var localizationFolderPath = editorStoryInfo.GetAssetPath("Localization");
			editorStoryInfo.stringTable = LocalizationEditorSettings.CreateStringTableCollection($"{id}.strings", localizationFolderPath, locales);
			editorStoryInfo.assetTable = LocalizationEditorSettings.CreateAssetTableCollection($"{id}.assets", localizationFolderPath, locales);
			MakeAddressable(editorStoryInfo.GetAssetPath("Assets"), "Assets");
			
			// Fill properties
			SetupLocalizedProperty(CreatedAsset.icon, "info.icon");
			SetupLocalizedProperty(CreatedAsset.title, "info.title");
			SetupLocalizedProperty(CreatedAsset.description, "info.description");
		}
	}
}