using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace HeroTeam.RichardPicture.StorySdk.Editor
{
	internal class StoryChecker
	{
		private static readonly string CheckStoryKey = typeof(StoryChecker).FullName!;

		internal static async Task CheckStory()
		{
			var path = RequestStoryPath();
			if (EditorApplication.isPlaying)
			{
				await CheckStoryInPlayMode(path);
			}
			else
			{
				SessionState.SetString(CheckStoryKey, path);
				EditorApplication.EnterPlaymode();
			}
		}

		[RuntimeInitializeOnLoadMethod]
		private static async void CheckRequest()
		{
			var path = SessionState.GetString(CheckStoryKey, null);
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			SessionState.EraseString(CheckStoryKey);
			await CheckStoryInPlayMode(path);
			EditorApplication.ExitPlaymode();
		}

		private static string RequestStoryPath()
		{
			var path = EditorUtility.OpenFilePanel("Select story bundle", Paths.PackagedStoriesFolder, "json,bin");
			return !string.IsNullOrEmpty(path)
				? path
				: throw new OperationCanceledException("Story was not selected");
		}

		private static async Task CheckStoryInPlayMode(string path)
		{
			Debug.Log($"Checking story at '{path}'...");
			await LocalizationSettings.InitializationOperation.Task;
			using var storyInfo = await StoryInfo.FromStoryFile(path);
			Debug.Log($"Story checked, got id '{storyInfo.id}' and {storyInfo.characters.Count} character(s).");
			var character0 = storyInfo.characters[0];
			var id0 = character0.id;
			var name0 = await character0.displayName.GetLocalizedStringAsync().Task;
			Debug.Log($"0th character id={id0} and name={name0}");
		}
	}
}