using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TileMatching.Data {

    public enum TileColorKey {
        Red,
        Green,
        Blue,
        Yellow
    }

    [System.Serializable]
    public struct TileMatcherDataEntry {
        [SerializeField] TileColorKey colorKey;
        [SerializeField] Color color;
        
        public Color TileColor => color;
        public TileColorKey TileColorKey => colorKey;
    }
    
    
    [CreateAssetMenu(fileName = "TileMatcherData", menuName = "GameData/TileMatcherData", order = 0)]
    public class TileMatcherData : ScriptableObject{
        [SerializeField] List<TileMatcherDataEntry> tileMatcherData;
        
        public List<TileMatcherDataEntry> TileMatcherDataList => tileMatcherData; 

        public Color GetTileColor(TileColorKey tileColorKey) {
            return tileMatcherData.
                Where(key => key.TileColorKey == tileColorKey).
                Select(key => key.TileColor).FirstOrDefault();
        }
    }
    
    [CreateAssetMenu(fileName = "GameConfig", menuName = "GameData/GameConfig", order = 0)]
    public class GameConfig : ScriptableObject {
        [SerializeField] TileMatcherData tileMatcherData;
    }
}