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
		public EditorStoryInfo EditorStoryInfo => (EditorStoryInfo)target;
		public StoryInfo StoryInfo => EditorStoryInfo.storyInfo;
		
		public override void OnInspectorGUI()
		{
			Button("Package and Export", BuildStory);
			Button("Add character", AddCharacter);
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

		private void AddCharacter()
		{
			var characterCreator = ScriptableWizard.DisplayWizard<CharacterCreator>($"Add a character to {StoryInfo.id}");
			characterCreator.editorStoryInfo = EditorStoryInfo;
		}
	}
}