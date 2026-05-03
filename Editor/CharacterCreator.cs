using System;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	public class CharacterCreator : ScriptableWizard
	{
		[HideInInspector] public required EditorStoryInfo editorStoryInfo;
		public string id = "test_character";

		private void OnWizardCreate()
		{
			var characterAssetPath = $"{editorStoryInfo.storyPaths.charactersFolder}/{id}.asset";
			
			// Check inputs
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Field 'id' can not be empty", nameof(id));
			}
			if (AssetDatabase.AssetPathExists(characterAssetPath))
			{
				throw new ArgumentException($"Asset '{characterAssetPath}' already exists", nameof(id));
			}
			id = id.Trim();
			
			//TODO: factor out common code with StoryCreator, including `NewLocalized()`
			// Create main manifest asset
			var characterInfo = CreateInstance<CharacterInfo>();
			characterInfo.id = id;
			NewLocalized(characterInfo.spritePrefab, editorStoryInfo.assetTable, $"characters.{id}.sprite");
			NewLocalized(characterInfo.displayName, editorStoryInfo.stringTable, $"characters.{id}.name");
			editorStoryInfo.storyInfo.characters.Add(characterInfo);
			AssetDatabase.CreateAsset(characterInfo, characterAssetPath);
			//TODO: mark addressable??
			
			// Select newly created object
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = characterInfo;
			EditorGUIUtility.PingObject(characterInfo);
		}

		private static void NewLocalized(LocalizedReference reference, LocalizationTableCollection table, string key)
		{
			var entry = table.SharedData.AddKey(key);
			reference.SetReference(table.TableCollectionNameReference, entry.Key);
		}
	}
}