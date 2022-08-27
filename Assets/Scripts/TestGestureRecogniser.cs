using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

// Script used to display the curl values of each finger for testing purposes
public class TestGestureRecogniser : MonoBehaviour
{
    private bool isDoneExecuting = true;

    public TextMeshPro angleText;
    public TextMeshPro fingerPosText;

    private Handedness leftHand = Handedness.Left;
    private Handedness rightHand = Handedness.Right;

    // Update is called once per frame
    void Update()
    {

        // Check if hand in view
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Both, out MixedRealityPose pose))
        {
            if (isDoneExecuting)
            {
                isDoneExecuting = false;
                fingerAngle();
                isDoneExecuting = true;
            }
        }
    }

    // Obtains the curl values of each hand and displays them on the text mesh
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
}
