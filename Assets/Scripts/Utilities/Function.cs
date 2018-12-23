using System;
using System.Collections.Generic;
using UnityEngine;
public class Function
{
    public static int Translate(int from, int value, int to)
    {
        int v = from + value;
        if (value > 0){
            return v > to ? to : v;
        }
        else {
            return v < to ? to : v;
        }
    }

    public static Vector3 Bezier(Vector3 p0, Vector3 p1, float t)
    {
        return (1 - t) * p0 + t * p1;
    }

    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 result = (1 - t) * p0p1 + t * p1p2;
        return result;
    }





}
