using HeroTeam.RichardPicture.StorySdk.Editor.AssetCreation;
using HeroTeam.RichardPicture.StorySdk.Editor.Implementations;
using UnityEditor;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Menus
{
	[CustomEditor(typeof(EditorStoryInfo))]
	[CanEditMultipleObjects]
	public class StoryInfoInspector : UnityEditor.Editor
	{
		private EditorStoryInfo EditorStoryInfo => (EditorStoryInfo)target;
		
		public override void OnInspectorGUI()
		{
			Button("Package and Export", () => StoryPackager.Package(EditorStoryInfo));
			CreatorButton<CharacterCreator>("character");
			CreatorButton<DialogCreator>("dialog");
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
					var creator = ScriptableWizard.DisplayWizard<TCreator>($"Add a {assetKind} to '{EditorStoryInfo.storyInfo.id}'");
					creator.editorStoryInfo = EditorStoryInfo;
				}
			);
		}
	}
}