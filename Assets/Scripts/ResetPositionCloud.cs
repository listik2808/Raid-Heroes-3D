using UnityEngine;

namespace Source.Scripts.Logic
{
    public class ResetPositionCloud : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out CloudMover cloudMover))
            {
                cloudMover.transform.localPosition = new Vector3(_startPoint.transform.localPosition.x, cloudMover.transform.localPosition.y, 0);
            }
        }
    }
}
