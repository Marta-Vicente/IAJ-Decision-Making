using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    public class ShieldOfFaith : Action
    {
        protected AutonomousCharacter Character { get; set; }
        public ShieldOfFaith(AutonomousCharacter character) : base("ShieldOfFaith")
        {
            Character = character;
        }

        public override bool CanExecute()
        {
            return Character.baseStats.ShieldHP < 5 && Character.baseStats.Mana >= 5;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;

            var currentShieldHP = (int)worldModel.GetProperty(Properties.ShieldHP);
            var currentMana = (int)worldModel.GetProperty(Properties.MANA);
            return currentShieldHP < 5 && currentMana >= 5;
        }

        public override void Execute()
        {
            GameManager.Instance.ShieldOfFaith();
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.SURVIVE_GOAL)
            {
                change -= goal.InsistenceValue;
            }
 
            return change;
        }

        
        public override void ApplyActionEffects(WorldModel worldModel)
        {
            base.ApplyActionEffects(worldModel);
            var shieldHP = 5;
            worldModel.SetProperty(Properties.ShieldHP, shieldHP);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, 0.0f);
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
