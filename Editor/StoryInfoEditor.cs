using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	[CustomEditor(typeof(StoryInfo))]
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
			var originalSettings = AddressableAssetSettingsDefaultObject.Settings;
			var filteredSettings = Instantiate(originalSettings);
			
			var entries = filteredSettings.groups.SelectMany(group => group.entries).ToArray();
			var dependencyPaths = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(storyInfo)).ToHashSet();
			foreach (var entry in entries)
			{
				if (!dependencyPaths.Contains(entry.AssetPath))
				{
					entry.parentGroup.RemoveAssetEntry(entry);
				}
			}

			var emptyGroups = filteredSettings.groups.Where(group => group.entries.Count == 0).ToArray();
			foreach (var group in emptyGroups)
			{
				filteredSettings.RemoveGroup(group);
			}
			
			try
			{
				Debug.Log($"Building story '{storyInfo.id}'...");
				AssetDatabase.CreateAsset(filteredSettings, "Assets/TemporaryAddressablesBuildSettings.asset");
				AddressableAssetSettingsDefaultObject.Settings = filteredSettings;
				AddressableAssetSettings.BuildPlayerContent(out var buildResult);
				Debug.Log($"Story '{storyInfo.id}' was built successfully in {buildResult.Duration}");
			}
			catch
			{
				Debug.Log($"Could not build story '{storyInfo.id}'");
				throw;
			}
			finally
			{
				AddressableAssetSettingsDefaultObject.Settings = originalSettings;
				AssetDatabase.DeleteAsset("Assets/TemporaryAddressablesBuildSettings.asset");
			}
		}
	}
}