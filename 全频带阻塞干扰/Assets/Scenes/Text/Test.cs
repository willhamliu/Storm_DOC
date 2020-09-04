using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform a;
    public Transform b;

    void Start()
    {
        Debug.Log(Vector3.Angle(-Vector2.right,-b.right));
    }
}
