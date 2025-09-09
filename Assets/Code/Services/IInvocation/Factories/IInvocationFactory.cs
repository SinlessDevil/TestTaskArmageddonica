using Code.Logic.Grid;
using Code.Services.IInvocation.DTO;
using UnityEngine;

namespace Code.Services.IInvocation.Factories
{
    public interface IInvocationFactory
    {
        GameObject CreateInvocation(InvocationDTO invocationDTO, Cell targetCell);
        GameObject CreateUnit(UnitDTO unitDTO, Cell targetCell);
        GameObject CreateBuilding(BuildingDTO buildingDTO, Cell targetCell);
        GameObject CreateSkill(SkillDTO skillDTO, Cell targetCell);
    }
}