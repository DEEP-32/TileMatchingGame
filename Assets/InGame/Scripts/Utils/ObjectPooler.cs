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
        
        Dictionary<GameObject, ObjectPool<GameObject>> pools = new Dictionary<GameObject, ObjectPool<GameObject>>();


        void Awake() {
            if (shouldPoolAutomatically) {
                for (var i = 0; i < objectsToPool.Count; i++) {
                    InitializePool(objectsToPool[i]);
                    PreWardPool(objectsToPool[i]);
                }
            }
        }


        public void InitializePool(GameObject prefab) {
            if (pools == null || pools.ContainsKey(prefab)) {
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
            
            pools.Add(prefab, newPool);
        }

        private GameObject InstantiatePooledObject(GameObject prefab) {
            
            var instance = Instantiate(prefab, transform);
            instance.SetActive(false);
            return instance;
        }

        public void PreWardPool(GameObject prefab) {
            List<GameObject> objects = new List<GameObject>(defaultPoolSize);
            if (pools == null || !pools.ContainsKey(prefab)) {
                Debug.LogWarning($"Cant pre warm the {prefab.name} as its not initialized");
                return;
            }
            var currentPool = pools[prefab];
            for (int i = 0; i < defaultPoolSize; i++) {
                var obj = currentPool.Get();
                objects.Insert(i,obj);
            }
            
            objects.ForEach(obj => currentPool.Release(obj));
        }
        
        public GameObject GetPooledObject(GameObject prefab) {
            if (pools == null || !pools.ContainsKey(prefab)) {
                return null;
            }
            
            return pools[prefab].Get();
        }
    }
}