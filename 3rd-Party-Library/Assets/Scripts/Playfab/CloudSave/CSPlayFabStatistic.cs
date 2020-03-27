using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPlayFabStatistic : MonoBehaviour
{
    private string entityId; // Id representing the logged in player

    private string entityType; // entityType representing the logged in player

    //Constructor method
    public CSPlayFabStatistic(string entityId, string entityType)
    {
        this.entityId = entityId;

        this.entityType = entityType;

        Debug.Log("{ Cloud Player Statistic } Service is Initializing...{ "+ entityId + " <---> " + entityType + " }");
    }
}
