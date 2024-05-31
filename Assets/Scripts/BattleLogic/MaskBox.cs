using UnityEngine;

public class MaskBox : MonoBehaviour
{
    [SerializeField] private GameObject _heroObj;
    public GameObject Hero => _heroObj;
    //bool _isActive = false;

    public void ReturnStartingPosition()
    {
        _heroObj.transform.SetParent(transform);
        _heroObj.transform.localScale = new Vector3(1, 1, 1);
        _heroObj.transform.position = gameObject.transform.position;
    }
}
