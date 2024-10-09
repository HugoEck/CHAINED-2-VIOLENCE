using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraManager : MonoBehaviour
{
    GameObject _midChainSegment;

    private void Update()
    {
        if (_midChainSegment == null)
        {
            _midChainSegment = GameObject.FindGameObjectWithTag("MidChainSegment");
            gameObject.GetComponent<CinemachineVirtualCamera>().LookAt = _midChainSegment.transform;
        }
    }
}
