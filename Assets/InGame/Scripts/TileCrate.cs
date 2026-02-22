using System;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using TileMatching.Data;
using TileMatching.Spawning;
using TileMatching.Utils;
using UnityEngine;

namespace TileMatching {
    public class TileCrate : MonoBehaviour {
        readonly static int BaseColor = Shader.PropertyToID("_BaseColor");
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] TilePositionHolder pointsHolder;
        [SerializeField] int triggerIndex = 0;

        List<Tile> tiles = new List<Tile>();
        int currentAnimatingIndex = 0;

        public int CurrentIndex {
            get; 
            private set;
        }

        public TileColorKey CurrentColorKey {
            get;
            private set;
        }
        
        public SplineTrigger SplineTrigger {
            get; 
            private set;
        }

        public bool ShouldAnimateToCrate {
            get;

            private set;
        } = true;


        
        public int TriggerIndex => triggerIndex;
        
        

        public void Initialize(SplineTrigger trigger) {
            SplineTrigger = trigger;
            var pointsCount = pointsHolder.Points.Count;
            tiles.Capacity = pointsCount;

            SetColorWithRandomValue();
        }

        void SetColorWithRandomValue() {
            var randomEnumValue = Utility.GetRandomEnumValue<TileColorKey>(TileColorKey.None);
            CurrentColorKey = randomEnumValue;
            SetColor(randomEnumValue);
        }

        void SetColor(TileColorKey tileColorKey) {
            var tileColor = GameManager.Instance.GameConfig.GetTimeMatcherDataFor(tileColorKey).TileColor;
            meshRenderer.material.SetColor(BaseColor, tileColor);
        }

        public void AddTile(Tile tile) {
            tiles.Insert(CurrentIndex++, tile);
        }

        public void ConsumeTiles() {
            ShouldAnimateToCrate = false;
            StartEndAnimation();
        }

        public bool IsAtMaxIndex() {
            return CurrentIndex >= pointsHolder.Points.Count;
        }

        public int GetAndUpdateAnimatingIndex() {
            return currentAnimatingIndex++;
        }

        private void StartEndAnimation() {
            var startScale = transform.localScale;
            var originalTileScale = tiles[0].transform.localScale;
            var scaleTween = transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InOutBounce).
                OnComplete(() => {
                    ConsumeTile_Internal();
                    SetColorWithRandomValue();
                    transform.DOScale(startScale, .25f).SetEase(Ease.InOutBounce).OnComplete(() => {
                        currentAnimatingIndex = 0;
                    });
                });
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(scaleTween);
            foreach (var tile in tiles) {
                var tileTween = tile.transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InOutBounce).OnComplete(() => {
                    tile.transform.localScale = originalTileScale;
                });
                sequence.Join(tileTween);
            }
            
            sequence.Play();
        }

        private void ConsumeTile_Internal() {
            foreach (var tile in tiles) {
                tile.ReturnToPool();
                tile.transform.position = Vector3.zero;
            }
            tiles.Clear();
            CurrentIndex = 0;
            currentAnimatingIndex = 0;
        }
    }
}