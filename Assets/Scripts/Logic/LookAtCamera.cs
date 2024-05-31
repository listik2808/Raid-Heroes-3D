using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera _camera;
    private Quaternion _vector;
    private void Start()
    {
        _camera = Camera.main;
        _vector = transform.rotation;
    }

    private void Update()
    {
        //transform.LookAt(_camera.transform.position);
        transform.rotation = _vector;
    }
}
