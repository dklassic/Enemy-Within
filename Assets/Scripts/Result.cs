using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public class Result : MonoBehaviour
{
    [Header("Result Shower")]
    [SerializeField] GameObject yes = null;
    [SerializeField] GameObject no = null;
    [SerializeField] List<bool> testResult = new List<bool>();
    [SerializeField] float dropInterval = 0.2f;
    [ExposeMethodInEditor]
    void DropYes()
    {
        if (yes == null)
            return;
        Instantiate(yes, transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
    }
    [ExposeMethodInEditor]
    void DropNo()
    {
        if (no == null)
            return;
        Instantiate(no, transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
    }

    public void ShowResult(List<bool> results)
    {
        int totalCount = results.Count;
        List<bool> rndResults = new List<bool>();
        for (int i = 0; i < totalCount; i++)
        {
            int index = Random.Range(0, totalCount - rndResults.Count);
            rndResults.Add(results[index]);
            results.RemoveAt(index);
        }
        StartCoroutine(DropStone(rndResults));
    }

    [ExposeMethodInEditor]
    void TestShowResult()
    {
        ShowResult(testResult);
    }

    IEnumerator DropStone(List<bool> results)
    {
        foreach (bool result in results)
        {
            if (result)
                DropYes();
            else
                DropNo();
            yield return new WaitForSeconds(dropInterval);
        }
    }
}
