using System.Linq;
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
			CreatorButton<BackgroundCreator>("background");
			CreatorButton<EffectCreator>("effect");
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
			var article = GetArticle(assetKind);
			Button(
				$"Add {article} {assetKind}",
				() =>
				{
					var creator = ScriptableWizard.DisplayWizard<TCreator>($"Add {article} {assetKind} to '{EditorStoryInfo.storyInfo.id}'");
					creator.editorStoryInfo = EditorStoryInfo;
				}
			);
		}
		
		private string GetArticle(string word)
		{
			var vowels = new[] { 'A', 'E', 'I', 'O', 'U' };
			var firstLetterCapitalized = char.ToUpper(word[0]);
			var doesBeginWithVowel = vowels.Contains(firstLetterCapitalized);
			return doesBeginWithVowel ? "an" : "a";
		}
	}
}