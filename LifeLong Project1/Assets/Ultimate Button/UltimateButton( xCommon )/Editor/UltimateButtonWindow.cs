/* Written by Kaz Crowe */
/* UltimateButtonWindow.cs ver 1.0 */
using UnityEngine;
using UnityEditor;

public class UltimateButtonWindow : EditorWindow
{
	GUILayoutOption[] buttonSize = new GUILayoutOption[] { GUILayout.Width( 150 ), GUILayout.Height( 35 ) };
	Texture2D croweGamingLogo = null;
	
	[ MenuItem( "Window/Ultimate Button" ) ]
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		UltimateButtonWindow ujWindow = ( UltimateButtonWindow )EditorWindow.GetWindow( typeof( UltimateButtonWindow ) );
		ujWindow.maxSize = new Vector2( 350, 350 );
		ujWindow.minSize = new Vector2( 350, 350 );
		ujWindow.Show();
	}
	
	void OnEnable ()
	{
		croweGamingLogo = ( Texture2D )AssetDatabase.LoadAssetAtPath( "Assets/Ultimate Button/UltimateButton( xCommon )/Editor/CGLogo.png", typeof( Texture2D ) );
	}
	
	void OnGUI ()
	{
		EditorGUILayout.Space();
		if( croweGamingLogo != null )
			GUILayout.BeginVertical( croweGamingLogo, "Box" );
		else
			GUILayout.BeginVertical( "Box" );

		EditorGUILayout.LabelField( " Version 1.5.1", EditorStyles.whiteMiniLabel );

		GUILayout.FlexibleSpace();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( croweGamingLogo != null )
			EditorGUILayout.LabelField( "Ultimate Button Help", EditorStyles.whiteLargeLabel, GUILayout.Width( 140 ), GUILayout.Height( 20 ) );
		else
			EditorGUILayout.LabelField( "Ultimate Button Help", EditorStyles.largeLabel, GUILayout.Width( 140 ), GUILayout.Height( 20 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Online Readme", EditorStyles.miniButton, buttonSize ) )
			Application.OpenURL( "http://crowegamingassets.weebly.com/ub-online-readme.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Free Example Scripts", EditorStyles.miniButton, buttonSize ) )
			Application.OpenURL( "http://crowegamingassets.weebly.com/ub-example-scripts.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Documentation", EditorStyles.miniButton, buttonSize ) )
			Application.OpenURL( "http://crowegamingassets.weebly.com/ub-documentation.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.EndVertical();
	}
}