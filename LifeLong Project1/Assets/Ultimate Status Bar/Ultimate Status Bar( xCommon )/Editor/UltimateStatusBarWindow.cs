/* Written by Kaz Crowe */
/* UltimateStatusBarWindow.cs ver 1.0 */
using UnityEngine;
using UnityEditor;

public class UltimateStatusBarWindow : EditorWindow
{
	GUILayoutOption[] buttonSize = new GUILayoutOption[] { GUILayout.Width( 150 ), GUILayout.Height( 35 ) };
	Texture2D croweGamingLogo = null;
	
	[ MenuItem( "Window/Ultimate UI/Ultimate Status Bar" ) ]
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		UltimateStatusBarWindow usbWindow = ( UltimateStatusBarWindow )EditorWindow.GetWindow( typeof( UltimateStatusBarWindow ) );
		usbWindow.maxSize = new Vector2( 350, 350 );
		usbWindow.minSize = new Vector2( 350, 350 );
		usbWindow.Show();
	}
	
	void OnEnable ()
	{
		croweGamingLogo = ( Texture2D )AssetDatabase.LoadAssetAtPath( "Assets/Ultimate Status Bar/Ultimate Status Bar( xCommon )/Editor/CGLogo.png", typeof( Texture2D ) );
	}
	
	void OnGUI ()
	{
		EditorGUILayout.Space();
		if( croweGamingLogo != null )
			GUILayout.BeginVertical( croweGamingLogo, "Box" );
		else
			GUILayout.BeginVertical( "Box" );
		
		GUILayout.FlexibleSpace();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( croweGamingLogo != null )
			EditorGUILayout.LabelField( "Ultimate Status Bar Help", EditorStyles.whiteLargeLabel, GUILayout.Width( 160 ), GUILayout.Height( 20 ) );
		else
			EditorGUILayout.LabelField( "Ultimate Status Bar Help", EditorStyles.largeLabel, GUILayout.Width( 160 ), GUILayout.Height( 20 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Online Readme", EditorStyles.miniButton, buttonSize ) )
			Application.OpenURL( "http://crowegamingassets.weebly.com/usb-online-readme.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Video Tutorials", EditorStyles.miniButton, buttonSize ) )
			Application.OpenURL( "http://crowegamingassets.weebly.com/usb-videos.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Documentation", EditorStyles.miniButton, buttonSize ) )
			Application.OpenURL( "http://crowegamingassets.weebly.com/usb-documentation.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.EndVertical();
	}
}