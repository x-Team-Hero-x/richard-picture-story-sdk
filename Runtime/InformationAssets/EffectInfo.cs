using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.InformationAssets
{
	[CreateAssetMenu(fileName = "EffectInfo", menuName = "Scriptable Objects/Effect info")]
	public class EffectInfo : InformationAsset
	{
		public LocalizedAsset<GameObject> effectPrefab = new();
	}
}