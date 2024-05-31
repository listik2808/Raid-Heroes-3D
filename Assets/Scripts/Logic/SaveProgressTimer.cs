using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Scripts.Logic
{
    public class SaveProgressTimer : MonoBehaviour
    {
        private float _elepsedTime;
        private ISaveLoadService _saveLoadService;

        private void Start ()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void Update()
        {
            _elepsedTime += Time.deltaTime;
            if (_elepsedTime >= 10)
            {
                _saveLoadService.SaveProgress();
                _elepsedTime = 0;
            }
        }
    }
}
