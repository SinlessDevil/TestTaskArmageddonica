using Code.Logic.Grid;

namespace Code.Services.LevelConductor
{
    public interface ILevelConductor
    {
        void Setup(Grid grid);
        void Initialize();
        void Run();
        void Cleanup();
    }
}