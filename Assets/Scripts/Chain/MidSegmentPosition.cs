using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidSegmentPosition : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {       
        Vector3 player1Position = _player1.transform.position;
        Vector3 player2Position = _player2.transform.position;

        Vector3 midPoint = (player1Position + player2Position) / 2f;

        // Set this object's position to the midpoint
        transform.position = midPoint;
    }
}
