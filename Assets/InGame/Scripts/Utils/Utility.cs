using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
        
        public static T GetRandomEnumValue<T>() where T : Enum  // Generic for any enum
        {
            Array values = Enum.GetValues(typeof(T));  // Get all enum values
            int randomIndex = Random.Range(0, values.Length);  // Random index [0, count)
            return (T)values.GetValue(randomIndex);  // Cast back to enum
        }
    }
}