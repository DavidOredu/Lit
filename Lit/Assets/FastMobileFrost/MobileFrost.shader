Shader "SupGames/Mobile/Frost"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
	UNITY_DECLARE_SCREENSPACE_TEXTURE(_FrostTex);
	UNITY_DECLARE_SCREENSPACE_TEXTURE(_BumpTex);
	fixed _Vignette;
	fixed _Transparency;

	struct appdata
	{
		fixed4 pos : POSITION;
		fixed2 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f 
	{
		fixed4 pos : POSITION;
		fixed2 uv : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	v2f vert(appdata i)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_INITIALIZE_OUTPUT(v2f, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.pos = UnityObjectToClipPos(i.pos);
		o.uv.xy = i.uv;
		return o;

	}

	fixed4 frag(v2f i) : COLOR
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		fixed4 f = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_FrostTex, i.uv);
		f.a = saturate(f.a + _Vignette);
		fixed4 n = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_BumpTex, i.uv);
		n.x *= n.w;
		half2 n1 = n.xy * 2 - 1;
		fixed4 c = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, UnityStereoTransformScreenSpaceTex(i.uv) + n1 * f.a);
		return lerp(c, lerp(f, fixed4(c.rgb*(f.rgb + 0.5h)*(f.rgb + 0.5h),1.0h), _Transparency), f.a);
	}
	ENDCG

	Subshader
	{
		Pass
		{
		  ZTest Always Cull Off ZWrite Off
		  Fog{ Mode off }
		  CGPROGRAM
		  #pragma vertex vert
		  #pragma fragment frag
		  #pragma fragmentoption ARB_precision_hint_fastest
		  ENDCG
		}
	}
	Fallback off
}