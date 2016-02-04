Thank you for purchasing the Ultimate Status Bar UnityPackage!

This package comes with both C# and Javascript coding languages for all needed scripts, so please feel free
to use which ever coding language you are most comfortable with. However, please do not modify the scripts at
all. Please make your own scripts for any character controllers or status functionality. If you need some help
getting started, please feel free to email us at CroweGaming@live.com.

/* ------- < IMPORTANT INFORMATION > ------- */
Within Unity, please go to Window / Ultimate Status Bar to access important information on how to get started
using this package. There are links in that window to an extensive Online Readme, complete Documentation,
and FREE example scripts to get you started as fast as possible!
/* ----- < END IMPORTANT INFORMATION > ----- */

// --- IF YOU CAN'T VIEW THE ONLINE INFORMATION, READ THIS SECTION --- //
Please note that there is a C# AND Javascript version of the UltimateStatusBar, UltimateStatusBarController and the
example scripts. We also included C# and Javascript prefabs located in a folder named Resources, which must remain
in the Resources folder. Also, please note that we are using our own custom inspector, and these Editor scripts are
located in the Ultimate Status Bar / Scripts folder in a folder named "Editor". You must leave these scripts in this
folder in order for the Editor scripts to work correctly.

// --- HOW TO USE THE ULTIMATE STATUS BAR --- //
The Ultimate Status Bar is extremely easy to use. First of all though, you must have the code already created that
takes care of the actual status. For instance, if you are wanting to display your character's health, then you will
have to already have the functionality of taking damage, before the player's health status can be displayed. There
is an example of how to have your player take damage supplied within this package. Please see the ExamplePlayerHealth
script for more information.

So let's assume that you already have the functionality of receiving damage, and you want this to be displayed. Simply
create a Ultimate Status Bar by going to GameObject / UI / Ultimate UI / Ultimate Status Bar. After you have created
the status bar, be sure to assign a name on the Ultimate Status Bar script. This is how the status bar will be referenced.

After your Ultimate Status Bar has been created and customized, then it's time to implement it into your player's health
script. Simply find the Ultimate Status Bar and copy the code provided within the Script Reference section. Then paste it
into your existing code when your player takes damage, and be sure to change the currentValue and maxValue variables to the
current health of the player, and the max health of the player.
/* ------ < EXAMPLE > ------ */
EXAMPLE C#:
	UltimateStatusBar.UpdateStatus( "Health", currentHealth, maxHealth );
EXAMPLE Java:
	UltimateStatusBarJAVA.UpdateStatus( "Health", currentHealth, maxHealth );

After this code has been implemented, SAVE and return to Unity to see your player's health updated visually for the user to
see! If you have found this asset helpful, please leave us a review on the Unity Asset Store. If you are experiencing ANY
problems with Ultimate Status Bar, or any of our assets, please do not hesitate to contact us at CroweGaming@live.com.

Thank you,
-Crowe Gaming