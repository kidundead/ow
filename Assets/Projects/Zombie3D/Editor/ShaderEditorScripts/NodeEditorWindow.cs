using UnityEditor;
using UnityEngine;
using StrumpyShaderEditor;

public class NodeEditorWindow : EditorWindow {
	private static NodeEditor _editor = null;
	
	[MenuItem("Shader Editor/Strumpy Shader Editor")]
	public static void Init ()
	{
		GetWindow (typeof(NodeEditorWindow));
	}
	
	[MenuItem("Shader Editor/Donate")]
	public static void Donate ()
	{
		Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=59CS4BHGRLWDS&lc=AU&item_name=http%3a%2f%2fwww%2estrumpy%2enet&item_number=Strumpy%20Shader%20Editor&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHostedGuest");
	}
	
	public void Update()
	{
		
		if( _editor == null )
		{
			Initialize();
		}
		_editor.Update( this );
	}
	
	public void OnGUI()
	{
		if( _editor == null )
		{
			Initialize();
		}
		_editor.OnGUI( this );
	}
	
	public void OnDisable()
	{
		if( _editor != null )
		{
			_editor.CacheLastGraph();
			_editor = null;
		}
	}

	private void Initialize()
	{
		_editor = NodeEditor.Instance;
		wantsMouseMove = true;
		_editor.LoadLastGraph();
		GUIUtility.hotControl = 0;
	}
	
	public void OnEnable() 
	{
		if( _editor == null )
		{
			Initialize();
		}
	}
}
