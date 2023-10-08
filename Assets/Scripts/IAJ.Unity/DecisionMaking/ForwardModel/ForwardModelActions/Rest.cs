using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    public class Rest : Action
    {
        protected AutonomousCharacter Character { get; set; }
        public Rest(AutonomousCharacter character) : base("Rest")
        {
            Character = character;
        }

        public override bool CanExecute()
        {
            return Character.baseStats.HP < Character.baseStats.MaxHP;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;

            var currentdHP = (int)worldModel.GetProperty(Properties.HP);
            var MaxHP = (int)worldModel.GetProperty(Properties.MAXHP);
            return currentdHP < MaxHP;
        }

        public override void Execute()
        {
            GameManager.Instance.Rest();
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.SURVIVE_GOAL)
            {
                change -= 2;
            }
 
            return change;
        }

        
        public override void ApplyActionEffects(WorldModel worldModel)
        {
            base.ApplyActionEffects(worldModel);

            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);
            var currentHP = (int)worldModel.GetProperty(Properties.HP);
            var surviveGoal = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);

            var change = 0;

            if (currentHP + 2 <= maxHP )
            {
                change = 2;
            }
            else if (currentHP + 1 <= maxHP)
            {
                change = 1;
            }

            worldModel.SetProperty(Properties.HP, currentHP + change);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, surviveGoal - change);

        }

        /*public override float GetHValue(WorldModel worldModel)
        {
            var currentHP = (int)worldModel.GetProperty(Properties.HP);
            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);

            return (currentHP / maxHP) + base.GetHValue(worldModel);
        }
        */
    }
}
