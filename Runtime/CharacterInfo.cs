using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk
{
	[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/Character info")]
	public class CharacterInfo : ScriptableObject
	{
		public required GameObject spritePrefab;
    	public required LocalizedString displayName;
	}
}