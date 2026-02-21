using System;
using System.Threading.Tasks;
using Dreamteck.Splines;
using TileMatching.Data;
using TileMatching.Utils;
using UnityEngine;

namespace TileMatching{
    public class GameManager : PersistentSingleton<GameManager> {
        [SerializeField] GameConfig gameConfig;
        [SerializeField] LevelStaticDataHolder levelDataHolder;
        
        public GameConfig GameConfig => gameConfig;
        public LevelStaticDataHolder LevelDataHolder => levelDataHolder;


        async void Start() {
            await Task.Delay(2000);
            StartGame();
        }

        private void StartGame() {
            var splineTriggers = LevelDataHolder.SplineComputer.triggerGroups[0].triggers;

            foreach (var trigger in splineTriggers) {
                trigger.onCross.AddListener(OnSplineUserReachTrigger);
                Debug.Log("Added listener for trigger : " + trigger.name);
            }

            foreach (var spawner in levelDataHolder.Spawners) {
                spawner.Spawn();
            }
        }
        
        private void OnSplineUserReachTrigger(SplineUser splineUser) {
            splineUser.enabled = false;
            //TileHandler.Instance.AnimateTileFromSplineToEnd(splineUser.GetComponent<Tile>(),);
        }
    }
}