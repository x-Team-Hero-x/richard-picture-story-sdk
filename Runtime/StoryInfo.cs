using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk
{
    [CreateAssetMenu(fileName = "StoryInfo", menuName = "Scriptable Objects/Story info")]
    public class StoryInfo : ScriptableObject
    {
        [HideInInspector] public required string id;
        public LocalizedAsset<Sprite> icon = new();
        public LocalizedString title = new();
        public LocalizedString description = new();
    }
}
