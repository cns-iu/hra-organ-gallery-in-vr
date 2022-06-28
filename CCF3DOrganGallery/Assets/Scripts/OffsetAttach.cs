using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OffsetAttach : XRGrabInteractable
{
    private Vector3 interactorPosition = Vector3.zero;
    private Quaternion interactorRotation = Quaternion.identity;
    
    protected void OnSelectEnter(XRBaseInteractor interactor)
    {
        //base.OnSelectEnter(interactor);
        StoreInteractor(interactor);
        MatchAttachmentPoints(interactor);
    }

    private void StoreInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = interactor.attachTransform.localPosition;
        interactorRotation = interactor.attachTransform.localRotation;
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
        interactor.attachTransform.localPosition = interactorPosition;
        interactorRotation = interactor.attachTransform.localRotation;
    }

    private void ClearInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = Vector3.zero;
        interactorRotation = Quaternion.identity;
    }
}