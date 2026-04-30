using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.Editor;

internal static class LocalesRegistration
{
	[InitializeOnLoadMethod]
	private static void RegisterLocales()
	{
		var localesFolderPath = AssetDatabase.GUIDToAssetPath("68646bc326eee864b97b17756c74aada");
		var localeGuids = AssetDatabase.FindAssetGUIDs($"t:{nameof(Locale)}", new[] { localesFolderPath });
		foreach (var localeGuid in localeGuids)
		{
			var locale = AssetDatabase.LoadAssetByGUID<Locale>(localeGuid);
			LocalizationEditorSettings.AddLocale(locale);
		}
		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
	}
}