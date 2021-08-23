using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATGStateMachine;
using DG.Tweening;

public class DefaultSlicedObjectState : BaseStatement<SlicedObject>
{
    private const float _moveDuration = 0.35f;

    private DefaultTransform _defaultTransform;

    public DefaultSlicedObjectState(SlicedObject sliceObj, IStateSwitcher stateSwitcher, DefaultTransform selectedTransform)
       : base(sliceObj, stateSwitcher)
    {
        _defaultTransform = selectedTransform;
    }

    public override void Enter()
    {
        _mainObject.IsZRotate = true;
        if (Vector3.Distance(_mainObject.transform.position, _defaultTransform.position) > 0.01f)
        {
            DOTween.Sequence()
                .Append(_mainObject.transform.DOMove(_defaultTransform.position, _moveDuration))
                .Join(_mainObject.transform.DORotate(_defaultTransform.rotation, _moveDuration))
                .OnComplete(() => 
                {
                    _mainObject.SliceElementList.ForEach(element =>
                    {
                        element.SetShadow(true);
                        element.OnState();
                    });
                    GameManager.Instance.FindManager<UIManager>().Up();
                });
        }
        else if(_mainObject.SliceElementList.Count == 1)
        {
            _mainObject.SliceElementList.ForEach(slice =>
                {
                    slice.IsStopSwitchState = true;
                    slice.Speed = 0f;
                    GameManager.Instance.FindManager<InputManager>().IsActive = false;
                });
            _mainObject.WaitToDebrief(() => _stateSwitcher.StateSwitcher<DebriefSlicedObjectState>());
        }
    }

    public override void Execute()
    {
        _mainObject._composition.Update(_mainObject.SliceElementList);

        if (_mainObject.SliceElementList.Count == 1)
        {
            _mainObject.SliceElementList.ForEach(slice =>
            {
                slice.IsStopSwitchState = true;
                slice.Speed = 0f;
                GameManager.Instance.FindManager<InputManager>().IsActive = false;
            });
            _mainObject.WaitToDebrief(() => _stateSwitcher.StateSwitcher<DebriefSlicedObjectState>());
        }
    }
}
