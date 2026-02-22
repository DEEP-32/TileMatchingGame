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


        TileCrateAnimationData animationData;
        

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
        
        

        public void Initialize(SplineTrigger trigger,TileCrateAnimationData animationData) {
            this.animationData = animationData;
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
            var originalPos = transform.position;
            Sequence sequence = DOTween.Sequence();


            var moveTween = transform.DOMoveY(transform.position.y + animationData.moveAnimationOffset, animationData.moveAnimationTime);
            
            sequence.Append(moveTween);
            
            foreach (var tile in tiles) {
                var tileTween = tile.transform.DOMoveY(transform.position.y + animationData.moveAnimationOffset,animationData.moveAnimationTime);
                sequence.Join(tileTween);
            }
            
            
            var scaleTween = transform.DOScale(animationData.scaleAnimationFinalValue, animationData.scaleAnimationTime).SetEase(animationData.scaleAnimationEase).
                OnComplete(() => {
                    ConsumeTile_Internal();
                    SetColorWithRandomValue();
                    transform.DOScale(startScale, animationData.scaleAnimationTime).SetEase(animationData.scaleAnimationEase).OnComplete(() => {
                        currentAnimatingIndex = 0;
                    });
                });
            
            
            sequence.Append(scaleTween);
            foreach (var tile in tiles) {
                var tileTween = tile.transform.DOScale(animationData.scaleAnimationFinalValue,animationData.scaleAnimationTime).SetEase(animationData.scaleAnimationEase).OnComplete(() => {
                    tile.transform.localScale = originalTileScale;
                });
                sequence.Join(tileTween);
            }
            
            sequence.Play();

            sequence.OnComplete(() => {
                transform.position = originalPos;
            });
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