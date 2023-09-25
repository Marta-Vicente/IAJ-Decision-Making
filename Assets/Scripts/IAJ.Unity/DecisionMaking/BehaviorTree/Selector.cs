﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.BehaviorTree
{
    public class Selector : CompositeTask
    {
   
        public Selector(List<Task> tasks) : base(tasks)
        {
        }

        public Selector() { }

        // A Selector will return immediately with a success status code when one of its children
        // runs successfully.As long as its children are failing, it will keep on trying. If it runs out of
        // children completely, it will return a failure status code
        public override Result Run()
        {

            // TODO implement


            return Result.Failure;

        }
    }
}