using System;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace HeroTeam.RichardPicture.StorySdk.Editor.Implementations
{
	[ScriptedImporter(7, "dialog")]
	public class DialogImporter : ScriptedImporter
	{
		private static readonly Lazy<Texture2D> TextAssetIcon =
			new(() => (Texture2D)EditorGUIUtility.IconContent("TextAsset Icon").image);
		
		public override void OnImportAsset(AssetImportContext context)
		{
			var fileContents = File.ReadAllText(context.assetPath);
			var textAsset = new TextAsset(fileContents);
			context.AddObjectToAsset("text", textAsset, TextAssetIcon.Value);
			context.SetMainObject(textAsset);
		}
	}
}