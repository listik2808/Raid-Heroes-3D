using Scripts.Army.PlayerSquad;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    //public GameObject[] maskObj;

    //private void OnEnable()
    //{
    //    for (int i = 0; i < maskObj.Length; i++)
    //    {
    //        if (maskObj[i].TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
    //        {
    //            skinnedMeshRenderer.material.renderQueue = 3002;
    //        }
    //        else if (maskObj[i].TryGetComponent(out MeshRenderer meshRenderer))
    //        {
    //            meshRenderer.material.renderQueue = 3002;
    //        }
    //       // maskObj[i].GetComponent<MeshRenderer>().material.renderQueue = 3002;
    //    }
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent(out MaskBox maskBox))
    //    {
    //        if(maskBox.Hero.activeInHierarchy == true)
    //            maskBox.Hero.SetActive(false);
    //        else
    //            maskBox.Hero.SetActive(true);
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out MaskBox maskBox))
        {
            if (maskBox.Hero.activeInHierarchy == true)
                return;
            else
                maskBox.Hero.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MaskBox maskBox))
        {
            if (maskBox.Hero.activeInHierarchy == true)
                maskBox.Hero.SetActive(false);
        }
        
    }
}
