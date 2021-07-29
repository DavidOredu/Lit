Shader "SupGames/Mobile/FrostURP"
{
	Properties
	{
		[HideInInspector]_MainTex("Base (RGB)", 2D) = "white" {}
	}

	HLSLINCLUDE

	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

	TEXTURE2D_X(_MainTex);
	SAMPLER(sampler_MainTex);
	TEXTURE2D_X(_FrostTex);
	SAMPLER(sampler_FrostTex);
	TEXTURE2D_X(_BumpTex);
	SAMPLER(sampler_BumpTex);

	half _Vignette;
	half _Transparency;

	struct AttributesDefault
	{
		half4 pos : POSITION;
		half2 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f 
	{
		half4 pos : POSITION;
		half2 uv  : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	v2f vert(AttributesDefault i)
	{
		v2f o = (v2f)0;
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.pos = TransformWorldToHClip(TransformObjectToWorld(i.pos.xyz));
		o.uv = i.uv;
		return o;
	}

	half4 frag(v2f i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		half4 f = SAMPLE_TEXTURE2D(_FrostTex, sampler_FrostTex, i.uv);
		f.a = saturate(f.a + _Vignette);
		half4 n = SAMPLE_TEXTURE2D(_BumpTex, sampler_BumpTex, i.uv);
		n.x *= n.w;
		half2 n1 = n.xy * 2 - 1;
		half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv) + n1 * f.a);
		return lerp(c, lerp(f, half4(c.rgb*(f.rgb + 0.5h)*(f.rgb + 0.5h),1.0h), _Transparency), f.a);
	}
	ENDHLSL

	Subshader
	{
		Pass //0
		{
		  ZTest Always Cull Off ZWrite Off
		  Fog { Mode off }
		  HLSLPROGRAM
		  #pragma vertex vert
		  #pragma fragment frag
		  #pragma fragmentoption ARB_precision_hint_fastest
		  ENDHLSL
		}
	}
}