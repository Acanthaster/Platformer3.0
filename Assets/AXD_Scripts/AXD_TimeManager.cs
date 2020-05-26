using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_TimeManager : MonoBehaviour
{
    private static int seconds;
    private static bool counting;
    private Coroutine CountCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        seconds = 0;
        counting = true;
        CountCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (counting && CountCoroutine == null)
        {
            CountCoroutine = StartCoroutine(CountSeconds());
        }
    }

    IEnumerator CountSeconds()
    {
        while (counting)
        {
            yield return new WaitForSeconds(1);
            seconds++;
        }
        CountCoroutine = null;
    }

    public static void StartCounting()
    {
        counting = true;
    }

    public static void StopCounting()
    {
        counting = false;
    }

    public static int GetSeconds()
    {
        return seconds;
    }
}
