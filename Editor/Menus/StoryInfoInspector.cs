using HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using HeroTeam.RichardPicture.StorySdk.InformationAssets;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Menus
{
	[CustomEditor(typeof(EditorStoryInfo))]
	[CanEditMultipleObjects]
	public class StoryInfoInspector : UnityEditor.Editor
	{
		private EditorStoryInfo EditorStoryInfo => (EditorStoryInfo)target;
		private string StoryId => EditorStoryInfo.storyInfo.id;
		
		public override void OnInspectorGUI()
		{
			Button("Package and Export", () => StoryPackager.Package(StoryId));
			CreatorButton<CharacterCreator>("character");
			// CreatorButton<DialogCreator>("dialog");
			EditorGUILayout.Space();
			base.OnInspectorGUI();
		}

		private static void Button(string buttonText, EditorApplication.CallbackFunction action)
		{
			if (GUILayout.Button(buttonText))
			{
				EditorApplication.delayCall += action;
			}
		}

		private void CreatorButton<TCreator>(string assetKind) where TCreator: StoryAssetCreatorBase
		{
			Button(
				$"Add a {assetKind}",
				() =>
				{
					var creator = ScriptableWizard.DisplayWizard<TCreator>($"Add a {assetKind} to '{StoryId}'");
					creator.editorStoryInfo = EditorStoryInfo;
				}
			);
		}
	}
}