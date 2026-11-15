using System.Collections;
using UnityEngine;

public class hitStop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public static hitStop instance;
    
    private Coroutine hitStopCoroutine;
    private void Awake()
    {
        instance = this;
    }
    public void StopTime(float duration)
    {
        Debug.Log("Hit Stop Triggered for " + duration + " seconds.");
        if(hitStopCoroutine != null)
        {
            StopCoroutine(hitStopCoroutine);
        }
        hitStopCoroutine = StartCoroutine(StopTimeCoroutine(duration));
    }
    private IEnumerator StopTimeCoroutine(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
