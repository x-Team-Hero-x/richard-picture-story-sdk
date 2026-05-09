using System.IO;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation
{
	public class DialogCreator : StoryAssetCreatorBase<DialogInfo>
	{
		protected override string IdExample => "test_dialog";

		protected override void BeforeSave()
		{
			base.BeforeSave();
			var key = $"dialogs.{id}";
			SetupLocalizedProperty(CreatedAsset.dialogFile, key);
			editorStoryInfo.storyInfo.dialogs.Add(CreatedAsset);
			
			foreach (var localeTable in editorStoryInfo.assetTable.AssetTables)
			{
				var filePath = editorStoryInfo.GetAssetPath($"{id}_{localeTable.LocaleIdentifier.Code}.dialog");
				
				// TODO: Creating a lot of files for a lot of languages might take a long time
				// TODO: Use async versions for File.WriteAllText and AssetDatabase.ImportAsset
				File.WriteAllText(filePath, string.Empty);
				AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceSynchronousImport);

				var fileAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
				editorStoryInfo.assetTable.AddAssetToTable(localeTable, key, fileAsset);
			}
		}
	}
}