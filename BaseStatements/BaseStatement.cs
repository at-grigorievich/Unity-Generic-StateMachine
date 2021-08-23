using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATGStateMachine
{
    /// <summary>
    /// Base class of any object state
    /// This type can manipulate data of type T
    /// And also change the state based on the conditions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseStatement<T>
    {
        protected readonly T _mainObject; // manipulate object
        protected readonly IStateSwitcher _stateSwitcher; // switch state

        protected BaseStatement(T mainObject, IStateSwitcher stateSwitcher)
        {
            _mainObject = mainObject;
            _stateSwitcher = stateSwitcher;
        }

        public abstract void Enter(); // enter the state callback
        public virtual void Exit() { } // exit the state callback
        public virtual void Execute() { } // stay the state callback
    }
}
