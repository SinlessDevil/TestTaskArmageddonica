using Code.StaticData.Invocation.DTO;

namespace Code.Services.PowerCalculation
{
    public interface IInvocationPowerCalculationService
    {
        float CalculatePlayerPower();
        float CalculateEnemyPower();
        float CalculateInvocationPower(InvocationDTO invocation);
        BattlResult ComparePowers();
    }
}
