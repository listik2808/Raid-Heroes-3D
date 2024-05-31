using Scripts.Infrastructure.Services;
using UnityEngine;

namespace Scripts.Infrastructure.AssetManagement
{
    public interface IAsset : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path,Vector3 at);
    }
}