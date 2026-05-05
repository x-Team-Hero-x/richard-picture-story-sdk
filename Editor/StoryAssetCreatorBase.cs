using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	[SuppressMessage("ReSharper", "Unity.RedundantEventFunction")]
	public abstract class StoryAssetCreatorBase<T> : ScriptableWizard where T : StoryAssetBase
	{
		[HideInInspector] public required EditorStoryInfo editorStoryInfo;
		protected T CreatedAsset = null!;
		public string id = null!;

		protected abstract string IdExample { get; }
		protected abstract string AssetPath { get; }
		protected abstract string AddressableName { get; }

		protected virtual void OnEnable()
		{
			CreatedAsset = CreateInstance<T>();
			id = IdExample;
			OnValidate();
		}

		protected virtual void OnValidate()
		{

		}

		protected virtual void CheckInputs()
		{
			id = id.Trim();
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Field 'id' can not be empty", nameof(id));
			}

			if (AssetDatabase.AssetPathExists(AssetPath))
			{
				throw new ArgumentException($"Asset '{AssetPath}' already exists", nameof(id));
			}
		}

		protected virtual void OnWizardCreate()
		{
			// Check inputs
			try
			{
				CheckInputs();
			}
			catch (ArgumentException exception)
			{
				EditorUtility.DisplayDialog($"Wrong value fo '{exception.ParamName}'", exception.Message, "OK");
				throw;
			}

			// Create asset
			BeforeSave();
			AssetDatabase.CreateAsset(CreatedAsset, AssetPath);

			// Make asset addressable
			var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
			var path = AssetDatabase.GetAssetPath(CreatedAsset);
			var assetGuid = AssetDatabase.AssetPathToGUID(path);
			var storyInfoEntry =
				addressableSettings.CreateOrMoveEntry(assetGuid, editorStoryInfo.addressableGroup, true);
			storyInfoEntry.address = AddressableName;

			// Select newly created object
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = CreatedAsset;
			EditorGUIUtility.PingObject(CreatedAsset);
		}

		protected virtual void BeforeSave()
		{
			CreatedAsset.id = id;
		}

		protected void SetupLocalizedProperty(LocalizedReference reference, string key)
		{
			LocalizationTableCollection table = reference is LocalizedString
				? editorStoryInfo.stringTable
				: editorStoryInfo.assetTable;
			var entry = table.SharedData.AddKey(key);
			reference.SetReference(table.TableCollectionNameReference, entry.Key);
		}
	}
}