using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.InformationAssets
{
	[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/Character info")]
	public class CharacterInfo : InformationAsset
	{
		public LocalizedAsset<GameObject> spritePrefab = new();
		public LocalizedString displayName = new();
	}
}