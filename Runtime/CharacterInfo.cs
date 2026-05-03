using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk
{
	[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/Character info")]
	public class CharacterInfo : ScriptableObject
	{
		[HideInInspector] public required string id;
		public LocalizedAsset<GameObject> spritePrefab = new();
    	public LocalizedString displayName = new();
	}
}