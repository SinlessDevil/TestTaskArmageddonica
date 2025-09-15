using Code.Logic.Cell;
using Code.Logic.Invocations.Builds;
using Code.Logic.Invocations.Skills;
using Code.Logic.Invocations.Units;
using Code.StaticData.Invocation.DTO;
using UnityEngine;

namespace Code.Services.Invocations.Factories
{
    public interface IInvocationFactory
    {
        Logic.Invocations.Invocation CreateInvocationByType(InvocationDTO dto, Cell targetCell, Quaternion rotation);
        Logic.Invocations.Invocation CreateInvocationBase(InvocationDTO invocationDTO, Cell targetCell, Quaternion rotation);
        Unit CreateUnit(UnitDTO unitDTO, Cell targetCell, Quaternion rotation);
        Build CreateBuilding(BuildDTO buildDto, Cell targetCell, Quaternion rotation);
        Skill CreateSkill(SkillDTO skillDTO, Cell targetCell, Quaternion rotation);
    }
}