using Agava.WebUtility;
using Scripts.Army.TypesSoldiers;
using UnityEngine;

namespace Scripts.Mouse
{
    public class DragAndDrop : MonoBehaviour
    {
        public const string LayerHero = "Hero";
        private const string GroundZone = "GroundZone";
        private Soldier _soldier;
        private Collider _currentCollider;
        private Camera _camera;
        private Plane _dragPlane;
        private Vector3 _offSet;
        //private bool _isPause = false;
        //private bool _isFocus = true;

        private void OnEnable()
        {
            WebApplication.InBackgroundChangeEvent += Droping;
        }

        private void OnDisable()
        {
            WebApplication.InBackgroundChangeEvent -= Droping;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectPart();
            }
            if (Input.GetMouseButtonUp(0))
            {
                Drop();
            }

            DragAndDropObject();
            //if (_isFocus == false)
            //{
            //    if (_currentCollider != null || _soldier != null)
            //    {
            //        _soldier.PlayerCell.SetTransformPoint(_soldier);
            //        _currentCollider = null;
            //        _soldier = null;
            //    }
            //}
        }

        //private void OnApplicationFocus(bool focus)
        //{
        //    _isFocus = focus; 
        //}

        //private void OnApplicationPause(bool pause)
        //{
        //    _isPause = pause;
        //}

        private void SelectPart()
        {
            RaycastHit hit;

            Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(cameraRay, out hit, 1000f, LayerMask.GetMask(LayerHero
                )))
            {
                _currentCollider = hit.collider;
                if (_soldier == null)
                {
                    _soldier = _currentCollider.GetComponent<Soldier>();
                }
                _dragPlane = new Plane(Vector3.up, _currentCollider.transform.position);
                float planeDist;
                _dragPlane.Raycast(cameraRay, out planeDist);
                _offSet = _currentCollider.transform.position - cameraRay.GetPoint(planeDist);
            }
        }

        private void DragAndDropObject()
        {
            if (_currentCollider == null)
                return;

            RaycastHit hit;

            Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(cameraRay, out hit, 1000f, LayerMask.GetMask(GroundZone)))
            {
                float planeDist;
                _dragPlane.Raycast(cameraRay, out planeDist);

                _currentCollider.transform.position = cameraRay.GetPoint(planeDist) + _offSet; //hit.point 

                if (_currentCollider.transform.position.y < 1f || _currentCollider.transform.position.y > 1f)
                {
                    _currentCollider.transform.position = new Vector3(_currentCollider.transform.position.x, 1f, _currentCollider.transform.position.z);
                }
            }
        }

        private void Drop()
        {
            if (_currentCollider == null || _soldier == null)
                return;

            _soldier.PlayerCell.SetTransformPoint(_soldier);
            _currentCollider = null;
            _soldier = null;
        }

        private void Droping(bool value)
        {
            if (value)
            {
                if (_currentCollider == null || _soldier == null)
                    return;

                _soldier.PlayerCell.SetTransformPoint(_soldier);
                _currentCollider = null;
                _soldier = null;
            }
        }
    }
}
