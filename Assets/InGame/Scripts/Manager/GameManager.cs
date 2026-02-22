using System;
using System.Threading.Tasks;
using Dreamteck.Splines;
using TileMatching.Data;
using TileMatching.Spawning;
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

        void StartGame() {
            var splineTriggers = LevelDataHolder.SplineComputer.triggerGroups[0].triggers;
            foreach (var trigger in splineTriggers) {
                var currentTrigger = trigger;
                trigger.onCross.AddListener(splineUser => {
                    OnSplineUserReachTrigger(splineUser,currentTrigger);
                });
                
                Debug.Log("Added listener for trigger : " + trigger.name);
            }
            foreach (var tileCrate in LevelDataHolder.TileCrates) {
                Debug.Log("Initializing tile crate : " + tileCrate.name + "for index : " + tileCrate.TriggerIndex +" and spline trigger size: " + splineTriggers.Length);
                var splineTrigger = splineTriggers[tileCrate.TriggerIndex];
                tileCrate.Initialize(splineTrigger);
            }
            
            
            
            foreach (var spawner in levelDataHolder.Spawners) {
                spawner.Spawn();
            }
        }
        
        void OnSplineUserReachTrigger(SplineUser splineUser,SplineTrigger trigger) {
            var tileCrateTo = LevelDataHolder.GetTileCrateForTrigger(trigger);
            var tile = splineUser.GetComponent<Tile>();
            if (tile.CurrentColorKey != tileCrateTo.CurrentColorKey) {
                return;
            }
            
            splineUser.enabled = false;
           
            TileHandler.Instance.AnimateTileFromSplineToEnd(
                splineUser.GetComponent<Tile>(),
                tileCrateTo,
                null,
                tileCrateTo.ConsumeTiles
            );
        }
    }
}