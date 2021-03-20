using Kuhpik;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ArrayGCTestSystem : GameSystem, IIniting
{
    [SerializeField] bool isTesting;
    GameObject[] testArray;
    Stopwatch stopwatch;

    GameObject[] replaceArray;

    void IIniting.OnInit()
    {
        //Array.Empty is 10 time lower GC alloc
        if (isTesting) StartCoroutine(DelayRun());
    }

    IEnumerator DelayRun()
    {
        yield return null;
        yield return null;

        stopwatch = new Stopwatch();
        replaceArray = new GameObject[] { new GameObject("Yes"), new GameObject("No"), new GameObject("IDK") };

        NewArray();
        ArrayEmpty();
    }

    void NewArray()
    {
        Debug.Log("<color=orange>Begin new Array check</color>");
        stopwatch.Reset();
        stopwatch.Start();

        for (int i = 0; i < 1000; i++)
        {
            testArray = replaceArray;
            testArray = new GameObject[0];
        }

        stopwatch.Stop();
        Debug.Log("code took " + stopwatch.ElapsedMilliseconds + " ms");
    }

    void ArrayEmpty()
    {
        Debug.Log("<color=orange>Begin Array.Empty check</color>");
        stopwatch.Reset();
        stopwatch.Start();

        for (int i = 0; i < 1000; i++)
        {
            testArray = replaceArray;
            testArray = Array.Empty<GameObject>();
        }

        stopwatch.Stop();
        Debug.Log("code took " + stopwatch.ElapsedMilliseconds + " ms");
    }
}
