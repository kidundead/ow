
Shader "Triniti/ModelTransparent3"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "" {}
	}

	SubShader
	{
		Tags { "Queue" = "Transparent+3" }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			Lighting Off
			SetTexture [_MainTex] {}
		}
	}
}

