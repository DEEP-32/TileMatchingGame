using System.Collections.Generic;
using TileMatching.Data;
using TileMatching.Utils;
using UnityEngine;

namespace TileMatching {
    public class Spawner : MonoBehaviour {
        [SerializeField] GameObject tilePrefab;
        [SerializeField,Tooltip("Which color tile it should spawn")] TileColorKey tileColorKey;
        [SerializeField] List<Transform> spawnPoints;


        public void Spawn(TileColorKey colorKey) {
            
            SpawnTile(colorKey);
        }
        
        public void SpawnTile(TileColorKey tileColorKey) {
            for (var i = 0; i < spawnPoints.Count; i++) {
                var tileGameObject = ObjectPooler.Instance.GetPooledObject(tilePrefab);
                tileGameObject.transform.position = spawnPoints[i].position;
            }
        }
        
    }
}