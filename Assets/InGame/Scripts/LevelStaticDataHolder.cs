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
        List<Spawner> spawners = new List<Spawner>();
        
        public Transform StartPoint => startPoint;
        public SplineComputer SplineComputer => splineComputer;
        public IReadOnlyList<Spawner> Spawners => spawners;
        
        public double GetStartPointPercent() => startPoint.GetComponent<SplineFollower>().GetPercent();

        void Awake() {
            spawners = FindObjectsByType<Spawner>(FindObjectsInactive.Exclude,FindObjectsSortMode.None).ToList();
        }


        //should have list of all the spawner , where the tile is spawned and which is handler of output
        //should also have list of all object where the tile should after reaching the destination
    }
}