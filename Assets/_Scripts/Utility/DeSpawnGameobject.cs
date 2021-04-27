using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSpawnGameobject : MonoBehaviour, IPoolable
{

    public float lifetime = 1;

    void IPoolable.Spawn()
    {
        Invoke("DoDespawn",lifetime);
    }

    void IPoolable.Despawn() { }


    void DoDespawn()
    {

        ObjectPooler.Instance.Despawn(gameObject);

    }

}
