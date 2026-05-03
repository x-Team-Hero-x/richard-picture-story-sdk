using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk
{
    [CreateAssetMenu(fileName = "StoryInfo", menuName = "Scriptable Objects/Story info")]
    public class StoryInfo : ScriptableObject, IDisposable
    {
        [HideInInspector] public required string id;
        public LocalizedAsset<Sprite> icon = new();
        public LocalizedString title = new();
        public LocalizedString description = new();
        [HideInInspector] public List<CharacterInfo> characters = new();

        public static async Task<StoryInfo> FromFile(string path)
        {
            var catalogLocator = await Addressables.LoadContentCatalogAsync(path, true).Task;
            catalogLocator.Locate("StoryInfo", typeof(StoryInfo), out var storyInfoLocations);
            var location = storyInfoLocations.Single();
            var storyInfo = await Addressables.LoadAssetAsync<StoryInfo>(location).Task;
            Addressables.RemoveResourceLocator(catalogLocator);
            if (storyInfo is null)
            {
                throw new NullReferenceException($"Could not load story from '{path}'");
            }
            return storyInfo;
        }

        public void Dispose()
        {
            Addressables.Release(this);
        }
    }
}
