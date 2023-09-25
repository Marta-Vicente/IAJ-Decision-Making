﻿using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    public class GetHealthPotion : WalkToTargetAndExecuteAction
    {
        public GetHealthPotion(AutonomousCharacter character, GameObject target) : base("GetHealthPotion",character,target)
        {
        }

        public override bool CanExecute()
        {
            if (!base.CanExecute()) return false;
            return Character.baseStats.HP < Character.baseStats.MaxHP;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;

            var currentHP = (int)worldModel.GetProperty(Properties.HP);
            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);
            return currentHP < maxHP;
        }

        public override void Execute()
        {
            base.Execute();
            GameManager.Instance.GetHealthPotion(this.Target);
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
            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);
            worldModel.SetProperty(Properties.HP, maxHP);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, 0.0f);

            //disables the target object so that it can't be reused again
            worldModel.SetProperty(this.Target.name, false);
        }

        public override float GetHValue(WorldModel worldModel)
        {
            var currentHP = (int)worldModel.GetProperty(Properties.HP);
            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);

            return (currentHP / maxHP) + base.GetHValue(worldModel);
        }
    }
}