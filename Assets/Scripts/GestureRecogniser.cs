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

    public float LCurlThreshold = 0.5f;
    public float LIndexThreshold = 0.1f;
    public float LThumbThreshold = 0.3f;

    public float thumbsUpCurlThreshold = 0.6f;
    public float thumbsUpStraightThreshold = 0.3f;
    public float upAngleThreshold = 30;
    public float downAngleThreshold = 150;

    public float pointCurlThreshold = 0.5f;
    public float pointStraightThreshold = 0.2f;

    public float pinchIndexThreshold = 0.25f;
    public float pinchThumbThreshold = 0.45f;
    public float jointsTogetherDistance = 0.05f;

    public float fistFingerThreshold = 0.6f;
    public float fistThumbThreshold = 0.5f;

    public float openFingerThreshold = 0.1f;
    public float openThumbThreshold = 0.3f;

    public float facingAwayAngleThreshold = 150f;

    public TextMeshPro currentGestureText;
    public TextMeshPro angleText;
    public TextMeshPro fingerPosText;

    protected Handedness rightHand = Handedness.Right;
    protected Handedness leftHand = Handedness.Left;

    public SummonToFinger anchor;
    public SummonToFinger width1;
    public SummonToFinger width2;

    public GameObject approveDialog;
    public GameObject rejectDialog;
    public GameObject demoBuilding;
    public GameObject startScenarioButton;
    public GameObject returnToSceneButton;



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
                fingerAngle();
                getFingerPos(rightHand);
                isDoneExecuting = true;
            }
        }
    }
    protected virtual void gestureRecogniser()
    {
        if (isThumbs("Up"))
        {
            currentGestureText.SetText("Thumbs Up");
            approveDialog.SetActive(true);
        }
        else if (isThumbs("Down"))
        {
            currentGestureText.SetText("Thumbs Down");
            rejectDialog.SetActive(true);
        }
        else if (isRect())
        {
            currentGestureText.SetText("Rectangle");
        }
        else if (isL(rightHand))
        {
            currentGestureText.SetText("L Right");
        }
        else if (isL(leftHand))
        {
            currentGestureText.SetText("L Left");
        }
        else if (isPointDown(rightHand))
        {
            currentGestureText.SetText("Pointing Down Right");
            anchor.Summon(getFingerPos(rightHand));
        }
        else if (isPointDown(leftHand))
        {
            currentGestureText.SetText("Pointing Down Left");
            anchor.Summon(getFingerPos(leftHand));
        }
        else if (isIndexPointed(rightHand) || isIndexPointed(leftHand))
        {
            currentGestureText.SetText("Pointing");
        }
        else if (isDoublePinch())
        {
            currentGestureText.SetText("Double Pinch");
            width1.Summon(getFingerPos(leftHand));
            width2.Summon(getFingerPos(rightHand));
        }
        else if (isAwayFist(rightHand) || isAwayFist(leftHand))
        {
            currentGestureText.SetText("Away Fist");
            demoBuilding.SetActive(false);
            startScenarioButton.SetActive(false);
            returnToSceneButton.SetActive(true);
        }
        else if (isAwayOpen(rightHand) || isAwayOpen(leftHand))
        {
            currentGestureText.SetText("Away Open");
            demoBuilding.SetActive(true);
            startScenarioButton.SetActive(true);
            returnToSceneButton.SetActive(false);
        }
        else
        {
            currentGestureText.SetText("NONE");
        }
    }
    private void fingerAngle()
    {
        float Lthumb = HandPoseUtils.ThumbFingerCurl(leftHand);
        float Lindex = HandPoseUtils.IndexFingerCurl(leftHand);
        float Lmiddle = HandPoseUtils.MiddleFingerCurl(leftHand);
        float Lring = HandPoseUtils.RingFingerCurl(leftHand);
        float Lpinky = HandPoseUtils.PinkyFingerCurl(leftHand);

        float Rthumb = HandPoseUtils.ThumbFingerCurl(rightHand);
        float Rindex = HandPoseUtils.IndexFingerCurl(rightHand);
        float Rmiddle = HandPoseUtils.MiddleFingerCurl(rightHand);
        float Rring = HandPoseUtils.RingFingerCurl(rightHand);
        float Rpinky = HandPoseUtils.PinkyFingerCurl(rightHand);

        string s = string.Format("Lthumb = {0}, Lindex = {1}, Lmiddle = {2}, Lring = {3}, Lpinky = {4}\nRthumb = {5}, Rindex = {6}, Rmiddle = {7}, Rring = {8}, Rpinky = {9}",
            Lthumb.ToString("n2"),
            Lindex.ToString("n2"),
            Lmiddle.ToString("n2"),
            Lring.ToString("n2"),
            Lpinky.ToString("n2"),
            Rthumb.ToString("n2"),
            Rindex.ToString("n2"),
            Rmiddle.ToString("n2"),
            Rring.ToString("n2"),
            Rpinky.ToString("n2"));

        angleText.SetText(s);
    }
    private Vector3 getFingerPos(Handedness hand)
    {

        HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out MixedRealityPose indexTipPose);


        string s = string.Format("Right Index Position = {0}", indexTipPose.Position);

        fingerPosText.SetText(s);

        return indexTipPose.Position;
    }
    private bool isL(Handedness hand)
    {
        if (HandPoseUtils.ThumbFingerCurl(hand) <= LThumbThreshold &&
            HandPoseUtils.IndexFingerCurl(hand) <= LIndexThreshold &&
            HandPoseUtils.MiddleFingerCurl(hand) >= LCurlThreshold &&
            HandPoseUtils.RingFingerCurl(hand) >= LCurlThreshold &&
            HandPoseUtils.PinkyFingerCurl(hand) >= LCurlThreshold)
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
        if (HandPoseUtils.ThumbFingerCurl(rightHand) <= thumbsUpStraightThreshold &&
            HandPoseUtils.IndexFingerCurl(rightHand) > thumbsUpCurlThreshold &&
            HandPoseUtils.MiddleFingerCurl(rightHand) > thumbsUpCurlThreshold &&
            HandPoseUtils.RingFingerCurl(rightHand) > thumbsUpCurlThreshold &&
            HandPoseUtils.PinkyFingerCurl(rightHand) > thumbsUpCurlThreshold)
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
    // 0 - 60, thumbdpwn is 120- 180
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
        if (//HandPoseUtils.ThumbFingerCurl(hand) > pointCurlThreshold &&
            HandPoseUtils.IndexFingerCurl(hand) <= pointStraightThreshold &&
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
            HandPoseUtils.IndexFingerCurl(hand) >= fistFingerThreshold &&
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
