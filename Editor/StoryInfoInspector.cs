using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	[CustomEditor(typeof(EditorStoryInfo))]
	[CanEditMultipleObjects]
	public class StoryInfoInspector : UnityEditor.Editor
	{
		private EditorStoryInfo EditorStoryInfo => (EditorStoryInfo)target;
		private StoryInfo StoryInfo => EditorStoryInfo.storyInfo;
		
		public override void OnInspectorGUI()
		{
			Button("Package and Export", BuildStory);
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
					var creator = ScriptableWizard.DisplayWizard<TCreator>($"Add a {assetKind} to '{StoryInfo.id}'");
					creator.editorStoryInfo = EditorStoryInfo;
				}
			);
		}

		private void BuildStory()
		{
			var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
			foreach (var addressableGroup in addressableSettings.groups)
			{
				var schema = addressableGroup.GetSchema<BundledAssetGroupSchema>();
				schema.IncludeInBuild = addressableGroup.Name == StoryInfo.id;
			}
			
			Debug.Log($"Building story '{StoryInfo.id}'...");
			AddressableAssetSettings.BuildPlayerContent(out var buildResult);
			if (!string.IsNullOrEmpty(buildResult.Error))
			{
				Debug.LogError($"Story build for '{StoryInfo.id}' failed after {buildResult.Duration}. {buildResult.Error}.");
			}
			Debug.Log($"Story '{StoryInfo.id}' was built successfully");
		}
	}
}