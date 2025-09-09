using Code.Logic.Grid;
using Code.Logic.Invocation;
using Code.Logic.Invocation.Builds;
using Code.Logic.Invocation.Skills;
using Code.Logic.Invocation.Units;
using Code.Services.IInvocation.DTO;
using UnityEngine;

namespace Code.Services.IInvocation.Factories
{
    public interface IInvocationFactory
    {
        GameObject CreateInvocation(InvocationDTO invocationDTO, Cell targetCell);
        Unit CreateUnit(UnitDTO unitDTO, Cell targetCell);
        Build CreateBuilding(BuildingDTO buildingDTO, Cell targetCell);
        Skill CreateSkill(SkillDTO skillDTO, Cell targetCell);
    }
}