using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATGStateMachine;
using DG.Tweening;

public class DebriefSlicedObjectState : BaseStatement<SlicedObject>
{
    private const float _moveDuration = 0.45f;
    private DefaultTransform _debriefTransform;


    public DebriefSlicedObjectState(SlicedObject sliceObj, IStateSwitcher stateSwitcher, DefaultTransform transform)
       : base(sliceObj, stateSwitcher)
    {
        _debriefTransform = transform;
    }

    public override void Enter()
    {
        Taptic.Vibrate();

        Vector3 rot = !_mainObject.IsZRotate ? new Vector3(720f, 0f, 0) : new Vector3(0f, 0f, 720f);

        GameManager.Instance.FindManager<InputManager>().isEnable = false;
        GameManager.Instance.FindManager<UIManager>().HideReplay();

        var shadows = GameObject.FindObjectsOfType<SliceElementShadow>();
        foreach(var shadow in shadows)
        {
            shadow.ResetShadow();
        }

        _mainObject.SliceElementList.ForEach(
            slice =>
            {
                slice.End();
            });

        Transform child = _mainObject.transform.GetChild(0);
        child.SetParent(null);
        //child.SetParent(null);

        //_mainObject.transform.position = child.transform.position;

        //child.DORotate(_debriefTransform.rotation,_moveDuration);

        child.transform.rotation = Quaternion.Euler(_debriefTransform.rotation);

        var childSlice = child.GetComponent<SliceElement>();
        childSlice.ClearState();

        Vector3 endPos = child.transform.position;
        childSlice.MoveToFinish(endPos, () =>
        {
            DOTween.Sequence()

                .Append(child.transform.DOMove(_debriefTransform.position, _moveDuration))

                .OnComplete(() =>
                {
                    _mainObject.transform.position = Vector3.zero;
                    _mainObject.SliceElementList.Clear();
                    Vector3 endPos = child.transform.position;
                    DOTween.Sequence()
                        .Append(child.DOJump(endPos, 1.5f, 1, _moveDuration * 2f))
                        .Join(child.DORotate(rot, 1f + _moveDuration, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuad))
                        .AppendInterval(0.75f)
                        .OnComplete(() =>
                        {
                            GameManager.Instance.FindManager<UIManager>().ShowNextLevel();

                            _mainObject.transform.position = endPos;
                            float maxY = child.position.y + 0.05f;
                            float minY = child.position.y - 0.05f;

                            DOTween.Sequence()
                            .Append(child.transform.DOMoveY(maxY, 0.5f))
                            .Append(child.transform.DOMoveY(minY, 1.1f))
                            .SetLoops(-1, LoopType.Yoyo);
                        });

                    Confetti[] confettiArr = GameObject.FindObjectsOfType<Confetti>();
                    foreach (var conf in confettiArr)
                    {
                        conf.ShowCongetti();
                    }
                });
        });
    }
}
