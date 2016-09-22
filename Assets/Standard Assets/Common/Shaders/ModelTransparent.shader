
Shader "Triniti/ModelTransparent"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "" {}
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			Lighting Off
			SetTexture [_MainTex] {}
		}
	}
}

