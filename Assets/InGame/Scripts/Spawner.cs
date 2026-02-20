using System.Collections.Generic;
using TileMatching.Data;
using TileMatching.Interaction;
using TileMatching.Utils;
using UnityEngine;

namespace TileMatching {
    public class Spawner : MonoBehaviour,IInteractable {
        [SerializeField] GameObject tilePrefab;
        [SerializeField,Tooltip("Which color tile it should spawn")] TileColorKey tileColorKey;
        [SerializeField] List<Transform> spawnPoints;

        [SerializeField] MeshRenderer meshRenderer;
        
        List<Tile> spawnedTiles = new List<Tile>();

        public void Spawn(TileColorKey colorKey = TileColorKey.None) {
            if (colorKey == TileColorKey.None) {
                colorKey = tileColorKey;
            }

            meshRenderer.material = GameManager.Instance.GameConfig.GetTimeMatcherDataFor(colorKey).Material;
            SpawnTile(colorKey);
        }
        
        public void SpawnTile(TileColorKey tileColorKey) {
            for (var i = 0; i < spawnPoints.Count; i++) {
                var tileGameObject = ObjectPooler.Instance.GetPooledObject(tilePrefab);
                tileGameObject.transform.position = spawnPoints[i].position;
                tileGameObject.transform.rotation = spawnPoints[i].rotation;
                tileGameObject.SetActive(true);
                spawnedTiles.Add(tileGameObject.GetComponent<Tile>());
            }
        }

        public void Interact() {
            var toTransform = GameManager.Instance.LevelDataHolder.StartPoint;
            TileHandler.Instance.AnimateTiles(spawnedTiles,toTransform);
        }
    }
}