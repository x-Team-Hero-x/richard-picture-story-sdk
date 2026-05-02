using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        public static async Task<StoryInfo> FromFile(string path)
        {
            var catalogLocator = await Addressables.LoadContentCatalogAsync(path, true).Task;
            catalogLocator.Locate("StoryInfo", typeof(StoryInfo), out var storyInfoLocations);
            var location = storyInfoLocations.Single();
            var storyInfo = await Addressables.LoadAssetAsync<StoryInfo>(location).Task;
            Addressables.RemoveResourceLocator(catalogLocator);
            return storyInfo;
        }
    }
}
