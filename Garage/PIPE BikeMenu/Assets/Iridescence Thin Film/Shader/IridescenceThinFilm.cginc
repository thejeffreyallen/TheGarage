#ifndef Iridescence_ThinFilm_CGINC
#define Iridescence_ThinFilm_CGINC

float _Thinfilm_Color_Freq;
float _Thinfilm_Strength;

float ITF_range (float v, float oMin, float oMax, float iMin, float iMax)
{
	return iMin + ((v - oMin)/(oMax - oMin)) * (iMax - iMin);
}
float ITF_noise (float3 pos)
{
	float mult = 1;
	float oset = 45;
	return	sin(pos.x * mult * 2 + 12 + oset) + cos(pos.z * mult + 21 + oset) *
			sin(pos.y * mult * 2 + 23 + oset) + cos(pos.y * mult + 32 + oset) *
			sin(pos.z * mult * 2 + 34 + oset) + cos(pos.x * mult + 43 + oset);
}
float3 ITF_color (float orient, float3 P)
{
	float freq = _Thinfilm_Color_Freq;
	float oset = 25;
	float noiseMult = 1;
	
	float3 c;
	c.r = abs(cos(orient * freq + ITF_noise(P) * noiseMult + 1 + oset));
	c.g = abs(cos(orient * freq + ITF_noise(P) * noiseMult + 2 + oset));
	c.b = abs(cos(orient * freq + ITF_noise(P) * noiseMult + 3 + oset));
	return c;
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
sampler2D _MainTex, _BumpTex;
float4 _SpecularColor;
float _SpecPower;

struct SurfaceOutputCustom
{
	fixed3 Albedo;
	fixed3 Normal;
	fixed3 Emission;
	half Specular;
	fixed Gloss;
	fixed Alpha;
	float3 VertexPos;
};
inline fixed4 LightingThinFilm (SurfaceOutputCustom s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	float3 V = normalize(viewDir);
	float3 N = normalize(s.Normal);
	float3 L = normalize(lightDir);
	float vdn = dot(V, N);
			
	fixed3 iridColor = ITF_color(vdn, s.VertexPos) * ITF_range(pow(1.0 - vdn, 1.0 / 0.75), 0.0, 1.0, 0.1, 1.0);
	iridColor *= _Thinfilm_Strength;
			
	float diff = max(0, dot(N, L));
	fixed3 diffColor = s.Albedo * _LightColor0.rgb * diff;
			
	float3 H = normalize(L + V);
	float ndh = max(0, dot(N, H));
	float spec = pow(ndh, _SpecPower);
	fixed3 specColor = (_LightColor0.rgb * _SpecularColor.rgb * spec * iridColor.rgb) * (atten * 2);
			
	fixed4 c;
	c.rgb = diffColor + specColor;
	c.a = s.Alpha;
	return c;
}

#endif