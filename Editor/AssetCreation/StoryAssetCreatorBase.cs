using System;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation
{
	public abstract class StoryAssetCreatorBase<T> : StoryAssetCreatorBase where T : InformationAsset
	{
		protected new T CreatedAsset => (T)base.CreatedAsset;
		protected sealed override InformationAsset CreateEmptyInstance() => CreateInstance<T>();
	}
	
	public abstract class StoryAssetCreatorBase : ScriptableWizard
	{
		[HideInInspector] public required EditorStoryInfo editorStoryInfo;
		protected InformationAsset CreatedAsset = null!;
		public string id = null!;
		
		protected abstract InformationAsset CreateEmptyInstance();
		protected abstract string IdExample { get; }
		protected abstract string AssetPath { get; }
		protected abstract string AddressableName { get; }
		
		protected virtual void OnEnable()
		{
			CreatedAsset = CreateEmptyInstance();
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

		protected virtual void BeforeSave()
		{
			CreatedAsset.id = id;
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
			EditorApplication.delayCall += () =>
			{
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = CreatedAsset;
				EditorGUIUtility.PingObject(CreatedAsset);
			};
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