using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

// The main script used to check if any gestures are being performed based on provided thresholds
public class GestureRecogniser : MonoBehaviour
{
    private bool isDoneExecuting = true;

    public float LIndexThreshold = 0.1f;
    public float LThumbThreshold = 0.35f;

    public float thumbsUpFingerThreshold = 0.7f;
    public float thumbsUpThumbThreshold = 0.25f;

    public float upAngleThreshold = 30;
    public float downAngleThreshold = 135; // needs to be more lenient as tracking breaks when hand is behind arm

    public float pointCurlThreshold = 0.5f;
    public float pointStraightThreshold = 0.2f;

    public float pinchIndexThreshold = 0.25f;
    public float pinchThumbThreshold = 0.45f;
    public float jointsTogetherDistance = 0.05f;

    public float fistFingerThreshold = 0.7f;
    public float fistThumbThreshold = 0.6f;
    public float fistIndexThreshold = 0.5f;

    public float openFingerThreshold = 0.1f;
    public float openThumbThreshold = 0.5f;

    public float facingAwayAngleThreshold = 100f; // needs to be smaller because the angle is not exact if hand is to the side

    protected Handedness rightHand = Handedness.Right;
    protected Handedness leftHand = Handedness.Left;

    public SummonToFinger anchor;

    public SummonToFinger width1;
    public SummonToFinger width2;

    public SummonToFinger area1;
    public SummonToFinger area2;
    public SummonToFinger area3;
    public SummonToFinger area4;

    public GameObject approveDialog;
    public GameObject rejectDialog;
    public GameObject demoBuilding;

    public GameObject anchorManager;
    public GameObject distanceManager;
    public GameObject areaManager;

    public GameObject fires;

    public GameObject changeSceneBtn;


    // Update is called once per frame
    void Update()
    {
        // Check if hand in view
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Both, out MixedRealityPose pose))
        {
            // Only starts detecting for new gestures once current function has finished excecuting
            if (isDoneExecuting)
            {
                isDoneExecuting = false;
                gestureRecogniser();
                isDoneExecuting = true;
            }
        }
    }

    // The main function for determining if a gesture is currently active and calls the corresponding functions
    protected virtual void gestureRecogniser()
    {
        if (isThumbs("Up"))
        {
            approveDialog.SetActive(true);
        }
        if (isThumbs("Down"))
        {
            rejectDialog.SetActive(true);
        }
        if (isRect())
        {
            // Obtains the positional data for the ThumbProximalJoint and IndexTip on each hand
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, rightHand, out MixedRealityPose thumbProxPoseRight);
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, leftHand, out MixedRealityPose thumbProxPoseLeft);

            // Summons the area nodes to the specified joints
            area1.Summon(getFingerPos(leftHand));
            area2.Summon(thumbProxPoseLeft.Position);
            area3.Summon(getFingerPos(rightHand));
            area4.Summon(thumbProxPoseRight.Position);

            // Display the area tool while hiding others
            areaManager.SetActive(true);
            anchorManager.SetActive(false);
            distanceManager.SetActive(false);
        }
        if (isPointDown(rightHand))
        {
            // Summons anchor node to specified finger tip
            anchor.Summon(getFingerPos(rightHand));

            // Display the anchor tool while hiding others
            anchorManager.SetActive(true);
            areaManager.SetActive(false);
            distanceManager.SetActive(false);
        }
        if (isPointDown(leftHand))
        {
            // Summons anchor node to specified finger tip
            anchor.Summon(getFingerPos(leftHand));

            // Display the anchor tool while hiding others
            anchorManager.SetActive(true);
            areaManager.SetActive(false);
            distanceManager.SetActive(false);
        }
        if (isDoublePinch())
        {
            // Summon one node to each index finger tip
            width1.Summon(getFingerPos(leftHand));
            width2.Summon(getFingerPos(rightHand));

            // Display the distance tool while hiding others
            distanceManager.SetActive(true);
            areaManager.SetActive(false);
            anchorManager.SetActive(false);

            // start the fire behind user
            fires.SetActive(true);
            fires.GetComponent<FirePos>().Summon();
        }
        if (isAwayFist(rightHand) && isAwayFist(leftHand))
        {
            // Hides all virtual elements
            demoBuilding.SetActive(false);
            changeSceneBtn.SetActive(false);
            distanceManager.SetActive(false);
            areaManager.SetActive(false);
            anchorManager.SetActive(false);

            // Put out the fire
            fires.SetActive(false);
        }
        if (isAwayOpen(rightHand) && isAwayOpen(leftHand))
        {
            // Unhides building model and change scene button
            demoBuilding.SetActive(true);
            changeSceneBtn.SetActive(true);

            // Put out the fire
            fires.SetActive(false);
            
        }
    }


    // Obtains the IndexTip joint position of specified hand
    private Vector3 getFingerPos(Handedness hand)
    {

        HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out MixedRealityPose indexTipPose);

        return indexTipPose.Position;
    }

    // Detects if the user is making an L gesture with one of their hands
    private bool isL(Handedness hand)
    {
        if (HandPoseUtils.ThumbFingerCurl(hand) <= LThumbThreshold &&
            HandPoseUtils.IndexFingerCurl(hand) <= LIndexThreshold)
            // Value of other fingers do not matter
        {
            return true;
        }
        return false;
    }

    // Detects if the user is making a rectangle gesture
    private bool isRect()
    {
        if (isL(leftHand) && isL(rightHand))
        {
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, rightHand, out MixedRealityPose rightIndexTipPose);
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, leftHand, out MixedRealityPose leftIndexTipPose);
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, rightHand, out MixedRealityPose rightThumbTipPose);
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, leftHand, out MixedRealityPose leftThumbTipPose);

            // Detects if the index on the right hand is within range of the thumb on the left and vicec versa
            if (Vector3.Distance(rightIndexTipPose.Position, leftThumbTipPose.Position) <= jointsTogetherDistance &&
                Vector3.Distance(rightThumbTipPose.Position, leftIndexTipPose.Position) <= jointsTogetherDistance)
            {
                return true;
            }
        }

        return false;
    }

    // Detects if the user is making a thumbs up/down gesture
    private bool isThumbs(string direction)
    {
        if (HandPoseUtils.ThumbFingerCurl(rightHand) <= thumbsUpThumbThreshold &&
            HandPoseUtils.IndexFingerCurl(rightHand) > thumbsUpFingerThreshold &&
            HandPoseUtils.MiddleFingerCurl(rightHand) > thumbsUpFingerThreshold &&
            HandPoseUtils.RingFingerCurl(rightHand) > thumbsUpFingerThreshold &&
            HandPoseUtils.PinkyFingerCurl(rightHand) > thumbsUpFingerThreshold)
        {
            // Check thumb direction
            if (direction == "Up")
            {
                return isThumbUp(rightHand);
            }
            else if (direction == "Down")
            {
                return isThumbDown(rightHand);
            }
        }

        return false;
    }

    // Detects if the user is making thumbs up gesture
    private bool isThumbUp(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out MixedRealityPose thumbTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, hand, out MixedRealityPose thumbProximalPose))
        {
            // Determine angle by comparing to camera up transform
            Vector3 thumbDirection = thumbTipPose.Position - thumbProximalPose.Position;
            float thumbAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbAngle < upAngleThreshold;

        }
        return false;
    }

    // Detects if the user is making thumbs down gesture
    private bool isThumbDown(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out MixedRealityPose thumbTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, hand, out MixedRealityPose thumbProximalPose))
        {
            // Determine angle by comparing to camera up transform
            Vector3 thumbDirection = thumbTipPose.Position - thumbProximalPose.Position;
            float thumbAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbAngle > downAngleThreshold;

        }
        return false;
    }

    // Detects if user is making pointing gesture
    private bool isIndexPointed(Handedness hand)
    {
        if (// Thumb position does not matter
            HandPoseUtils.IndexFingerCurl(hand) <= pointStraightThreshold &&
            HandPoseUtils.MiddleFingerCurl(hand) > pointCurlThreshold &&
            HandPoseUtils.RingFingerCurl(hand) > pointCurlThreshold &&
            HandPoseUtils.PinkyFingerCurl(hand) > pointCurlThreshold)
        {
            return true;
        }
        return false;
    }

    // Detects if user is making pointing down gesture
    private bool isPointDown(Handedness hand)
    {
        if (isIndexPointed(hand))
        {
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out MixedRealityPose indexTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out MixedRealityPose indexKnucklePose))
            {
                // Determine angle by comparing to camera up transform
                Vector3 indexDirection = indexTipPose.Position - indexKnucklePose.Position;
                float indexAngle = Vector3.Angle(indexDirection, CameraCache.Main.transform.up);

                return indexAngle > downAngleThreshold;

            }
        }

        return false;
    }

    // Detects if the user is making a double pinch gesture
    private bool isDoublePinch()
    {
        HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, rightHand, out MixedRealityPose rightIndexTipPose);
        HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, leftHand, out MixedRealityPose leftIndexTipPose);
        if (// Other fingers on each hand do not matter
            HandPoseUtils.ThumbFingerCurl(rightHand) >= pinchThumbThreshold &&
            HandPoseUtils.ThumbFingerCurl(leftHand) >= pinchThumbThreshold &&
            HandPoseUtils.IndexFingerCurl(rightHand) >= pinchIndexThreshold &&
            HandPoseUtils.IndexFingerCurl(leftHand) >= pinchIndexThreshold &&

            // Determines if the fingers are close enough together
            Vector3.Distance(rightIndexTipPose.Position, leftIndexTipPose.Position) <= jointsTogetherDistance)
        {
            return true;
        }

        return false;
    }

    // Detects if the user is making a fist gesture with palm pointed away
    private bool isAwayFist(Handedness hand)
    {
        // Determines if palm is pointed forward by comparing to the forward camera transform
        HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out MixedRealityPose palmPose);
        float palmAngle = Vector3.Angle(palmPose.Up, CameraCache.Main.transform.forward);

        if (HandPoseUtils.ThumbFingerCurl(hand) >= fistThumbThreshold &&
            HandPoseUtils.IndexFingerCurl(hand) >= fistIndexThreshold &&
            HandPoseUtils.MiddleFingerCurl(hand) >= fistFingerThreshold &&
            HandPoseUtils.RingFingerCurl(hand) >= fistFingerThreshold &&
            HandPoseUtils.PinkyFingerCurl(hand) >= fistFingerThreshold &&
            palmAngle > facingAwayAngleThreshold)
        {
            return true;
        }
        return false;
    }

    // Detects if the user is making an open palm gesture with palm pointed away
    private bool isAwayOpen(Handedness hand)
    {
        // Determines if palm is pointed forward by comparing to the forward camera transform
        HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out MixedRealityPose palmPose);
        float palmAngle = Vector3.Angle(palmPose.Up, CameraCache.Main.transform.forward);

        if (HandPoseUtils.ThumbFingerCurl(hand) <= openThumbThreshold &&
            HandPoseUtils.IndexFingerCurl(hand) <= openFingerThreshold &&
            HandPoseUtils.MiddleFingerCurl(hand) <= openFingerThreshold &&
            HandPoseUtils.RingFingerCurl(hand) <= openFingerThreshold &&
            HandPoseUtils.PinkyFingerCurl(hand) <= openFingerThreshold &&
            palmAngle > facingAwayAngleThreshold)
        {
            return true;
        }
        return false;
    }
}
