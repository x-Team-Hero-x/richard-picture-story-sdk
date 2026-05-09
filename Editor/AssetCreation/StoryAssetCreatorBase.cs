using System;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
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
		protected abstract void BeforeSave();
		
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
			CreatedAsset.id = id;

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
				EditorUtility.DisplayDialog($"Wrong value for '{exception.ParamName}'", exception.Message, "OK");
				throw;
			}

			// Create asset
			BeforeSave();
			var (assetParentFolder, _) = Paths.SplitPath(AssetPath);
			Paths.EnsureFolderExists(assetParentFolder);
			AssetDatabase.CreateAsset(CreatedAsset, AssetPath);

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