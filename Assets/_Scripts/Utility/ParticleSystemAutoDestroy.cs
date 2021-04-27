using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{


    void Start()
    {

        ParticleSystem ps = GetComponent<ParticleSystem>();

        if (ps)
        Destroy(gameObject, ps.main.duration + ps.main.startLifetimeMultiplier);

    }
}