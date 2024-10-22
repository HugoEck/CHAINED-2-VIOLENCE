using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegister : MonoBehaviour
{
    public float totalDataPointsGathered;
    public float totalEnemyGathered;

    public void AddDataPoints(float value)
    {
        totalDataPointsGathered += value;
    }

    public void AddEnemyValue()
    {

    }
}
