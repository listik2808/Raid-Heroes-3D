using Scripts.Data;

namespace Scripts.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();
        bool Chek();
        PlayerProgress LoadProgress();
    }
}