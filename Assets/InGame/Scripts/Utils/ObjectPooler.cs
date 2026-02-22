using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TileMatching.Utils {
    public class ObjectPooler : PersistentSingleton<ObjectPooler> {
        
        const int MaxPoolSize = 100;
        
        [SerializeField] int defaultPoolSize = 30;
        [SerializeField] bool shouldPoolAutomatically = false;
        [SerializeField] List<GameObject> objectsToPool;
        
        Dictionary<string, ObjectPool<GameObject>> pools = new Dictionary<string, ObjectPool<GameObject>>();


        protected override void Awake() {
            if (shouldPoolAutomatically) {
                for (var i = 0; i < objectsToPool.Count; i++) {
                    InitializePool(objectsToPool[i]);
                    PreWarmPool(objectsToPool[i]);
                }
            }
        }


        public void InitializePool(GameObject prefab) {
            if (pools == null || pools.ContainsKey(prefab.name)) {
                return;
            }

            var newPool = new ObjectPool<GameObject>(
                createFunc: () => InstantiatePooledObject(prefab),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false),
                actionOnDestroy: Destroy,
                collectionCheck: true,
                defaultCapacity: defaultPoolSize,
                maxSize:MaxPoolSize
            );
            
            pools.Add(prefab.name, newPool);
        }

        private GameObject InstantiatePooledObject(GameObject prefab) {
            var instanceObj = Instantiate(prefab, transform);
            instanceObj.name = prefab.name;
            instanceObj.SetActive(false);
            return instanceObj;
        }

        public void PreWarmPool(GameObject obj) {
            List<GameObject> objects = new List<GameObject>(defaultPoolSize);
            if (pools == null || !pools.ContainsKey(obj.name)) {
                Debug.LogWarning($"Cant pre warm the {obj.name} as its not initialized");
                return;
            }
            var currentPool = pools[obj.name];
            for (int i = 0; i < defaultPoolSize; i++) {
                var newObj = currentPool.Get();
                objects.Insert(i,newObj);
            }
            
            objects.ForEach(obj => currentPool.Release(obj));
        }
        
        public GameObject GetPooledObject(GameObject obj) {
            if (pools == null || !pools.ContainsKey(obj.name)) {
                return null;
            }
            
            return pools[obj.name].Get();
        }
        
        public void ReturnToPool(GameObject obj) {
            if (pools == null || !pools.ContainsKey(obj.name)) {
                return;
            }
            
            pools[obj.name].Release(obj);
        }
    }
}