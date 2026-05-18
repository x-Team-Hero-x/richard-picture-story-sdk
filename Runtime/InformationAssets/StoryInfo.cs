using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

namespace HeroTeam.RichardPicture.StorySdk.InformationAssets
{
    [CreateAssetMenu(fileName = "StoryInfo", menuName = "Scriptable Objects/Story info")]
    public class StoryInfo : InformationAsset, IDisposable
    {
        
        # region Properties
        public LocalizedAsset<Sprite> icon = new();
        public LocalizedString title = new();
        public LocalizedString description = new();
        [HideInInspector] public List<InformationAsset> informationAssets = new();
        [HideInInspector] public required DialogInfo initialDialog;
        #endregion
        
        #region Lifecycle
        
        private AssetBundle? _assetBundle;

        public static async Task<StoryInfo> FromStoryFile(string path)
        {
            var loadHandle = AssetBundle.LoadFromFileAsync(path);
            await loadHandle;
            var assetBundle = loadHandle.assetBundle;
            var storyInfo = assetBundle.LoadAsset<StoryInfo>("StoryInfo.asset");
            storyInfo._assetBundle = assetBundle;
            return storyInfo;
        }

        public void Dispose()
        {
            if (_assetBundle is null)
            {
                return;
            }
            _assetBundle?.Unload(true);
            _assetBundle = null;
        }
        
        #endregion

        #region Asset getter
        
        private Dictionary<Type, Dictionary<string, InformationAsset>> _informationAssetsDictionary = new();
        
        private void OnEnable()
        {
            _informationAssetsDictionary = informationAssets
                .GroupBy(asset => asset.GetType())
                .ToDictionary(
                    group => group.Key,
                    group => group.ToDictionary(asset => asset.id)
                );
        }

        public T GetAsset<T>(string assetId) where T : InformationAsset
        {
            if (!_informationAssetsDictionary.TryGetValue(typeof(T), out var informationAssetsOfType))
            {
                throw new KeyNotFoundException($"Story '{id}' has no assets of type '{typeof(T).Name}'");
            }

            if (!informationAssetsOfType.TryGetValue(assetId, out var informationAsset))
            {
                throw new KeyNotFoundException($"Story '{id}' has no '{typeof(T).Name}' with id '{assetId}'");
            }

            return (T)informationAsset;
        }
        
        #endregion
    }
}