using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerInput : MonoBehaviour
{
    public GameObject player;
    public Animator leftHandAnimator, rightHandAnimator;

    public Rigidbody carRigidBody;
    public Transform carForward;

    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] float rotateSpeed = 0.25f;

    public InputDevice leftController, rightController;

    Vector3 moveDirection;

    //int i = 0;
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        leftController = devices[0];
        rightController = devices[1];

        carRigidBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHands();

        if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 carMoveValue) && carMoveValue != Vector2.zero)
        {
            Debug.Log(carMoveValue);

            //car.transform.position = new Vector3(moveSpeed * carMoveValue.y + car.transform.position.x, car.transform.position.y, car.transform.position.z);

            //car.transform.position += car.transform.forward;

            //player.transform.Translate(0f, 0f, -(carMoveValue.x + carMoveValue.y) * moveSpeed);

            moveDirection = carForward.transform.forward * carMoveValue.y;

            
        }

        if (rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 carRotateValue) && carRotateValue != Vector2.zero)
        {
            //car.transform.localEulerAngles = new Vector3(car.transform.rotation.x, car.transform.rotation.y + rotateSpeed, car.transform.rotation.z);

            player.transform.Rotate(0f, carRotateValue.x * rotateSpeed, 0f);

            //Debug.Log(carRotateValue);
        }
    }
    private void FixedUpdate()
    {
        carRigidBody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
    }

    private void AnimateHands()
    {
        if (leftController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            leftHandAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            leftHandAnimator.SetFloat("Trigger", 0f);
        }

        if (leftController.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            leftHandAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            leftHandAnimator.SetFloat("Grip", 0f);
        }



        if (rightController.TryGetFeatureValue(CommonUsages.trigger, out float rTriggerValue))
        {
            rightHandAnimator.SetFloat("Trigger", rTriggerValue);
        }
        else
        {
            rightHandAnimator.SetFloat("Trigger", 0f);
        }

        if (rightController.TryGetFeatureValue(CommonUsages.grip, out float rGripValue))
        {
            rightHandAnimator.SetFloat("Grip", rGripValue);
        }
        else
        {
            rightHandAnimator.SetFloat("Grip", 0f);
        }
    }
}
