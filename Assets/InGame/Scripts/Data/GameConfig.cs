using UnityEngine;

namespace TileMatching.Data {
    [CreateAssetMenu(fileName = "GameConfig", menuName = "GameData/GameConfig", order = 0)]
    public class GameConfig : ScriptableObject {
        [SerializeField] TileMatcherData tileMatcherData;

        public TileMatcherDataEntry GetTimeMatcherDataFor(TileColorKey tileColorKey) {
            return tileMatcherData.GetTileData(tileColorKey);
        }
    }
}