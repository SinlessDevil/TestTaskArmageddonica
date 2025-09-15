using Code.Logic.Cell;
using Code.Logic.Grid;
using Code.StaticData.Invocation.DTO;
using Code.StaticData.Invocation.Data.Skill;

namespace Code.Services.Skills
{
    public interface ISkillExecutorService
    {
        void ExecuteBuildsSkill();
        bool SkillExecute(SkillDTO skillDTO, Cell targetCell);
    }
}