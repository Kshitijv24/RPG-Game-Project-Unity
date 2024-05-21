using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action)
        {
            if (currentAction == action) return;

            if(currentAction != null)
                currentAction.CancelAttack();

            currentAction = action;
        }

        public void CancelCurrentAction() => StartAction(null);
    }
}