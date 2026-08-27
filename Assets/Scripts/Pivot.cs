using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    public Rigidbody2D Body { get; private set; }

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
    }
}