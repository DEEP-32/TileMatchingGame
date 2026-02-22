using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace UIBase.UI{
    [CreateAssetMenu(fileName = "ScreensData", menuName = "UI/ScreensData", order = 0)]
    public class UiScreenData : ScriptableObject{

        public string LogChannel => nameof(UiScreenData);
        
        [Space]
        [SerializeField]
        private TransitionScreen defaultTransitionScreen;
        
        [Space]
        [Header("Screens")]
        [SerializeField, Tooltip("All the possible ui screen that game can have")]
        private List<UiScreen> uiScreens;

        [Space]
        [SerializeField, Tooltip("Make sure to add this screen to ui screen list")]
        private UiScreen startScreen;
        
        
        
        public List<UiScreen> UiScreens => uiScreens;
        public UiScreen StartScreen => startScreen;
        
        public TransitionScreen DefaultTransitionScreen => defaultTransitionScreen;
        
        private Dictionary<Type, UiScreen> _uiScreenDictionary;
        
        public void Initialize() {
            bool hasStartScreen = false;
            _uiScreenDictionary = new Dictionary<Type, UiScreen>();
            foreach (var uiScreen in uiScreens){
                if (startScreen.GetType() == uiScreen.GetType()){
                    hasStartScreen = true;
                }
                _uiScreenDictionary.Add(uiScreen.GetType(), uiScreen);
            }

            if (!hasStartScreen){
                Debug.Log(LogChannel + " : Start screen is not in the list of ui screens");
                _uiScreenDictionary.Add(startScreen.GetType(), startScreen);
            }

            
        }

        public UiScreen GetScreen(UiScreen screen) {
            return _uiScreenDictionary.ContainsKey(screen.GetType()) ? _uiScreenDictionary[screen.GetType()] : null;
        }

        
        /// <summary>
        /// Works the same as <see cref="GetScreen(UiScreen)"/> but with a generic type 
        /// and typecasts the result before returning.
        /// </summary>
        /// <param name="screen">Screen that we want to show</param>
        /// <typeparam name="T">Should be of type UiScreen or it child</typeparam>
        /// <returns></returns>
        public UiScreen GetScreen<T>(T screen) where T : UiScreen {
            return _uiScreenDictionary.ContainsKey(screen.GetType()) ? _uiScreenDictionary[screen.GetType()] as T : null;
        }
    }
}