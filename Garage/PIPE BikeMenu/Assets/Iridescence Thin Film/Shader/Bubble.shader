Shader "Iridescence Thin Film/Bubble" {
	Properties {
		[Header(Iridescence)]
		[HideInInspector] _MainTex ("Main", 2D) = "white" {}
		_NoiseTex     ("Noise", 2D) = "white" {}
		_ColorRampTex ("ColorRamp",2D) = "white"{}
		_RimPower     ("RimPower", Range(0, 5)) = 0
		_Blend        ("Blend",Range(0,1)) = 0.5
		_Distortion   ("Distortion",Float) = 6
//		_CubeTex   ("Env", CUBE) = "" {}
		[Space(20)][Header(Animation)]
		_Speed     ("Speed", Float) = 1
		_Direction ("Direction", Vector) = (0, 0.2, 0, 0)
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Unlit vertex:vert alpha:fade

		sampler2D _NoiseTex, _ColorRampTex, _MainTex;
//		samplerCUBE _CubeTex;
		float4 _Direction, _ColorRampTex_ST;
		float _Speed, _Distortion, _RimPower, _Blend;
		
		struct Input
		{
			float2 uv_MainTex;
			float3 worldRefl;
			float3 viewDir;
		};
		void vert (inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o)
			float4 t = tex2Dlod(_NoiseTex, float4(v.texcoord.xy * 2.0 + (_Time.x * _Speed), 0.0, 0.0));
			v.vertex.xyz += t.y * _Direction.xyz;
		}
		void surf (Input IN, inout SurfaceOutput o) 
		{
			float nis = tex2D(_NoiseTex, IN.uv_MainTex).r;
			float4 rim = dot(normalize(IN.viewDir), o.Normal);
			float4 c = tex2D(_ColorRampTex, TRANSFORM_TEX(rim, _ColorRampTex) * nis * _Distortion + _Time.x * 3.0);
			float rim2 = saturate(1.0 - pow(rim, _RimPower));
			
//			float3 env = texCUBE(_CubeTex, IN.worldRefl).rgb;
			float4 envHdr = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, IN.worldRefl, 0.1);
			float3 envLdr = DecodeHDR(envHdr, unity_SpecCube0_HDR).rgb;

			o.Albedo = 0.0;
			o.Alpha = rim2;
			o.Emission = envLdr + lerp(0.0, c.rgb, _Blend) * rim2;
		}
		inline fixed4 LightingUnlit (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			return fixed4(s.Albedo + s.Emission, s.Alpha);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
