using Code.Logic.Grid;
using Code.Logic.Invocation;
using Code.Logic.Invocation.Builds;
using Code.Logic.Invocation.Skills;
using Code.Logic.Invocation.Units;
using Code.StaticData.Invocation.DTO;
using UnityEngine;

namespace Code.Services.IInvocation.Factories
{
    public interface IInvocationFactory
    {
        Invocation CreateInvocationByType(InvocationDTO dto, Cell targetCell, Quaternion rotation);
        Invocation CreateInvocationBase(InvocationDTO invocationDTO, Cell targetCell, Quaternion rotation);
        Unit CreateUnit(UnitDTO unitDTO, Cell targetCell, Quaternion rotation);
        Build CreateBuilding(BuildDTO buildDto, Cell targetCell, Quaternion rotation);
        Skill CreateSkill(SkillDTO skillDTO, Cell targetCell, Quaternion rotation);
    }
}