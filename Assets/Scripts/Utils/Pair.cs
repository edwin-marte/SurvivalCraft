using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pair<U, V>
{
    [SerializeField] private U first;
    [SerializeField] private V second;

    public Pair()
    {
        first = default(U);
        second = default(V);
    }

    public Pair(U first, V second)
    {
        this.first = first;
        this.second = second;
    }

    public U Item1()
    {
        return first;
    }

    public V Item2()
    {
        return second;
    }
}
