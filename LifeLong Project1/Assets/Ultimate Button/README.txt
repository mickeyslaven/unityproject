Thank you for purchasing the Ultimate Button UnityPackage!

This package comes with both C# and Javascript coding languages for all needed scripts,
so please feel free to use which ever coding language you are most comfortable with.

/* -----> IMPORTANT INFORMATION <----- */
Within Unity, please go to Window / Ultimate Button to access important information on how to get started
using the Ultimate Button. There are links in that window to an Extensive Online Readme, Complete Documentation,
and FREE example scripts to get you started as fast as possible!
/* ---> END IMPORTANT INFORMATION <--- */


/* IF YOU CAN'T VIEW THE ONLINE INFORMATION, READ THIS SECTION */
To create a Ultimate Button, go to GameObject > UI > Ultimate Button. This creates a copy of the Ultimate Button
prefab that is located in the folder named Resources. This folder must be named Resources in order for you to create
an Ultimate Button from the menu. Also, please note that we are using our own custom inspector, and the Editor script
is located in the Ultimate Button folder in a folder named "Editor". You must leave these scripts in this folder in
order for the Editor scripts to work correctly. The Ultimate Button is written in both Javascript and C#, and both
versions work the exact same. Please see the next section for information on how to integrate the Ultimate Button into
your project. Additionally, you can view Ultimate Button Video Tutorials and get free example scripts to help you get
started on our Support Website: http://crowegamingassets.weebly.com/

/* ---- HOW TO REFERENCE THE ULTIMATE BUTTON ---- */
The Ultimate Button is extremely easy to integrate into your project. There are 2 sseperate ways that you can do this.
The Script Reference section of the Ultimate Button in the Inspector, you will see the 2 options. The first Reference
Option is the Unity Events, and the second is Get Button States. So let's look at both options and see which one will
be easiest for you.

Unity Events
------------
To use the Unity Events - Reference Option, all you need to do is make the Function that you are trying to reference Public.
This will allow the Ultimate Button to see those functions and call them correctly. Next you will want to make a Event for
the Button by creating a On Button Down () or On Button Up () event. These are located in the Button Events section of the
inspector.

Get Button States
-----------------
The Get Button States - Reference Option is very much like Input system used in Unity. First, you will want to create an
Ultimate Button within your scene and assign a name within the Script Reference section. If you are using the Ultimate Button
to replace Standalone code, then you will want to find anything in your code with the Input.GetButton, Input.GetButtonDown, or
Input.GetButtonUp functions. Then replace them with UltimateButton.GetButton, UltimateButton.GetButtonDown, or
UltimateButton.GetButtonUp. These functions are available to copy for your code once you assign a name to your Ultimate Button
within your scene. If you have the Ultimate Button for use in your scripts from scratch, then all you need to do is create an
Ultimate Button within your scene and assign it a name so that you can use the Ultimate Button state functions.

/* - IMPORTANT NOTE - */
If you have any issues getting this product implemented into your project, please contact us at CroweGaming@live.com and we will
try to help you out as much as we can!