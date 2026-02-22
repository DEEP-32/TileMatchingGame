using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using TileMatching.Data;
using TileMatching.Interaction;
using TileMatching.Utils;
using UnityEngine;

namespace TileMatching.Spawning {

    public interface ITileAnimationFinishedCallback {
        void OnAnimationFinished();
    }
    
    
    
    public class Spawner : MonoBehaviour,IInteractable,ITileAnimationFinishedCallback {
        readonly static int BaseColor = Shader.PropertyToID("_BaseColor");
        [SerializeField] GameObject tilePrefab;
        [SerializeField,Tooltip("Which color tile it should spawn")] TileColorKey tileColorKey;
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] TilePositionHolder spawnPointHolder;

        bool canInteract = true;
        
        public List<Tile> SpawnedTiles {
            get; private set;
        } = new List<Tile>();
        
        
        
        public bool IsInteractable { get => canInteract;}

        public void Spawn(TileColorKey colorKey = TileColorKey.None) {
            if (colorKey == TileColorKey.None) {
                colorKey = tileColorKey;
            }

            //meshRenderer.material = GameManager.Instance.GameConfig.GetTimeMatcherDataFor(colorKey).Material;
            var randomEnumValue = Utility.GetRandomEnumValue<TileColorKey>(TileColorKey.None);
            var randomSpawnerColor = GameManager.Instance.GameConfig.GetTimeMatcherDataFor(randomEnumValue).TileColor;
            meshRenderer.material.SetColor(BaseColor,randomSpawnerColor );
            
            SpawnTile(colorKey);
        }
        
        public void SpawnTile(TileColorKey tileColorKey,bool shouldAnimate = false) {
            canInteract = false;
            Sequence sequence = DOTween.Sequence();
            
            for (var i = 0; i < spawnPointHolder.Points.Count; i++) {
                var tileGameObject = ObjectPooler.Instance.GetPooledObject(tilePrefab);
                var localScale = tileGameObject.transform.localScale;
                tileGameObject.transform.position = spawnPointHolder.Points[i].position;
                tileGameObject.transform.rotation = spawnPointHolder.Points[i].rotation;
                if (shouldAnimate) {
                    tileGameObject.transform.localScale = Vector3.zero;
                }
                tileGameObject.SetActive(true);
                var tile = tileGameObject.GetComponent<Tile>();
                tile.Initialize(GameManager.Instance.LevelDataHolder.SplineComputer,tileColorKey);
                SpawnedTiles.Add(tile);

                if (shouldAnimate) {
                    float delay = .1f;
                    var scaleTween = tileGameObject.transform.DOScale(localScale, .15f);
                    if (i != 0) {
                        scaleTween.SetDelay(delay);
                    }
                    sequence.Append(scaleTween);
                }
                
            }
            
            sequence.OnComplete(() => {
                canInteract = true;
            });
            
            sequence.Play();
        }

        

        public void Interact() {
            if (IsInGroupSpawner()) {
                var spawnerGroupIndicator = GetComponentInParent<SpawnerGroup>();
                spawnerGroupIndicator.HandleSpawnerClick(this);
            }
            else {
                var levelData = GameManager.Instance.LevelDataHolder;
                var toTransform = levelData.StartPoint;
                TileHandler.Instance.AnimateTilesToSpline(
                    SpawnedTiles,
                    toTransform,
                    OnSingleTileReached,
                    OnAnimationFinished
                );
            }
            
        }
        
        public void OnSingleTileReached(Tile tile) {
            var levelData = GameManager.Instance.LevelDataHolder;
            tile.AllowSplineMovement(true, levelData.GetStartPointPercent());
        }

        public void OnAnimationFinished() {
            SpawnedTiles.Clear();
            
            var moveDirection = transform.right;
            var currentPosition = transform.position;
            
            Sequence sequence = DOTween.Sequence();

            var firstHalfMov = transform.DOMove(moveDirection * 40, .5f);
            sequence.Append(firstHalfMov);
            
            var secondHalfMove = transform.DOMove(currentPosition, .5f).SetEase(Ease.OutSine);
            sequence.Append(secondHalfMove);

            sequence.OnComplete(() => {
                var randomEnumValue = Utility.GetRandomEnumValue<TileColorKey>(TileColorKey.None);
                Debug.Log($"Spawning with key : {randomEnumValue}");
                SpawnTile(randomEnumValue,true);
            });

            sequence.Play();
        }

        public bool IsInGroupSpawner() {
            return GetComponentInParent<SpawnerGroup>() != null;
        }

    }
}