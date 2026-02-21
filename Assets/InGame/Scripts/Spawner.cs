using System.Collections.Generic;
using Dreamteck.Splines;
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
                var tile = tileGameObject.GetComponent<Tile>();
                tile.Initialize(GameManager.Instance.LevelDataHolder.SplineComputer,tileColorKey);
                spawnedTiles.Add(tile);
            }
        }

        public void Interact() {
            var levelData = GameManager.Instance.LevelDataHolder;
            var toTransform = levelData.StartPoint;
            TileHandler.Instance.AnimateTiles(spawnedTiles,toTransform, (tile) => {
                tile.AllowSplineMovement(true, levelData.GetStartPointPercent());
            });
        }
    }
}