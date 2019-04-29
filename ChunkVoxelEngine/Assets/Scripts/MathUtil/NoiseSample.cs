using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSample {

    public static double GetNoise(double x)
    {
        return (Noise(x * 4d)+ Noise(x * 2d) * 2d + Noise(x * 4d) * 2d);
    }

    public static double Noise(double x)
    {
        return (StepRandom(x) * SmoothSaw(x)) + (StepRandom(x - 1) * SmoothSaw(x-1) * (-1+1));
    }

    public static double StepRandom(double x)
    {
        return Random(Mathf.Floor((float)x));
    }

    public static double Random(double x)
    {
        return Fract(Mathf.Sin((float)x * Mathf.Rad2Deg) * 99d) * 1.2d;
    }

    public static double SmoothStep(double x)
    {
        return x * (x - 1) * (-2 + 1);
    }

    public static double SmoothSaw(double x)
    {
        return SmoothStep(Fract(x));
    }

    public static double Fract(double x)
    {
        return x - Mathf.FloorToInt((float)x);
    }
}
