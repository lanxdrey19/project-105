using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

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
    public float jointsTogetherDistance = 0.1f;

    public float fistFingerThreshold = 0.7f;
    public float fistThumbThreshold = 0.6f;
    public float fistIndexThreshold = 0.5f;

    public float openFingerThreshold = 0.1f;
    public float openThumbThreshold = 0.5f;

    public float facingAwayAngleThreshold = 135f;

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
            if (isDoneExecuting)
            {
                isDoneExecuting = false;
                gestureRecogniser();
                isDoneExecuting = true;
            }
        }
    }
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
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, rightHand, out MixedRealityPose thumbProxPoseRight);
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, leftHand, out MixedRealityPose thumbProxPoseLeft);
            area1.Summon(getFingerPos(leftHand));
            area2.Summon(thumbProxPoseLeft.Position);
            area3.Summon(getFingerPos(rightHand));
            area4.Summon(thumbProxPoseRight.Position);
            areaManager.SetActive(true);
            anchorManager.SetActive(false);
            distanceManager.SetActive(false);
            fires.SetActive(true);
            Vector3 position = Camera.main.transform.position;
            fires.transform.position = position;
            // control the position of where the fire spawns relative to the camera
            fires.transform.Translate(0.15f, 0.1f, 0.1f);
        }
        if (isPointDown(rightHand))
        {
            anchor.Summon(getFingerPos(rightHand));
            anchorManager.SetActive(true);
            areaManager.SetActive(false);
            distanceManager.SetActive(false);
        }
        if (isPointDown(leftHand))
        {
            anchor.Summon(getFingerPos(leftHand));
            anchorManager.SetActive(true);
            areaManager.SetActive(false);
            distanceManager.SetActive(false);
        }
        if (isDoublePinch())
        {
            width1.Summon(getFingerPos(leftHand));
            width2.Summon(getFingerPos(rightHand));
            distanceManager.SetActive(true);
            areaManager.SetActive(false);
            anchorManager.SetActive(false);
        }
        if (isAwayFist(rightHand) || isAwayFist(leftHand))
        {
            demoBuilding.SetActive(false);
            changeSceneBtn.SetActive(false);
        }
        if (isAwayOpen(rightHand) || isAwayOpen(leftHand))
        {
            demoBuilding.SetActive(true);
            fires.SetActive(false);
            changeSceneBtn.SetActive(true);
        }
    }

    private Vector3 getFingerPos(Handedness hand)
    {

        HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out MixedRealityPose indexTipPose);

        return indexTipPose.Position;
    }
    private bool isL(Handedness hand)
    {
        if (HandPoseUtils.ThumbFingerCurl(hand) <= LThumbThreshold &&
            HandPoseUtils.IndexFingerCurl(hand) <= LIndexThreshold)
        {
            return true;
        }
        return false;
    }

    private bool isRect()
    {
        if (isL(leftHand) && isL(rightHand))
        {
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, rightHand, out MixedRealityPose rightIndexTipPose);
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, leftHand, out MixedRealityPose leftIndexTipPose);
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, rightHand, out MixedRealityPose rightThumbTipPose);
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, leftHand, out MixedRealityPose leftThumbTipPose);
            if (Vector3.Distance(rightIndexTipPose.Position, leftThumbTipPose.Position) <= jointsTogetherDistance &&
                Vector3.Distance(rightThumbTipPose.Position, leftIndexTipPose.Position) <= jointsTogetherDistance)
            {
                return true;
            }
        }

        return false;
    }
    private bool isThumbs(string direction)
    {
        if (HandPoseUtils.ThumbFingerCurl(rightHand) <= thumbsUpThumbThreshold &&
            HandPoseUtils.IndexFingerCurl(rightHand) > thumbsUpFingerThreshold &&
            HandPoseUtils.MiddleFingerCurl(rightHand) > thumbsUpFingerThreshold &&
            HandPoseUtils.RingFingerCurl(rightHand) > thumbsUpFingerThreshold &&
            HandPoseUtils.PinkyFingerCurl(rightHand) > thumbsUpFingerThreshold)
        {
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
    private bool isThumbUp(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out MixedRealityPose thumbTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, hand, out MixedRealityPose thumbProximalPose))
        {
            Vector3 thumbDirection = thumbTipPose.Position - thumbProximalPose.Position;
            float thumbAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbAngle < upAngleThreshold;

        }
        return false;
    }
    private bool isThumbDown(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out MixedRealityPose thumbTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, hand, out MixedRealityPose thumbProximalPose))
        {
            Vector3 thumbDirection = thumbTipPose.Position - thumbProximalPose.Position;
            float thumbAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbAngle > downAngleThreshold;

        }
        return false;
    }

    private bool isIndexPointed(Handedness hand)
    {
        if (HandPoseUtils.IndexFingerCurl(hand) <= pointStraightThreshold &&
            HandPoseUtils.MiddleFingerCurl(hand) > pointCurlThreshold &&
            HandPoseUtils.RingFingerCurl(hand) > pointCurlThreshold &&
            HandPoseUtils.PinkyFingerCurl(hand) > pointCurlThreshold)
        {
            return true;
        }
        return false;
    }

    private bool isPointDown(Handedness hand)
    {
        if (isIndexPointed(hand))
        {
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out MixedRealityPose indexTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out MixedRealityPose indexKnucklePose))
            {
                Vector3 indexDirection = indexTipPose.Position - indexKnucklePose.Position;
                float indexAngle = Vector3.Angle(indexDirection, CameraCache.Main.transform.up);

                return indexAngle > downAngleThreshold;

            }
        }

        return false;
    }

    private bool isDoublePinch()
    {
        HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, rightHand, out MixedRealityPose rightIndexTipPose);
        HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, leftHand, out MixedRealityPose leftIndexTipPose);
        if (HandPoseUtils.ThumbFingerCurl(rightHand) >= pinchThumbThreshold &&
            HandPoseUtils.ThumbFingerCurl(leftHand) >= pinchThumbThreshold &&
            HandPoseUtils.IndexFingerCurl(rightHand) >= pinchIndexThreshold &&
            HandPoseUtils.IndexFingerCurl(leftHand) >= pinchIndexThreshold &&
            Vector3.Distance(rightIndexTipPose.Position, leftIndexTipPose.Position) <= jointsTogetherDistance)
        {
            return true;
        }

        return false;
    }

    private bool isAwayFist(Handedness hand)
    {
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

    private bool isAwayOpen(Handedness hand)
    {
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
