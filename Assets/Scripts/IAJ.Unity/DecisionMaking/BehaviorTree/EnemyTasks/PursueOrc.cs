using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AI;
using Assets.Scripts.Game.NPCs;
using Assets.Scripts.Game;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree.EnemyTasks
{
    class PursueOrc : Pursue
    {
        private float timeToWaagh = 5f;
        private float time = 5.1f;
        private WAARRGGHHH scream;
        public PursueOrc(Monster character, GameObject target, float _range):base(character, target, _range) 
        {
            scream = new WAARRGGHHH(character as Orc, target);
        }

        public override Result Run()
        {
            time += Time.deltaTime;
            if (time > timeToWaagh) 
            {
                scream.Run();
                time = 0f;
            }

            return base.Run();
        }

    }
}
