using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.InformationAssets
{
	[CreateAssetMenu(fileName = "DialogInfo", menuName = "Scriptable Objects/Dialog info")]
	public class DialogInfo : InformationAsset
	{
		public LocalizedAsset<TextAsset> dialogFile = new();
	}
}