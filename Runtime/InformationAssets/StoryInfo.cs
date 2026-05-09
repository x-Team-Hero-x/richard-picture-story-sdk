using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
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
        [HideInInspector] public List<DialogInfo> dialogs = new();
        
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
    }
}