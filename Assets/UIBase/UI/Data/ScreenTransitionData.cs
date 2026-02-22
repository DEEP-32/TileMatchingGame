using UnityEngine;

namespace UIBase.UI{
    [CreateAssetMenu(fileName = "TransitionData", menuName = "UI/TransitionData", order = 0)]
    public class ScreenTransitionData : ScriptableObject{
        [SerializeField] private float time = 1f;
        [SerializeField] private float finalScale = 5000;
        [SerializeField] private DG.Tweening.Ease easeType;

        public float Time {
            get => time;
            set => time = value;
        }
        
        public float FinalScale {
            get => finalScale;
            set => finalScale = value;
        }
        
        public DG.Tweening.Ease EaseType {
            get => easeType;
            set => easeType = value;
        }
    }
}