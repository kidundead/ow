
Shader "Triniti/techTextureAlphaed"
{
	Properties
	{
		_texBase ("Base", 2D) = "" {}		
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Lighting OFF
			
			Cull OFF
			
			ZTest Always
			ZWrite Off						

			SetTexture [_texBase] {}
		}		
	}		
}

