using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TileMatching.Data;
using TileMatching.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace TileMatching.Spawning {
    
    
    public class SpawnerGroup : MonoBehaviour {
        [SerializeField] Spawner lastDisabledSpawner;
        
        [FormerlySerializedAs("spawners")] [SerializeField] List<Spawner> activeSpawners;
        
        public void HandleSpawnerClick(Spawner spawner) {

            if (spawner != activeSpawners.First()) {
                return;
            }
            
            var levelData = GameManager.Instance.LevelDataHolder;
            var toTransform = levelData.StartPoint;
            TileHandler.Instance.AnimateTilesToSpline(
                spawner.SpawnedTiles,
                toTransform,
                spawner.OnSingleTileReached,
                OnAnimationFinished
            );
        }

        public void OnAnimationFinished() {
            var firstSpawner = activeSpawners.First();
            var secondSpawner = activeSpawners[1];
            
            var diffZ = secondSpawner.transform.position.z - firstSpawner.transform.position.z;
            
            Sequence sequence = DOTween.Sequence();

            var firstMoveTween = firstSpawner.transform.DOMoveZ(firstSpawner.transform.position.z - diffZ, .25f).
                OnComplete(() => {
                    firstSpawner.gameObject.SetActive(false);
                });

            sequence.Append(firstMoveTween);
            //starting from second because the first is already taken care of.
            for (var i = 1; i < activeSpawners.Count; i++) {
                var targetPos = activeSpawners[i - 1].transform.position;
                var moveTween = activeSpawners[i].transform.DOMoveZ(targetPos.z, .25f);
                sequence.Join(moveTween);
                var diff = activeSpawners[i].transform.position.z - targetPos.z;
                
                foreach (var spawnedTile in activeSpawners[i].SpawnedTiles) {
                    var targetForTile = spawnedTile.transform.position.z - diff;
                    
                    var tileMoveTween = spawnedTile.transform.DOMoveZ(targetForTile, .25f);
                    sequence.Join(tileMoveTween);
                }
            }
            
            var lastSpawner = activeSpawners.Last();
            var disabledSpawnerPos = lastDisabledSpawner.transform.position;
            lastDisabledSpawner.transform.DOMoveZ(lastSpawner.transform.position.z, .25f);


            sequence.OnComplete(() => {
                activeSpawners.RemoveAt(0);
                activeSpawners.Add(lastDisabledSpawner);
                
                lastDisabledSpawner.gameObject.SetActive(true);
                
                var randomEnumValue = Utility.GetRandomEnumValue<TileColorKey>(TileColorKey.None);
                lastDisabledSpawner.SpawnTile(randomEnumValue,true);
                
                lastDisabledSpawner = firstSpawner;
                firstSpawner.transform.position = disabledSpawnerPos;
            });

        }
        
        public bool AreWeHandlingThisSpawner(Spawner spawner) {
            return activeSpawners.Contains(spawner);
        }
        
    }
}