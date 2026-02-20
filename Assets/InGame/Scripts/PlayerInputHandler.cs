using System;
using TileMatching.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TileMatching {
    public class PlayerInputHandler : MonoBehaviour {
        [SerializeField] InputActionReference tapInputAction;
        [SerializeField] LayerMask tapLayerMask;
        Camera cam;

        void Start() {
            cam = Camera.main;
        }

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
                var tapPos = Utility.GetMousePostion();
                var ray = cam.ScreenPointToRay(tapPos);
                
                if (Physics.Raycast(ray, out RaycastHit hit,Mathf.Infinity,tapLayerMask,QueryTriggerInteraction.Ignore)) {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                    // Interaction logic here
                }
            }
        }
    }
}