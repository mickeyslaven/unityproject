﻿/* Written by Kaz Crowe */
/* UltimateJoystickWindow.cs ver 1.0 */
using UnityEngine;
using UnityEditor;

public class UltimateJoystickWindow : EditorWindow
{
	GUILayoutOption[] buttonSize = new GUILayoutOption[] { GUILayout.Width( 150 ), GUILayout.Height( 35 ) };
	Texture2D croweGamingLogo = null;

	[ MenuItem( "Window/Ultimate UI/Ultimate Joystick" ) ]
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		UltimateJoystickWindow ujWindow = ( UltimateJoystickWindow )EditorWindow.GetWindow( typeof( UltimateJoystickWindow ) );
		ujWindow.maxSize = new Vector2( 350, 350 );
		ujWindow.minSize = new Vector2( 350, 350 );
		ujWindow.Show();
	}

	void OnEnable ()
	{
		croweGamingLogo = ( Texture2D )AssetDatabase.LoadAssetAtPath( "Assets/Ultimate Joystick/UltimateJoystick( xCommon )/Editor/CGLogo.png", typeof( Texture2D ) );
	}
	
	void OnGUI ()
	{
		EditorGUILayout.Space();
		if( croweGamingLogo != null )
			GUILayout.BeginVertical( croweGamingLogo, "Box" );
		else
			GUILayout.BeginVertical( "Box" );

		EditorGUILayout.LabelField( " Ultimate Joystick 1.6.1", EditorStyles.whiteMiniLabel );

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( croweGamingLogo != null )
			EditorGUILayout.LabelField( "Ultimate Joystick Help", EditorStyles.whiteLargeLabel, GUILayout.Width( 140 ), GUILayout.Height( 20 ) );
		else
			EditorGUILayout.LabelField( "Ultimate Joystick Help", EditorStyles.largeLabel, GUILayout.Width( 140 ), GUILayout.Height( 20 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Online Readme", EditorStyles.miniButton, buttonSize ) )
			Application.OpenURL( "http://crowegamingassets.weebly.com/uj-online-readme.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Video Tutorials", EditorStyles.miniButton, buttonSize ) )
			Application.OpenURL( "http://crowegamingassets.weebly.com/uj-videos.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Documentation", EditorStyles.miniButton, buttonSize ) )
			Application.OpenURL( "http://crowegamingassets.weebly.com/uj-documentation.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.EndVertical();
	}
}