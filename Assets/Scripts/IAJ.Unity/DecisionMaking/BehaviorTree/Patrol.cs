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
    class Patrol : Selector
    {

        public IsCharacterNearTarget checker;
        public Patrol(Monster character, GameObject target)
        {
            List<Task> tasks = new List<Task>();
            /*
            tasks.Add(
                new Selector(
                    new List<Task> { new Sequence(
                                            new List<Task>
                                            {
                                                new IsCharacterNearTarget(character, target, character.enemyStats.WeaponRange),
                                                new Pursue(character, target, character.enemyStats.WeaponRange),
                                                new LightAttack(character)
                                            }
                                    ) ,
                                     new Sequence(
                                            new List<Task>
                                            {
                                                new MoveTo(character, new Vector3(character.DefaultPosition.x + 10,
                                                                                   character.DefaultPosition.y,
                                                                                   character.DefaultPosition.z), 1.0f),
                                                new MoveTo(character, new Vector3(character.DefaultPosition.x - 10,
                                                                                   character.DefaultPosition.y,
                                                                                   character.DefaultPosition.z), 1.0f)
                                            }
                                         ),
                                    new MoveTo(character, character.DefaultPosition, 1.0f)
                                    }

                    )
             );
            */

            checker = new IsCharacterNearTarget(character, target, character.enemyStats.AwakeDistance);
            tasks.Add(
                new Sequence(new List<Task>
                                {
                                    new IsCharacterNearTarget(character, target, character.enemyStats.AwakeDistance),
                                    new Pursue(character, target, character.enemyStats.WeaponRange),
                                    new LightAttack(character)
                                })
            );
            tasks.Add(
                new Sequence(
                        new List<Task>
                        {
                            new MoveTo(character, new Vector3(character.DefaultPosition.x + 10,
                                                                character.DefaultPosition.y,
                                                                character.DefaultPosition.z), 1.0f, false),
                            new MoveTo(character, new Vector3(character.DefaultPosition.x - 10,
                                                                character.DefaultPosition.y,
                                                                character.DefaultPosition.z), 1.0f, false)
                        }
                 )
            );
            tasks.Add(new MoveTo(character, character.DefaultPosition, 1.0f));

            this.children = tasks;
        }

        
        public override Result Run()
        {
            /*
            Result result;
            if (currentChild < children.Count)
            {
               result = children[currentChild].Run();
            }
            else
            {
                return Result.Success;
            }

            Debug.Log(result + " " + currentChild);

            if(result == Result.Running) return Result.Running;

            switch (currentChild)
            {
                case 0:
                    if (result == Result.Success || result == Result.Failure)
                    {
                        currentChild = 2;
                    }
                    break;
                case 1:
                    currentChild ++;
                    break;
                case 2:
                    currentChild = 0;
                    return Result.Success;
            }
            return Result.Success;
            */

            if(currentChild != 0 && checker.Run() == Result.Success)
            {
                currentChild = 0;
                return Result.Running;
            }

            if (children.Count > this.currentChild)
            {
                Result result = children[currentChild].Run();

                if (result == Result.Running)
                    return Result.Running;

                else if (result == Result.Failure)
                {
                    currentChild++;
                    if (children.Count <= this.currentChild)
                    {
                        currentChild = 0;
                        children[currentChild].Run();
                        //return Result.Failure;
                    }

                    return Result.Running;
                }
                else
                {
                    currentChild = 0;
                    children[currentChild].Run();
                    //return Result.Success;
                }
            }
            return Result.Success;
        }
        
        
    }
}
