using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.Action;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using Assets.Scripts.IAJ.Unity.Utils;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSLimitedPlayout : MCTS
    {
        public int maxDepthAllowed = 5;
        public MCTSLimitedPlayout(CurrentStateWorldModel currentStateWorldModel) : base(currentStateWorldModel)
        {
            this.MaxIterations = 5000;
            this.MaxPlayoutsPerNode = 10;
        }

        // Selection and Expantion

        protected override float Playout(WorldModel initialStateForPlayout)
        {
            Action[] executableActions;

            var currentDepth = 0;
            
            while (currentDepth < maxDepthAllowed && !initialStateForPlayout.IsTerminal())
            {
                executableActions = initialStateForPlayout.GetExecutableActions();
                var ActionNumber = RandomGenerator.Next(executableActions.Length);
                executableActions[ActionNumber].ApplyActionEffects(initialStateForPlayout);
                currentDepth++;
            }
            
            if (currentDepth > this.MaxPlayoutDepthReached) this.MaxPlayoutDepthReached = currentDepth;

            return getWorldH(initialStateForPlayout);
        }

        protected override void Backpropagate(MCTSNode node, float reward)
        {
            //ToDo, do not forget to later consider two advesary moves...
            while (node != null)
            {
                node.N += 5;
                node.Q += reward;
                node = node.Parent;

                this.TotalN += 5;
                this.TotalQ += (int)reward;
            }
        }

        protected float getWorldH(WorldModel worldModel)
        {
            int money = (int) worldModel.GetProperty(Properties.MONEY);
            int hp = (int)worldModel.GetProperty(Properties.HP);
            int hpMax = (int)worldModel.GetProperty(Properties.MAXHP);
            int shield = (int)worldModel.GetProperty(Properties.ShieldHP);
            int mana = (int)worldModel.GetProperty(Properties.MANA);

            float time = (float)worldModel.GetProperty(Properties.TIME);
            int level = (int)worldModel.GetProperty(Properties.LEVEL);

            //are my stats good?
            float H1 = (hp + shield + mana) / hpMax;

            if(H1 > 1)
            {
                H1 = 1;
            }

            //is my money good?
            float H2 = money / 25;

            //money for the current time?
            float H3 = 1 - (time / 150f);

            //is my level good?
            float H4 = level / 3;

            //is the final boss dead?
            float H5 = 0;
            if (!(bool)worldModel.GetProperty(Properties.MIGHTDRAGON)) H5 = 1f;

            if(hp <= 0)
            {
                return 0;
            }

            return H1 + H2 + H3 + H4 + H5;
        }

    }
}
