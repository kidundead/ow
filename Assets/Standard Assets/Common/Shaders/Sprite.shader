
Shader "Triniti/Sprite"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "" {}
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }

		BindChannels
		{
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
			Bind "Color", color
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest Always
			AlphaTest Off

			Fog
			{
				Mode Off
			}

			SetTexture [_MainTex]
			{
				combine texture * primary
			}
		}
	}
}

