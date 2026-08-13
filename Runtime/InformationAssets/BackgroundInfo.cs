using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.InformationAssets
{
	[CreateAssetMenu(fileName = "BackgroundInfo", menuName = "Scriptable Objects/Background info")]
	public class BackgroundInfo : InformationAsset
	{
		public LocalizedAsset<Sprite> backgroundSprite = new();
	}
}