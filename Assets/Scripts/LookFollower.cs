using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteAlways]
public class LookFollower : MonoBehaviour
{
    [SerializeField] DeckArrangement deck = null;
    CinemachineVirtualCamera cm;
    void Start()
    {
        cm = GetComponent<CinemachineVirtualCamera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (deck == null)
            return;
        cm.LookAt = deck.CurrentFocus();
    }
}
