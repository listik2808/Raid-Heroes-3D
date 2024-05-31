using Source.Scripts.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Logic
{
    public class SetMainCameraCanvas : MonoBehaviour
    {
        //[SerializeField] private Vector3 _direction;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _speed;
        [SerializeField] private List<CloudMover> _сlouds = new List<CloudMover>();
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _canvas.worldCamera = _camera;
        }

        private void Update()
        {
            foreach (CloudMover cloud in _сlouds)
            {
                cloud.transform.localPosition += Vector3.right * _speed * Time.deltaTime;
            }
        }
    }
}