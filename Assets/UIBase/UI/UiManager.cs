using System;
using System.Collections;
using System.Collections.Generic;
using TileMatching.Utils;
using UnityEngine;
using UnityEngine.UI;


namespace UIBase.UI{
    public class UiManager : PersistentSingleton<UiManager>{
        [SerializeField] private UiScreenData uiScreenData;
        [SerializeField] private Image bgImage;

        [Header("HUD")] 
        [SerializeField] private Canvas hudCanvas;
        
        private UiScreen _currentScreen = null;
        private UiPopupScreen _currentPopupScreen = null;
        
        private Dictionary<Type, UiScreen> _uiScreensMap;

        public UiScreen GetCurrentActiveScreen => _currentScreen;
        public TransitionScreen DefaultTransitionScreen {
            get; private set;
        }
        

        protected override void Awake() {
            base.Awake();
        }

        public void Initialize() {
            uiScreenData.Initialize();
            
            _uiScreensMap = new Dictionary<Type, UiScreen>();
            foreach (var uiScreen in uiScreenData.UiScreens){
                var currentUiScreen = Instantiate(uiScreen, transform);
                print("Current ui screen : " + currentUiScreen.name);
                _uiScreensMap.Add(uiScreen.GetType(), currentUiScreen);
                currentUiScreen.Initialize();
            }

            if (uiScreenData.DefaultTransitionScreen != null) {
                DefaultTransitionScreen = Instantiate(uiScreenData.DefaultTransitionScreen, transform);
                DefaultTransitionScreen.gameObject.SetActive(false);
            }
        }


        #region Ui Screen methods

        public void DoTransitionAnimOut(){
            DefaultTransitionScreen.DoTransitionAnimOut();
        }
        
        public void DoTransitionAnimIn(){
            DefaultTransitionScreen.DoTransitionAnimIn();
        }
        
        
        public void HideCurrentScreen(){
            if (_currentScreen == null) return;
            _currentScreen.HideScreen();
            _currentScreen = null;
        }

        public void HideBGImage(){
            bgImage.gameObject.SetActive(false);
        }

        public void ShowBGImage(){
            bgImage.gameObject.SetActive(true);
        }
        
        public void ShowScreen<T>(bool useTransition = false,bool disableBGImage = false) where T : UiScreen {
            StartCoroutine(ShowScreenCoroutine<T>(useTransition,disableBGImage));
        }

        private IEnumerator ShowScreenCoroutine<T>(bool useTransition = false,bool disableBGImage  = false) where T : UiScreen{
            if (useTransition) {
                print($"UI Manager : doing anim in transition from {_currentScreen.GetType().Name} to {typeof(T).Name}");
                DefaultTransitionScreen.DoTransitionAnimIn();
                yield return new WaitForSecondsRealtime(DefaultTransitionScreen.TransitionData.Time * 2);
            }

            if (_currentScreen != null){
                _currentScreen.HideScreen();
            }
            Type screenType = typeof(T);
            _currentScreen = _uiScreensMap[screenType];
            _currentScreen.ShowScreen();
            
            bgImage.gameObject.SetActive(!disableBGImage);  

            if (useTransition) {
                print($"UI Manager : doing anim out transition from {_currentScreen.GetType().Name} to {typeof(T).Name}");
                DefaultTransitionScreen.DoTransitionAnimOut();
            }
        }
        

        #endregion

        #region HUD functions

        public BaseGameHUD CurrentHUD { get; private set; }
        
        public BaseGameHUD InitializeHUD(BaseGameHUD hudPrefab){
            CurrentHUD = Instantiate(hudPrefab, hudCanvas.transform);
            CurrentHUD.Initialize();
            return CurrentHUD;
        }

        public void HideHUD(){
            CurrentHUD.gameObject.SetActive(false);
        }
        
        public void ShowHUD(){
            CurrentHUD.gameObject.SetActive(true);
        }

        public void RemoveHUD(){
            Destroy(CurrentHUD.gameObject);
            CurrentHUD = null;
        }

        #endregion

        #region Pop ups

        //TODO : make different toggle for popup (like should block bg or just a yes/no popup), right now it just blocks the bg or any other screen behind it.
        public void ShowPopup<T>() where T : UiPopupScreen {
            _currentScreen.CanvasGroup.interactable = false;
            
            Type screenType = typeof(T);
            _currentPopupScreen = _uiScreensMap[screenType] as T;
            _currentPopupScreen.ShowScreen();
        }

        public void RemoveTopPopupScreen() {
            _currentPopupScreen.HideScreen();
            _currentPopupScreen = null;

            _currentScreen.CanvasGroup.interactable = true;
        }

        public T GetScreen<T>() where T : UiScreen {
            return _uiScreensMap.ContainsKey(typeof(T)) ? _uiScreensMap[typeof(T)] as T: null;
        }

        #endregion
    }
}