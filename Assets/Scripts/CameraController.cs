using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using CatchCo;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] List<GameObject> camList = new List<GameObject>();
    [SerializeField] List<Transform> focusPoint = null;
    [SerializeField] Volume volume = null;
    int focusIndex = -1;
    void Update()
    {
        if (focusIndex == -1 || volume == null || focusPoint.Count == 0 || focusIndex > focusPoint.Count - 1)
            return;
        if (volume.profile.TryGet<DepthOfField>(out var dof))
        {
            float distance = (Camera.main.transform.position - focusPoint[focusIndex].position).magnitude;
            dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, distance, 0.1f);
        }
    }
    void SwitchCam(int count)
    {
        if (camList.Count == 0 || count > camList.Count - 1)
            return;
        foreach (GameObject obj in camList)
        {
            obj.SetActive(false);
        }
        camList[count].SetActive(true);
    }

    void SwitchFocus(int count)
    {
        if (count > focusPoint.Count - 1 || count < 0)
            return;
        focusIndex = count;
    }
    [ExposeMethodInEditor]
    public void SwitchToTitleScreen()
    {
        SwitchAperture(8f);
        SwitchFocus(0);
        SwitchCam(0);
    }
    [ExposeMethodInEditor]
    public void SwitchToDeckView()
    {
        SwitchAperture(5.6f);
        SwitchFocus(1);
        SwitchCam(1);
    }
    [ExposeMethodInEditor]
    public void SwitchToTableView()
    {
        SwitchAperture(5.6f);
        SwitchFocus(0);
        SwitchCam(2);
    }
    [ExposeMethodInEditor]
    public void SwitchToResultView()
    {
        SwitchAperture(8f);
        SwitchFocus(0);
        SwitchCam(3);
    }
    void SwitchAperture(float aperture)
    {
        DOTween.Kill("Focus");
        HDAdditionalCameraData camData = Camera.main.GetComponent<HDAdditionalCameraData>();
        DOTween.To(() => camData.physicalParameters.aperture, x => camData.physicalParameters.aperture = x, aperture, 0.5f).SetId("Focus");
    }
}
