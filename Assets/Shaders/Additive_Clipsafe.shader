Shader "Particles/Additive Clipsafe" {
	Properties {
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_FallOffTex ("Falloff texture",2D) = "white" {}
		_FadeDistance ("Fade Start Distance", float) = 0.5
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	}

	SubShader {
		Tags { "Queue" = "Transparent" }
		Blend SrcAlpha One
		AlphaTest Greater .01
		ColorMask RGB
		Lighting Off
		ZWrite Off
		Cull Off
		Fog { Color (0,0,0,0) }
		Pass {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_builtin
				#pragma fragmentoption ARB_fog_exp2
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
				
				uniform float4 _MainTex_ST;
				uniform float4 _FallOffTex_ST;
				uniform float4 _TintColor;
				uniform float  _FadeDistance;
				
				struct appdata_vert {
					float4 vertex : POSITION;
					float4 texcoord : TEXCOORD0;
					float4 texcoord2 :TEXCOORD1;
					float4 color : COLOR;
				};
				
				uniform sampler2D _MainTex;
				uniform sampler2D _FallOffTex;
				
				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 falloffuv : TEXCOORD1; 
					float4 color : COLOR;
					float4 projPos : TEXCOORD2;
				};
				
				v2f vert (appdata_vert v) {
					v2f o;
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.falloffuv = TRANSFORM_TEX(v.texcoord2, _FallOffTex);

					o.projPos = ComputeScreenPos (o.pos);
					COMPUTE_EYEDEPTH(o.projPos.z);

					float4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
					float alpha = (-viewPos.z - _ProjectionParams.y+o.falloffuv)/_FadeDistance;
					alpha = min(alpha, 1);
					o.color = float4(v.color.rgb, v.color.a*alpha);
					o.color *= _TintColor*2;
					return o;
				}

				sampler2D_float _CameraDepthTexture;
				float _InvFade;
				
				float4 frag (v2f i) : COLOR {

					//Soft particles
					float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
					float partZ = i.projPos.z;
					float fade = saturate (_InvFade * (sceneZ-partZ));
					i.color.a *= fade;

					half4 texcol = tex2D( _MainTex, i.uv );
					half4 falluv = tex2D(_FallOffTex, i.falloffuv);
					
					return texcol*i.color;
				}
			ENDCG
		}
	}
	
	Fallback "Particles/Additive"
}