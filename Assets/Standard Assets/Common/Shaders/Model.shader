
Shader "Triniti/Model"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "" {}
	}

	SubShader
	{
		Pass
		{
			Cull Off
			Lighting Off
			SetTexture [_MainTex] {}
		}
	}
}

