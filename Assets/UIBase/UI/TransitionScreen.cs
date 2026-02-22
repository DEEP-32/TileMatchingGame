using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIBase.UI{
    //TODO : Future improvements - Make different settings scriptable object/serialize class olfor
    public class TransitionScreen : MonoBehaviour{

        [Header("Data")]
        [SerializeField] private ScreenTransitionData transitionData;
        
     
        [Space]
        [Header("Internal references")]
        [SerializeField] private Image maskImage;

        public RectTransform TransitionImageRect => maskImage.rectTransform;
        public ScreenTransitionData TransitionData => transitionData;
        
        public void DoTransitionAnimIn(){
            maskImage.gameObject.SetActive(true);
            maskImage.rectTransform.DOSizeDelta(Vector2.zero, transitionData.Time).SetUpdate(true).SetEase(transitionData.EaseType);
        }
        public void DoTransitionAnimOut() {
            StartCoroutine(TransitionAnimOut());
        }
        
        public void SetTransitionToDefault(){
            maskImage.gameObject.SetActive(true);
            maskImage.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }
        
        /*private void TransitionAnimIn() {
            
            
        }*/

        private IEnumerator TransitionAnimOut() {
            maskImage.rectTransform.DOSizeDelta(Vector2.one * 5000, transitionData.Time * 1.5f).SetUpdate(true).SetEase(transitionData.EaseType);

            yield return new WaitForSecondsRealtime(transitionData.Time * 1.5f);
            EventSystem.current.SetSelectedGameObject(null);
            maskImage.gameObject.SetActive(false);
        }
    }
}