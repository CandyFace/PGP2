//Highlights intersections with other objects
 
Shader "Custom/2LayerAlphaCut"
{
    Properties
    {
    	_Color ("Color", Color) = (1,1,1,1)
    	_MainTex ("Foreground texture", 2D) = "white" {}
    	_Tex2 ("Background texture", 2D) = "white" {}
        _PulFreq("Pulsating frequency", Range(1,40)) = 30
        _MinPulSize("Min Pulsation size", Range(0,1)) = 0.5
        _Cutoff("Alpha cutoff A", Range(0,1)) = 0.5
        _CutoffTwo("Alpha cutoff B", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }

         Pass {	
         Cull Back // now render the front faces
         ZWrite Off // don't write to depth buffer 
            // in order not to occlude other objects
         Blend SrcAlpha OneMinusSrcAlpha 
            // blend based on the fragment's alpha value
         
         CGPROGRAM
 
         	#pragma exclude_renderers d3d11 xbox360
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" 
 
            uniform sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform sampler2D _CameraDepthTexture; //Depth Texture
            uniform float _Cutoff;
            uniform float4 _Color;

 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 projPos : TEXCOORD1; //Screen position of pos
                float2 uv : TEXCOORD0;
            };
 
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.projPos = ComputeScreenPos(o.pos);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
 
                return o;
            }
 
            half4 frag(v2f i) : COLOR
            {

            half4 tex = tex2D (_MainTex, i.uv);

            if (tex.a < _Cutoff)
               // alpha value less than user-specified threshold?
            {
               discard; // yes: discard this fragment
            }

            float4 finalColor = _Color;

                half4 c;
                c.r = finalColor.r;
                c.g = finalColor.g;
                c.b = finalColor.b;
                c.a = finalColor.a;
 
                return tex * c;
            }
 
            ENDCG
        }

               Pass {	
         Cull Back // now render the front faces
         ZWrite Off // don't write to depth buffer 
            // in order not to occlude other objects
         Blend SrcAlpha OneMinusSrcAlpha 
            // blend based on the fragment's alpha value
         
         CGPROGRAM
 
         	#pragma exclude_renderers d3d11 xbox360
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" 
 
            uniform sampler2D _Tex2;
            float4 _Tex2_ST;
            uniform sampler2D _CameraDepthTexture; //Depth Texture
            uniform float _CutoffTwo;
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 projPos : TEXCOORD1; //Screen position of pos
                float2 uv : TEXCOORD0;
            };
 
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.projPos = ComputeScreenPos(o.pos);
                o.uv = TRANSFORM_TEX (v.texcoord, _Tex2);
 
                return o;
            }
 
            half4 frag(v2f i) : COLOR
            {

            	half4 tex = tex2D (_Tex2, i.uv);

           	    if (tex.a < _CutoffTwo)
           	       // alpha value less than user-specified threshold?
           	    {
           	       discard; // yes: discard this fragment
           	    }
 		  	   
           	        return tex;
           	    }
 
            ENDCG
        }
      }
    FallBack "VertexLit"
}