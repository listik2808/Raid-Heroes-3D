
using Scripts.Data;

namespace Scripts.Infrastructure.Services.PersistentProgress
{
    public class PersistenProgressService : IPersistenProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}
