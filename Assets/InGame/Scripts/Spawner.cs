using TileMatching.Data;
using UnityEngine;

namespace TileMatching {
    public class Spawner : MonoBehaviour {
        [SerializeField,Tooltip("Which color tile it should spawn")] TileColorKey tileColorKey;
        [SerializeField] int tileToSpawn;



        public void SpawnTile(TileColorKey tileColorKey) {
            
        }
        
    }
}