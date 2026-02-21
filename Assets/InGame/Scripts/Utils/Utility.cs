using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public static T GetRandomEnumValue<T>(params T[] exclusions) where T : Enum
        {
            // Get all values
            T[] allValues = (T[])Enum.GetValues(typeof(T));

            // Filter exclusions (using LINQ for simplicity)
            List<T> filtered = allValues.Where(v => !exclusions.Contains(v)).ToList();

            if (filtered.Count == 0)
            {
                throw new InvalidOperationException("No enum values left after exclusions.");
            }

            // Random index from filtered
            int randomIndex = Random.Range(0, filtered.Count);
            return filtered[randomIndex];
        }
    }
}