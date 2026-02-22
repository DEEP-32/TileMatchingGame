using System;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using TileMatching.Spawning;
using UnityEngine;

namespace TileMatching {
    public class LevelStaticDataHolder : MonoBehaviour {
        [SerializeField] SplineComputer splineComputer;
        [SerializeField] Transform startPoint;
        
        List<Spawner> spawners;
        List<TileCrate> tileCrates;
        
        public Transform StartPoint => startPoint;
        public SplineComputer SplineComputer => splineComputer;
        public IReadOnlyList<Spawner> Spawners => spawners;
        public IReadOnlyList<TileCrate> TileCrates => tileCrates;       
        
        public double GetStartPointPercent() => startPoint.GetComponent<SplineFollower>().GetPercent();

        void Awake() {
            spawners = FindObjectsByType<Spawner>(FindObjectsInactive.Exclude,FindObjectsSortMode.None).ToList();
            tileCrates = FindObjectsByType<TileCrate>(FindObjectsInactive.Exclude,FindObjectsSortMode.None).ToList();
        }

        public TileCrate GetTileCrateForTrigger(SplineTrigger trigger) {
            return tileCrates.FirstOrDefault(crate => crate.SplineTrigger == trigger);
        }


        //should have list of all the spawner , where the tile is spawned and which is handler of output
        //should also have list of all object where the tile should after reaching the destination
    }
}