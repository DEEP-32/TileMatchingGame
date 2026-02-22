using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace TileMatching.Spawning {
    
    
    public class SpawnerGroup : MonoBehaviour {
        
        [SerializeField] List<Spawner> spawners;
        Queue<Spawner> availableSpawners;

        void Awake() {
            availableSpawners = new Queue<Spawner>(spawners);
            foreach (var spawner in spawners) {
                availableSpawners.Enqueue(spawner);
            }
        }

        public void HandleSpawnerClick(Spawner spawner) {

            if (spawner != spawners.Last()) {
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
            /*var spawner = spawners.Last();
            Sequence sequence = DOTween.Sequence();

            var lastIndex = spawners.Count - 1;
            float step = spawners[lastIndex].transform.position.z - spawners[lastIndex - 1].transform.position.z;
            
            var aheadSpawnerPos = transform.position;
            var farSpawnrPos = spawner.transform.position;

            var aheadSpawnerMove = spawner.transform.DOMoveZ(spawner.transform.position.z + step, .25f);
            sequence.Append(aheadSpawnerMove);


            for (int i = lastIndex - 1; i >= 0; i--) {
                float targetZ = spawners[i + 1].transform.position.z;
                sequence.Join(spawners[i].transform.DOMoveZ(targetZ, .25f));
            }
            
            sequence.On*/
        }
        
    }
}