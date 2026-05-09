using System;
using System.IO;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
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

		private const string TypeSuffix = "Info";
		private static readonly string TypeName = typeof(T).Name is var name && name.EndsWith(TypeSuffix)
			? name[..^TypeSuffix.Length]
			: name;
		
		protected override string RelativeAssetPath => $"{TypeName}s/{id}.asset";
		protected override string AddressableName => $"{TypeName}-{id}";
	}
	
	public abstract class StoryAssetCreatorBase : ScriptableWizard
	{
		[HideInInspector] public required EditorStoryInfo editorStoryInfo;
		protected InformationAsset CreatedAsset = null!;
		public string id = null!;
		
		protected abstract InformationAsset CreateEmptyInstance();
		protected abstract string IdExample { get; }
		protected abstract string RelativeAssetPath { get; }
		protected string AssetPath => editorStoryInfo.GetAssetPath(RelativeAssetPath);
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
				EditorUtility.DisplayDialog($"Wrong value for '{exception.ParamName}'", exception.Message, "OK");
				throw;
			}

			// Create asset
			BeforeSave();
			Paths.EnsureFolderExists(AssetPath[..AssetPath.LastIndexOf('/')]);
			AssetDatabase.CreateAsset(CreatedAsset, AssetPath);

			// Make asset addressable
			var path = AssetDatabase.GetAssetPath(CreatedAsset);
			MakeAddressable(path, AddressableName);

			// Select newly created object
			EditorApplication.delayCall += () =>
			{
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = CreatedAsset;
				EditorGUIUtility.PingObject(CreatedAsset);
			};
		}

		protected void MakeAddressable(string assetPath, string addressableName)
		{
			var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
			var assetGuid = AssetDatabase.AssetPathToGUID(assetPath);
			var storyInfoEntry = addressableSettings.CreateOrMoveEntry(assetGuid, editorStoryInfo.addressableGroup, true);
			storyInfoEntry.address = addressableName;
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