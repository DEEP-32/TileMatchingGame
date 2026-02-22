using UnityEngine;


namespace UIBase.UI{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UiScreen : MonoBehaviour{

        protected CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup => canvasGroup;
        
        public virtual void Initialize() {
            CacheComponents();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Will enable interaction and visibility of the screen.
        /// </summary>
        public virtual void ShowScreen() {
            OnBeforeScreenShow();
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            gameObject.SetActive(true);
            OnAfterScreenShow();
        }

        /// <summary>
        /// Will disable interaction and visibility of the screen.
        /// </summary>
        public virtual void HideScreen() {
            OnBeforeScreenHide();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
            OnAfterScreenHide();
        }
        
        public virtual void OnBeforeScreenShow() {
            
        }

        public virtual void OnAfterScreenShow() {
            
        }

        public virtual void OnBeforeScreenHide() {
            
        }
        
        public virtual void OnAfterScreenHide() {
            
        }

        public void CacheComponents() {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}