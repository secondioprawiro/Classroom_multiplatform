using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowVisual : MonoBehaviour
{
    public Transform visualtarget;
    public Vector3 localAxis;
    public float resetspeed = 3;
    private float followAngle;
    private Vector3 initialLocalPos;

    private bool freeze = false;

    private Vector3 offset;
    private Transform pokeAttachTransform;

    private XRBaseInteractable interactable;
    private bool isFollowing = false;
    // Start is called before the first frame update
    void Start()
    {
        initialLocalPos = visualtarget.localPosition;
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if(hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            isFollowing = true;

            pokeAttachTransform = interactor.attachTransform;
            offset = visualtarget.position - pokeAttachTransform.position;

            float pokeAngle = Vector3.Angle(offset, visualtarget.TransformDirection(localAxis));
            if(pokeAngle < followAngle)
            {
                isFollowing = true;
                freeze = false;
            }
        }
    }
    public void Reset(BaseInteractionEventArgs hover)
    {
        if(hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
            freeze = false;
        }
    }

    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            freeze = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (freeze)
            return;
        if (isFollowing)
        {
            Vector3 localTargetPosition = visualtarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);
            visualtarget.position = visualtarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            visualtarget.localPosition = Vector3.Lerp(visualtarget.localPosition, initialLocalPos, Time.deltaTime * resetspeed);
        }
    }
}
