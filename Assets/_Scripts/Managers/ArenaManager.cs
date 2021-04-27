using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{

    public List<ArenaObject> arenaObjects = new List<ArenaObject>();

    public static ArenaManager instance;

    private void Awake()
    {

        if (!instance)
            instance = this;

    }


    public void DespawnAllObjects(float _minTime, float _maxTime)
    {
        foreach (ArenaObject aObject in arenaObjects)
        {


            aObject.Despawn(_minTime, _maxTime);


        }

    }

}
