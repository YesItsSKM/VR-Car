using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SteeringControls : MonoBehaviour
{
    // Right hand
    public GameObject rightHand;
        private Transform rightHandOriginalParent;
        private bool rightHandOnWheel = false;

    // Left hand
    public GameObject leftHand;
        private Transform leftHandOriginalParent;
        private bool leftHandOnWheel = false;

    public Transform[] snapPositions;

    // Vehicle
    public GameObject car;
    public Rigidbody carRigidBody;

    public float currentWheelRotation = 0f;

    //[SerializeField] float turnDampening = 1000f;

    public Transform directionalObject;

    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Steering Script Started");
    }

    // Update is called once per frame
    void Update()
    {
        ReleaseHandsFromWheel();
        ConvertWheelRotation();
        //TurnCar();

        currentWheelRotation = -transform.rotation.eulerAngles.x;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            Debug.Log(other.name + " trigger entered.");

            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            Debug.Log(other.name + "trigger exited.");
        }
    }
    */

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            //Debug.Log(other.name + "entered.");

            if((rightHandOnWheel == false) && playerInput.rightController.TryGetFeatureValue(CommonUsages.grip, out float gripValue) && gripValue > 0f)
            {
                //Debug.Log("Right Grip Value:" + gripValue);
                PlaceHandOnWheel(ref rightHand, ref rightHandOriginalParent, ref rightHandOnWheel);
            }

            if ((leftHandOnWheel == false) && playerInput.leftController.TryGetFeatureValue(CommonUsages.grip, out float lGripValue) && lGripValue > 0f)
            {
                //Debug.Log("Left Grip Value:" + lGripValue);
                PlaceHandOnWheel(ref leftHand, ref leftHandOriginalParent, ref leftHandOnWheel);
            }
        }
    }

    void PlaceHandOnWheel(ref GameObject hand, ref Transform originalParent, ref bool handOnWheel)
    {
        var shortestDistance = Vector3.Distance(snapPositions[0].position, hand.transform.position);
        var bestSnapPosition = snapPositions[0];

        foreach (var snapPosition in snapPositions)
        {
            if (snapPosition.childCount == 0)
            {
                var distance = Vector3.Distance(snapPosition.position, hand.transform.position);

                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestSnapPosition = snapPosition;
                }
            }
        }

        originalParent = hand.transform.parent;

        hand.transform.parent = bestSnapPosition.transform;
        hand.transform.position = bestSnapPosition.transform.position;

        handOnWheel = true;
    }

    void ReleaseHandsFromWheel()
    {
        if (rightHandOnWheel && playerInput.rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripR) && !gripR)
        {
            rightHand.transform.parent = rightHandOriginalParent;
            rightHand.transform.position = rightHandOriginalParent.position;
            rightHand.transform.rotation = rightHandOriginalParent.rotation;
            rightHand.transform.Rotate(0f, 0f, -90f);
            rightHandOnWheel = false;
        }

        if (leftHandOnWheel && playerInput.leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripL) && !gripL)
        {
            leftHand.transform.parent = leftHandOriginalParent;
            leftHand.transform.position = leftHandOriginalParent.position;
            leftHand.transform.rotation = leftHandOriginalParent.rotation;
            leftHand.transform.Rotate(0f, 0f, 90f);
            leftHandOnWheel = false;
        }

        if (!leftHandOnWheel && !rightHandOnWheel)
        {
            transform.parent = transform.root;
        }
    }

    void ConvertWheelRotation()
    {
        if (rightHandOnWheel && !leftHandOnWheel)
        {
            Quaternion newRot = Quaternion.Euler(rightHandOriginalParent.transform.rotation.eulerAngles.x, rightHandOriginalParent.transform.rotation.eulerAngles.y, 103.58f);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        }

        if (!rightHandOnWheel && leftHandOnWheel)
        {
            Quaternion newRot = Quaternion.Euler(leftHandOriginalParent.transform.rotation.eulerAngles.x, leftHandOriginalParent.transform.rotation.eulerAngles.y, 103.58f);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        }

        if (rightHandOnWheel && leftHandOnWheel)
        {
            Quaternion newRotLeft = Quaternion.Euler(leftHandOriginalParent.transform.rotation.eulerAngles.x, leftHandOriginalParent.transform.rotation.eulerAngles.y, 103.58f);
            Quaternion newRotRight = Quaternion.Euler(rightHandOriginalParent.transform.rotation.eulerAngles.x, rightHandOriginalParent.transform.rotation.eulerAngles.y, 103.58f);
            Quaternion finalRot = Quaternion.Slerp(newRotLeft, newRotRight, 0.5f);

            directionalObject.rotation = finalRot;
            transform.parent = directionalObject;
        }
    }

    void TurnCar()
    {
        var turn = -transform.rotation.eulerAngles.y;

        if(turn < -350)
        {
            turn = turn + 360;
        }

        carRigidBody.MoveRotation(Quaternion.RotateTowards(car.transform.rotation, Quaternion.Euler(0f, turn, 0f), 0.5f));
        //car.transform.Rotate(0f, turn, 0f);
    }
}
