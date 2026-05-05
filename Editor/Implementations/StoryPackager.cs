using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Build;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Implementations
{
	public static class StoryPackager
	{
		private static readonly string BuildFailedDialogKey = $"{typeof(StoryPackager).FullName}.{nameof(BuildFailedDialogKey)}";
		private static readonly string BuildSucceededDialogKey = $"{typeof(StoryPackager).FullName}.{nameof(BuildSucceededDialogKey)}";
	
		public static void Package(string storyId)
		{
			try
			{
				AddressablesBuilder.Build(storyId);
			}
			catch (BuildFailedException exception)
			{
				EditorUtility.DisplayDialog(
					"Story build failed",
					$"Story packaging for '{storyId}' failed\n\n{exception.Message}",
					"Ok", DialogOptOutDecisionType.ForThisUser, BuildFailedDialogKey);
				throw;
			}
			Debug.Log($"Story '{storyId}' was packaged successfully");
			EditorUtility.DisplayDialog(
				"Story build success", 
				$"Story '{storyId}' was packaged successfully",
				"Ok", DialogOptOutDecisionType.ForThisUser, BuildSucceededDialogKey);
		}

		private static class AddressablesBuilder
		{
			public static void Build(string storyId)
			{
				Debug.Log($"Building addressables for story '{storyId}'...");
				var schemas = GetSchemas();
				var originalFlags = CollectOriginalFlags(schemas);
				try
				{
					UpdateFlagsForId(schemas, storyId);
					BuildAddressables(storyId);
				}
				finally
				{
					RestoreOriginalFlags(originalFlags);
				}
			}
		
			private static List<BundledAssetGroupSchema> GetSchemas()
			{
				return AddressableAssetSettingsDefaultObject
					.Settings
					.groups
					.Select(addressableGroup => addressableGroup.GetSchema<BundledAssetGroupSchema>())
					.ToList();
			}

			private static Dictionary<BundledAssetGroupSchema, bool> CollectOriginalFlags(IEnumerable<BundledAssetGroupSchema> schemas)
			{
				return schemas.ToDictionary(schema => schema, schema => schema.IncludeInBuild);
			}

			private static void UpdateFlagsForId(IEnumerable<BundledAssetGroupSchema> schemas, string storyId)
			{
				foreach (var schema in schemas)
				{
					schema.IncludeInBuild = schema.Group.Name == storyId;
				}
			}

			private static void BuildAddressables(string storyId)
			{
				AddressableAssetSettings.BuildPlayerContent(out var buildResult);
				if (!string.IsNullOrEmpty(buildResult.Error))
				{
					throw new BuildFailedException(buildResult.Error);
				}
			}

			private static void RestoreOriginalFlags(Dictionary<BundledAssetGroupSchema, bool> originalFlags)
			{
				foreach (var (addressableAssetSchema, originalValue) in originalFlags)
				{
					addressableAssetSchema.IncludeInBuild = originalValue;
				}
			}
		}
	}
}