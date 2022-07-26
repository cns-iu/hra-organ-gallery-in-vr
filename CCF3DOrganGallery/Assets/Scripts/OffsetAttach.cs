using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OffsetAttach : XRGrabInteractable
{
    private Vector3 _interactorPosition = Vector3.zero;
    private Quaternion _interactorRotation = Quaternion.identity;
    
    protected void OnSelectEnter(XRBaseInteractor interactor)
    {
        //base.OnSelectEnter(interactor);
        StoreInteractor(interactor);
        MatchAttachmentPoints(interactor);
    }

    private void StoreInteractor(XRBaseInteractor interactor)
    {
        _interactorPosition = interactor.attachTransform.localPosition;
        _interactorRotation = interactor.attachTransform.localRotation;
    }

    private void MatchAttachmentPoints(XRBaseInteractor interactor)
    {
        bool hasAttach = attachTransform != null;
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
    }

    protected void OnSelectExit(XRBaseInteractor interactor)
    {
        //base.OnSelectExit(interactor);
        ResetAttachmentPoint(interactor);
        ClearInteractor(interactor);
    }

    private void ResetAttachmentPoint(XRBaseInteractor interactor)
    {
        interactor.attachTransform.localPosition = _interactorPosition;
        _interactorRotation = interactor.attachTransform.localRotation;
    }

    private void ClearInteractor(XRBaseInteractor interactor)
    {
        _interactorPosition = Vector3.zero;
        _interactorRotation = Quaternion.identity;
    }
}