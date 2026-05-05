using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk
{
	[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/Character info")]
	public class CharacterInfo : StoryAssetBase
	{
		public LocalizedAsset<GameObject> spritePrefab = new();
		public LocalizedString displayName = new();
	}
}