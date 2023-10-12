using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.EnemyTasks;
using UnityEngine;
using Assets.Scripts.Game;
using Assets.Scripts.Game.NPCs;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.BehaviourTrees
{
    class AnchorBasicTree : Sequence
    {
        public AnchorBasicTree(Monster character, GameObject target)
        {
            // To create a new tree you need to create each branck which is done using the constructors of different tasks
            // Additionally it is possible to create more complex behaviour by combining different tasks and composite tasks...
            this.children = new List<Task>()
            {
                new Patrol(character, target),
                new IsCharacterNearTarget(character, target, character.enemyStats.AwakeDistance),
                new Pursue(character, target, character.enemyStats.WeaponRange),
                new LightAttack(character),
                new MoveTo(character,character.DefaultPosition,1.0f)
            };

        }

    }
}
