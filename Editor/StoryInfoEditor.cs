using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	[CustomEditor(typeof(StoryInfo))]
	[CanEditMultipleObjects]
	public class StoryInfoEditor : UnityEditor.Editor
	{
		[SerializeField] private bool isControlsFoldoutOpen = true;
		
		public override void OnInspectorGUI()
		{
			var storyInfo = (StoryInfo)target;
			isControlsFoldoutOpen = EditorGUILayout.Foldout(isControlsFoldoutOpen, "Controls");
			if (isControlsFoldoutOpen)
			{
				if (GUILayout.Button("Package and Export"))
				{
					EditorApplication.delayCall += () => BuildStory(storyInfo);
				}
			}
			base.OnInspectorGUI();
		}

		private static void BuildStory(StoryInfo storyInfo)
		{
			var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
			foreach (var addressableGroup in addressableSettings.groups)
			{
				var schema = addressableGroup.GetSchema<BundledAssetGroupSchema>();
				schema.IncludeInBuild = addressableGroup.Name == storyInfo.id;
			}
			
			Debug.Log($"Building story '{storyInfo.id}'...");
			AddressableAssetSettings.BuildPlayerContent(out var buildResult);
			if (!string.IsNullOrEmpty(buildResult.Error))
			{
				Debug.LogError($"Story build for '{storyInfo.id}' failed after {buildResult.Duration}. {buildResult.Error}.");
			}
			Debug.Log($"Story '{storyInfo.id}' was built successfully");
		}
	}
}