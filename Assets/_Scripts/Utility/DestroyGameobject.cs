﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameobject : MonoBehaviour
{

    public float lifetime = 1;

    
    // Start is called before the first frame update
    void Start()
    {

        Destroy(gameObject, lifetime);

    }

}
