using UnityEngine;
using UnityEngine.InputSystem;

namespace TileMatching.Utils {
    public class Utility {
        /// <summary>
        /// Returns the mouse position either when using mouse or touchscreen
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetMousePostion() {
            if (Mouse.current != null) {
                return Mouse.current.position.ReadValue();
            }

            if (Touchscreen.current != null) {
                return Touchscreen.current.primaryTouch.position.ReadValue();
            }
            
            return Vector2.zero;
        }
    }
}