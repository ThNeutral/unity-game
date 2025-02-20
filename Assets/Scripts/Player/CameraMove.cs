using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform campos;
    public Vector3 adjustment = new Vector3 (0, 0.5f, 0);
    void Update()
    {
        transform.position = campos.position + adjustment;
    }
}
