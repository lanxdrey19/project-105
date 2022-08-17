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

    public float pointCurlThreshold = 0.5f;
    public float pointStraightThreshold = 0.2f;

    public float pinchIndexThreshold = .25f;
    public float pinchThumbThreshold = .45f;
    public float jointsTogetherDistance = .05f;

    public TextMeshPro currentGestureText;
    public TextMeshPro angleText;
    public TextMeshPro fingerPosText;

    protected Handedness rightHand = Handedness.Right;
    protected Handedness leftHand = Handedness.Left;

    public SummonToFinger anchor;

    // Start is called before the first frame update
    void Start()
    {
    }

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
        }
        else if (isThumbs("Down"))
        {
            currentGestureText.SetText("Thumbs Down");
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
        }
        else
        {
            currentGestureText.SetText("NONE");
        }
    }
    protected void fingerAngle()
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
    protected Vector3 getFingerPos(Handedness hand)
    {

        HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out MixedRealityPose indexTipPose);


        string s = string.Format("Right Index Position = {0}", indexTipPose.Position);

        fingerPosText.SetText(s);

        return indexTipPose.Position;
    }
    protected bool isL(Handedness hand)
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

    protected bool isRect()
    {
        if (isL(leftHand) && isL(rightHand)){
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
    protected bool isThumbs(string direction)
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
            float thumbCameraAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbCameraAngle < 60;

        }
        return false;
    }
    private bool isThumbDown(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out MixedRealityPose thumbTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, hand, out MixedRealityPose thumbProximalPose))
        {
            Vector3 thumbDirection = thumbTipPose.Position - thumbProximalPose.Position;
            float thumbCameraAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbCameraAngle > 120;

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
                float indexCameraAngle = Vector3.Angle(indexDirection, CameraCache.Main.transform.up);

                return indexCameraAngle > 120;

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
}
