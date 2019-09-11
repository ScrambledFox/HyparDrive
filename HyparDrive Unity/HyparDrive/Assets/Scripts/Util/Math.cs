using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math {

    /// <summary>
    /// Get the smallest float of an array.
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    public static float Smallest (float[] numbers) {
        float smallest = float.MaxValue;
        for (int i = 0; i < numbers.Length; i++) {
            if (numbers[i] < smallest) {
                smallest = numbers[i];
            }
        }

        return smallest;
    }
    /// <summary>
    /// Get the smallest float of an array.
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    public static float Smallest ( Vector3 vector3 ) {
        float smallest = float.MaxValue;
        for (int i = 0; i < 3; i++) {
            if (vector3[i] < smallest) {
                smallest = vector3[i];
            }
        }

        return smallest;
    }

    /// <summary>
    /// Get the biggest float of an array.
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    public static float Biggest ( float[] numbers ) {
        float biggest = float.MinValue;
        for (int i = 0; i < numbers.Length; i++) {
            if (numbers[i] < biggest) {
                biggest = numbers[i];
            }
        }

        return biggest;
    }

}
