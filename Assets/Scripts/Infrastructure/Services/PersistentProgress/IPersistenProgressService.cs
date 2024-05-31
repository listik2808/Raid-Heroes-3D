using Scripts.Data;

namespace Scripts.Infrastructure.Services.PersistentProgress
{
    public interface IPersistenProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}