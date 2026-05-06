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
		protected override string AssetPath => $"{editorStoryInfo.storyPaths.dialogFilesFolder}/{id}.asset";
		protected override string AddressableName => $"Dialogs-{id}";

		protected override void BeforeSave()
		{
			base.BeforeSave();
			var key = $"dialogs.{id}";
			SetupLocalizedProperty(CreatedAsset.dialogFile, key);
			editorStoryInfo.storyInfo.dialogs.Add(CreatedAsset);
			
			foreach (var localeTable in editorStoryInfo.assetTable.AssetTables)
			{
				var fileName = $"{id}_{localeTable.LocaleIdentifier.Code}.dialog";
				var filePath = $"{editorStoryInfo.storyPaths.dialogFilesFolder}/{fileName}";
				
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