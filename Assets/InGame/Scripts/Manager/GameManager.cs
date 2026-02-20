using System;
using System.Threading.Tasks;
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
            foreach (var spawner in levelDataHolder.Spawners) {
                spawner.Spawn();
            }
        }
    }
}