using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public static class RandomUtility
{
    public static  int WeightedRandom(float[] weights)
    {
        var weightSum = weights.Sum();

        for (var i = 0; i < weights.Length; ++i)
        {
            if (Random.Range(0, weightSum) < weights[i])
            {
                return i;
            }

            weightSum -= weights[i];
        }

        return weights.Length;
    }
}
