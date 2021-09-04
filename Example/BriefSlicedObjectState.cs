using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATGStateMachine;
using DG.Tweening;

public class BriefSlicedObjectState : BaseStatement<SlicedObject>
{
    private const float _scaleDuration = 0.45f;

    private DefaultTransform _briefTransform;

    public BriefSlicedObjectState(SlicedObject sliceObj, IStateSwitcher stateSwitcher, DefaultTransform selectedTransform)
       : base(sliceObj, stateSwitcher)
    {
        _briefTransform = selectedTransform;
    }


    public override void Enter()
    {
        _mainObject.SliceElementList.ForEach(element =>
        {
            if (!element.IsStatic)
            {
                element.SetShadow(false);
            }
        });

        _mainObject.transform.localScale = Vector3.zero;
        _mainObject.transform.position = _briefTransform.position;
        _mainObject.transform.eulerAngles = _briefTransform.rotation;

        DOTween.Sequence()
            .Append(_mainObject.transform.DOScale(1.2f * Vector3.one, _scaleDuration))
            .Append(_mainObject.transform.DOShakeRotation(0.8f, 15, 10))
            .Join(_mainObject.transform.DOScale(Vector3.one, _scaleDuration / 2f))
            .OnComplete(() =>
            {
                _stateSwitcher.StateSwitcher<DefaultSlicedObjectState>();
            });
    }
}
