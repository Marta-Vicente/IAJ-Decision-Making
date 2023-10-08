using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.Action;
using UnityEditor.Experimental.GraphView;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTS
    {
        public const float C = 1.4f;
        public bool InProgress { get; private set; }
        public int MaxIterations { get; set; }
        public int MaxIterationsPerFrame { get; set; }
        public int MaxPlayoutDepthReached { get; private set; }
        public int MaxSelectionDepthReached { get; private set; }
        public float TotalProcessingTime { get; private set; }
        public MCTSNode BestFirstChild { get; set; }
        public List<Action> BestActionSequence { get; private set; }
        public WorldModel BestActionSequenceEndState { get; private set; }
        protected int CurrentIterations { get; set; }
        protected int CurrentDepth { get; set; }
        protected int FrameCurrentIterations { get; set; }
        protected CurrentStateWorldModel InitialState { get; set; }
        protected MCTSNode InitialNode { get; set; }
        protected System.Random RandomGenerator { get; set; }

        public CurrentStateWorldModel CurrentStateWorldModel { get; set; }
        
        public float CurrentIterationsInFrame { get; set; }

        public WorldModel BestActionSequenceWorldState;


        public MCTS(CurrentStateWorldModel currentStateWorldModel)
        {
            this.InProgress = false;
            this.CurrentStateWorldModel = currentStateWorldModel;
            this.MaxIterations = 1000;
            this.MaxIterationsPerFrame = 100;
            this.RandomGenerator = new System.Random();
            this.InitialState = currentStateWorldModel;
        }


        public void InitializeMCTSearch()
        {
            this.InitialState.Initialize();
            this.MaxPlayoutDepthReached = 0;
            this.MaxSelectionDepthReached = 0;
            this.CurrentIterations = 0;
            this.CurrentIterationsInFrame = 0;
            this.TotalProcessingTime = 0.0f;
 
            // create root node n0 for state s0
            this.InitialNode = new MCTSNode(this.InitialState)
            {
                Action = null,
                Parent = null,
                PlayerID = 0
            };
            this.InProgress = true;
            this.BestFirstChild = null;
            this.BestActionSequence = new List<Action>();
        }

        public Action ChooseAction()
        {
            //InitializeMCTSearch();
            MCTSNode selectedNode;
            float reward;

            var startTime = Time.realtimeSinceStartup;

            int i = 50;
            while(i > 0)
            {
                var node1 = Selection(InitialNode);
                reward = Playout(node1.State);
                Backpropagate(node1, reward);
                i--;
            }

            return BestAction(InitialNode);
        }

        // Selection and Expantion
        protected MCTSNode Selection(MCTSNode initialNode)
        {
            Action nextAction;
            MCTSNode currentNode = initialNode;
            MCTSNode bestChild;

            while (!currentNode.State.IsTerminal())
            {
                nextAction = currentNode.State.GetNextAction();
                if (currentNode.ChildNodes.Count == 0)
                {
                    return Expand(currentNode, nextAction);
                }
                else
                {
                    currentNode = BestChild(currentNode);
                }
            }

            //   while(!currentNode.State.IsTerminal())

                // nextAction = currentNode.State.GetNextAction();

                //Expansion

            return currentNode;
        }

        protected virtual float Playout(WorldModel initialStateForPlayout)
        {
            Action[] executableActions;

            while (!initialStateForPlayout.IsTerminal())
            {
                executableActions = initialStateForPlayout.GetExecutableActions();
                var ActionNumber = RandomGenerator.Next(executableActions.Length);
                executableActions[ActionNumber].ApplyActionEffects(initialStateForPlayout);
            }

            return initialStateForPlayout.GetScore();
        }

        protected virtual void Backpropagate(MCTSNode node, float reward)
        {
            //ToDo, do not forget to later consider two advesary moves...

            while(node != null)
            {
                node.N += 1;
                node.Q += reward /* plus utc stuff */;
                node = node.Parent;
            }
        }

        protected MCTSNode Expand(MCTSNode parent, Action action)
        {
            WorldModel newState = parent.State.GenerateChildWorldModel();

            action.ApplyActionEffects(newState);

            MCTSNode child = new MCTSNode(newState);

            child.Action = action;

            parent.ChildNodes.Add(child);

            return child;
        }

        protected virtual MCTSNode BestUCTChild(MCTSNode node)
        {
            //ToDo
            return null;
        }

        //this method is very similar to the bestUCTChild, but it is used to return the final action of the MCTS search, and so we do not care about
        //the exploration factor
        protected MCTSNode BestChild(MCTSNode node)
        {
            MCTSNode bestChild = node.ChildNodes[0];
            float bestValue = 0;
            foreach (MCTSNode child in node.ChildNodes)
            {
                if(child.N > 0 && child.Q / child.N > bestValue)
                {
                    bestChild = child;
                    bestValue = child.Q / child.N;
                }
            }
            return bestChild;
        }


        protected Action BestAction(MCTSNode node)
        {
            var bestChild = this.BestChild(node);
            if (bestChild == null) return null;

            this.BestFirstChild = bestChild;

            //this is done for debugging proposes only
            this.BestActionSequence = new List<Action>();
            this.BestActionSequence.Add(bestChild.Action);
            node = bestChild;

            while(!node.State.IsTerminal())
            {
                bestChild = this.BestChild(node);
                if (bestChild == null) break;
                this.BestActionSequence.Add(bestChild.Action);
                node = bestChild;
                this.BestActionSequenceWorldState = node.State;
            }

            return this.BestFirstChild.Action;
        }

    }
}
