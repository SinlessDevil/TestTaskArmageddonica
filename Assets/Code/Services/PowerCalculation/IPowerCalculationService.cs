using Code.StaticData.Invocation.DTO;

namespace Code.Services.PowerCalculation
{
    public interface IPowerCalculationService
    {
        float CalculatePlayerPower();
        float CalculateEnemyPower();
        float CalculateInvocationPower(InvocationDTO invocation);
        BattleResult ComparePowers();
    }
    
    public enum BattleResult
    {
        Draw,    // Ничья
        Player,  // Игрок сильнее
        Enemy    // Враг сильнее
    }
}
