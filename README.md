# Gesture Set Development for an Augmented Reality (AR) Environment for Safe Construction On-site
Samuel Liu, Lance Delos Reyes

## Overview
Augmented Reality (AR) technology has seen many use cases on construction sites [1]. However, due to the immersive nature of AR, the technology is impractical to use in a high-risk environment as users can lose situational awareness [1]. The use of gestures has shown promise in completing tasks more effectively due to their intuitiveness [2] and is more suited to construction sites than other forms of Human-Computer Interaction (HCI) [3]. Our objective was to determine if gesture sets could lead to a safer use of AR on-site compared to other AR user interfaces such as virtual buttons. 

## Implementation
The realistic construction-site scenario and associated functionality were developed in Unity. The Mixed Reality Toolkit (MRTK) was used to create and recognise the gestures based on the curl of their fingers and the position of their hands. The values were determined by testing the gestures with various hand shapes and sizes. The final application was deployed onto the Trimble XR10 with Hololens 2. 

## Prerequisites
(Courtesy of Microsoft Docs. More detailed information can be found in their tutorial: https://learn.microsoft.com/en-us/training/modules/learn-mrtk-tutorials/1-1-introduction?ns-enrollment-type=learningpath&ns-enrollment-id=learn.azure.beginner-hololens-2-tutorials )

- Trimble XR10 with Hololens 2 
- Windows 10 SDK 10.0.18362.0 or later
- Unity Hub with Unity 2020.3.x/2019.4.x installed
- Mixed Reality Feature Tool

## How to Build and Deploy Project to Hololens
(Courtesy of Microsoft Docs. More information can be found in the link below. The link also contains information on how to pair your Hololens for first time users or how to run the emulator if you do not have a Hololens 2.
https://learn.microsoft.com/en-us/training/modules/learn-mrtk-tutorials/1-7-exercise-hand-interaction-with-objectmanipulator?ns-enrollment-type=learningpath&ns-enrollment-id=learn.azure.beginner-hololens-2-tutorials)

- Clone the project and open the project in Unity
- Select `File -> Build Settings` and then click the `Build` button
- Navigate to the `Build Universal Windows Platform` window and then either create a new folder or select an existing folder where you would want to store the build. To start the build, click 'Select Folder'.
- A new Windows Explorer window should appear. Open the solution (.sln) folder in Visual Studio
- Change the configuration of Visual Studio as shown in the image below to deploy via wifi (photos retrieved from Microsoft Docs)
![image](https://user-images.githubusercontent.com/58032488/195483321-826119a6-ebd4-410c-bf1a-9bff1e52cb92.png)
![image](https://user-images.githubusercontent.com/58032488/195483513-a569d523-2105-4b54-a5db-533cad276ac8.png)
- In Visual Studio, go to 'Project -> Properties' and under the `Property Pages` window, go to `Configuration Properties > Debugging`. Under `Debugging to Launch`, select `Remote Machine` if it has not been selected already
- Enter the IP address as shown below (photo retrieved from Microsoft Docs). Make sure the authentication mode is set to `Universal (Unencrypted Protocol)`
![image](https://user-images.githubusercontent.com/58032488/195484075-e02083e1-3bd4-4998-94a8-c4a34ba050b1.png)
- After connecting the Hololens to the computer, select `Build -> Deploy Solution`
- In your Hololens, you can find the deployed project under your applications. Click to project to start it. 


## References
- [1] X. Li, W. Yi, H. Chi, X. Wang and A. Chan, "A critical review of virtual and augmented reality (VR/AR) applications in construction safety", Automation in Construction, vol. 86, pp. 150-162, 2018. Available: 10.1016/j.autcon.2017.11.003.
- [2] M. Mihajlov, E. L.-C. Law, and M. Springett, “Intuitive Learnability of Touch Gestures for Technology-Naïve Older Adults,” vol. 27, no. 3, pp. 344–356, 2014, doi: 10.1093/iwc/iwu044. [Online]. Available: https://doi.org/10.1093/iwc/iwu044.
- [3] A. E. Uva et al., "A User-Centered Framework for Designing Midair Gesture Interfaces," in IEEE Transactions on
Human-Machine Systems, vol. 49, no. 5, pp. 421-429, Oct. 2019, doi: 10.1109/THMS.2019.2919719.

