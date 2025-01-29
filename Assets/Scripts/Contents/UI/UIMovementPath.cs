using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovementPath : MonoBehaviour
{
    [SerializeField]
    private LineRenderer movementPathObject;

    [SerializeField]
    private GameObject startPointObject;

    [SerializeField]
    private GameObject destinationPointObject;

    private Vector3 startPoint;
    private Vector3 destinationPoint;

    public void SetStartPoint(Vector3 startPoint)
    {
        this.startPoint = startPoint;
        movementPathObject.SetPosition(0, startPoint);
        startPointObject.transform.localPosition = startPoint;
    }

    public void SetDestination(Vector3 destinationPoint)
    {
        this.destinationPoint = destinationPoint;
        movementPathObject.SetPosition(1, destinationPoint);
        destinationPointObject.transform.localPosition = destinationPoint;

        if (startPoint == destinationPoint)
            destinationPointObject.SetActive(false);
        else
            destinationPointObject.SetActive(true);
    }
}
