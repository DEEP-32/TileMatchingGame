using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TileMatching {
    public class PlayerInputHandler : MonoBehaviour {
        [SerializeField] InputActionReference tapInputAction;

        void OnEnable() {

            if (tapInputAction != null) {
                tapInputAction.action.Enable();
                tapInputAction.action.performed += OnTap;
            }
            
        }

        void OnDisable() {
            if (tapInputAction != null) {
                tapInputAction.action.Disable();
                tapInputAction.action.performed -= OnTap;
            }
        }
        
        public void OnTap(InputAction.CallbackContext context) {
            if (context.performed) {
                Debug.Log("Tapped");
            }
        }
    }
}