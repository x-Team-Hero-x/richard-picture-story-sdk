using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.InformationAssets
{
    [CreateAssetMenu(fileName = "StoryInfo", menuName = "Scriptable Objects/Story info")]
    public class StoryInfo : InformationAsset, IDisposable
    {
        public LocalizedAsset<Sprite> icon = new();
        public LocalizedString title = new();
        public LocalizedString description = new();
        [HideInInspector] public List<CharacterInfo> characters = new();
        
        private IResourceLocator? _catalogLocator;

        public static async Task<StoryInfo> FromStoryFile(string path)
        {
            var catalogLocator = await Addressables.LoadContentCatalogAsync(path, true).Task;
            var isManifestPresent = catalogLocator.Locate("StoryInfo", typeof(StoryInfo), out var storyInfoLocations);
            if (!isManifestPresent)
            {
                throw new InvalidDataException($"Missing StoryInfo asset in catalog at '{path}'");
            }
            var location = storyInfoLocations.Single();
            var storyInfo = await Addressables.LoadAssetAsync<StoryInfo>(location).Task;
            if (storyInfo is null)
            {
                throw new InvalidDataException($"Broken StoryInfo asset in catalog at '{path}'");
            }
            storyInfo._catalogLocator = catalogLocator;
            return storyInfo;
        }

        public void Dispose()
        {
            if (_catalogLocator is null)
            {
                return;
            }
            Addressables.Release(this);
            Addressables.RemoveResourceLocator(_catalogLocator);
            _catalogLocator = null;
        }
    }
}