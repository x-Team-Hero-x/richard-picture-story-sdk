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
		public new T CreatedAsset => (T)base.CreatedAsset;
		public new T CreateAsset() => (T)base.CreateAsset();
		
		protected sealed override InformationAsset CreateEmptyInstance() => CreateInstance<T>();

		private const string TypeSuffix = "Info";
		private static readonly string TypeName = typeof(T).Name is var name && name.EndsWith(TypeSuffix)
			? name[..^TypeSuffix.Length]
			: name;
		protected string ParentFolderRelativePath => $"{TypeName}s"; 
		protected override string RelativeAssetPath => $"{ParentFolderRelativePath}/{id}.asset";
	}
	
	public abstract class StoryAssetCreatorBase : ScriptableWizard
	{
		[HideInInspector] public required EditorStoryInfo editorStoryInfo;
		protected InformationAsset CreatedAsset = null!;
		public string id = null!;
		private string _assetPath = null!;
		
		protected abstract InformationAsset CreateEmptyInstance();
		protected abstract string IdExample { get; }
		protected abstract string RelativeAssetPath { get; }
		
		protected virtual void OnEnable()
		{
			CreatedAsset = CreateEmptyInstance();
			id = IdExample;
			OnValidate();
		}

		protected virtual void OnValidate()
		{

		}

		protected virtual void OnBeforeCreate()
		{
			// Validate id
			id = id.Trim();
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Field 'id' can not be empty", nameof(id));
			}
			CreatedAsset.id = id;

			// Validate asset path
			_assetPath = editorStoryInfo.GetAssetPath(RelativeAssetPath);
			if (AssetDatabase.AssetPathExists(_assetPath))
			{
				throw new ArgumentException($"Asset '{_assetPath}' already exists", nameof(id));
			}
		}

		protected void OnWizardCreate()
		{
			CreateAsset();
			RevealAsset();
		}

		public InformationAsset CreateAsset()
		{
			// Custom callback - check inputs, fill asset values, etc
			try
			{
				OnBeforeCreate();
			}
			catch (ArgumentException exception)
			{
				EditorUtility.DisplayDialog($"Wrong value for '{exception.ParamName}'", exception.Message, "OK");
				throw;
			}

			// Make asset persistent
			var (assetParentFolder, _) = Paths.SplitPath(_assetPath);
			Paths.EnsureFolderExists(assetParentFolder);
			AssetDatabase.CreateAsset(CreatedAsset, _assetPath);
			
			// Add asset to story
			editorStoryInfo.storyInfo.informationAssets.Add(CreatedAsset);
			EditorUtility.SetDirty(editorStoryInfo.storyInfo);
			
			// Save assets and return
			AssetDatabase.SaveAssets();
			return CreatedAsset;
		}

		private void RevealAsset()
		{
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
			EditorUtility.SetDirty(table.SharedData);
		}
	}
}