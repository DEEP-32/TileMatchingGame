using System;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

namespace TileMatching {
    public class LevelStaticDataHolder : MonoBehaviour {
        [SerializeField] Transform startPoint;
        public Transform StartPoint => startPoint;
        
        
        List<Spawner> spawners = new List<Spawner>();
        public IReadOnlyList<Spawner> Spawners => spawners;

        void Awake() {
            spawners = FindObjectsByType<Spawner>(FindObjectsInactive.Exclude,FindObjectsSortMode.None).ToList();
        }


        //should have list of all the spawner , where the tile is spawned and which is handler of output
        //should also have list of all object where the tile should after reaching the destination
    }
}