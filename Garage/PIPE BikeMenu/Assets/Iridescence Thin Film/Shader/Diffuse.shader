Shader "Iridescence Thin Film/Diffuse" {
	Properties {
		_MainTex ("Base", 2D) = "white" {}
		_SpecularColor ("Specular Tint", Color) = (1, 1, 1, 1)
		_SpecPower ("Specular Power", Float) = 3
		_Thinfilm_Strength  ("Film Strength", Float) = 0.5
		_Thinfilm_Color_Freq ("Film Frequency", Float) = 10
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		
		CGPROGRAM
		#include "IridescenceThinFilm.cginc"
		#pragma surface surf ThinFilm
		
		struct Input 
		{
			float2 uv_MainTex;
			float3 worldPos;
		};
		void surf (Input IN, inout SurfaceOutputCustom o) 
		{
			float4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.VertexPos = IN.worldPos;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
