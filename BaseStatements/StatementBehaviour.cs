using System.Collections.Generic;
using UnityEngine;
using System;

namespace ATGStateMachine
{
    /// <summary>
    /// The base class of any object that can have several states
    /// _currentState contains the current state
    /// _allStates contains all possible states for a given object
    /// </summary> 
    /// <typeparam name="T"></typeparam>
    public class StatementBehaviour<T> : MonoBehaviour, IStateSwitcher
    {
        protected BaseStatement<T> _currentState = null;
        private BaseStatement<T> _waitingState;


        protected HashSet<BaseStatement<T>> _allStates = new HashSet<BaseStatement<T>>();
        

        public virtual void OnState(bool isExit = false)
        {
            if (_currentState != null)
            {
                _currentState.Enter();
            }
        }
        public virtual void OnExecute()
        {
            if(_currentState != null)
            {
                _currentState.Execute();
            }
        }
        public virtual void OnContinueState()
        {
            if (_waitingState != null && _currentState == null)
            {
                _currentState = _waitingState;
                _waitingState = null;
            }
            else Debug.LogError("Waiting state in null !");
        }
        public virtual void OnPauseState()
        {
            if(_currentState != null)
            {
                _waitingState = _currentState;
                _currentState = null;
            }
        }
        public virtual void OnStopState()
        {
            if(_currentState != null)
            {
                _currentState.Exit();
                _currentState = null;

                _allStates.Clear();
            }
        }

        // switch object state
        public virtual void StateSwitcher<K>()
        {
            BaseStatement<T> state = null;

            foreach(var s in _allStates)
            {
                if(s is K)
                {
                    state = s as BaseStatement<T>;
                }
            }

            if (state != null)
            {
                if (state != _currentState)
                {
                    if (_currentState != null)
                    {
                        _currentState.Exit();
                    }
                    state.Enter();
                }
                _currentState = state;
            }
            else
            {
                throw new NullReferenceException($"Cant find state {typeof(K)}");
            }
        }
    }
}
