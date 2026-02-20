using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TileMatching.Data {
    
    public enum TileColorKey {
        None = 0,
        Red,
        Green,
        Blue,
        Yellow
    }

    [System.Serializable]
    public struct TileMatcherDataEntry {
        [SerializeField] TileColorKey colorKey;
        [SerializeField] Color color;
        [SerializeField] Material material;
        
        public Color TileColor => color;
        public TileColorKey TileColorKey => colorKey;
        public Material Material => material;
    }
    
    [CreateAssetMenu(fileName = "TileMatcherData", menuName = "GameData/TileMatcherData", order = 0)]
    public class TileMatcherData : ScriptableObject{
        [SerializeField] List<TileMatcherDataEntry> tileMatcherData;
        
        public List<TileMatcherDataEntry> TileMatcherDataList => tileMatcherData; 

        public Color GetTileColor(TileColorKey tileColorKey) {
            return tileMatcherData.
                Where(key => key.TileColorKey == tileColorKey).
                Select(key => key.TileColor).
                FirstOrDefault();
        }
        
        public TileMatcherDataEntry GetTileData(TileColorKey tileColorKey) {
            return tileMatcherData.
                FirstOrDefault(key => key.TileColorKey == tileColorKey);
        }
    }
}