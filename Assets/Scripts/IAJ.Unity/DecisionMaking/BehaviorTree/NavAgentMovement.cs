using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.EnemyTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Game;
using Assets.Scripts.Game.NPCs;
using Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree;


namespace Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.BehaviourTrees
{
    class NavAgentMovement : Selector
    {
        public IsCharacterNearTarget checker;

        public NavAgentMovement(Monster character, Vector3 PositionA, Vector3 PositionB)
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(
                new Sequence(
                        new List<Task>
                        {
                            new MoveTo(character, new Vector3(PositionA.x, PositionA.y, PositionA.z), 1.0f, false, 50f),
                            new MoveTo(character, new Vector3(PositionB.x, PositionB.y, PositionB.z), 1.0f, false, 50f)
                        }
                 )
            );
            this.children = tasks;
        }

        public override Result Run()
        {
            return children[0].Run();
        }

    }
}
