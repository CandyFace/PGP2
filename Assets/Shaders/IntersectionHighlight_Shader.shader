//Highlights intersections with other objects
 
Shader "Custom/IntersectionHighlight_Shader"
{
    Properties
    {
    	_MainTex ("Foreground Texture", 2D) = "white" {}
    	_SecondTex("Second Texture", 2D) = "white" {}
        _RegularColor("Background color", Color) = (1, 1, 1, .5) //Color when not intersecting
        _HighlightColor("Highlight Color", Color) = (1, 1, 1, .5) //Color when intersecting
        _HighlightThresholdMax("Highlight Threshold Max", Float) = 1 //Max difference for intersections
        _PulFreq("Pulsating frequency", Range(1,40)) = 30
        _MinPulSize("Min Pulsation size", Range(0,1)) = 0.5
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _CutoffTwo ("Alpha Cutoff tex2", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }
 
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
 
            CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members uv_MainTex)
			#pragma exclude_renderers d3d11 xbox360
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _CameraDepthTexture; //Depth Texture
            uniform float4 _RegularColor;
            uniform float4 _HighlightColor;
            uniform float _HighlightThresholdMax;
            half _PulFreq;
            half _MinPulSize;
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 projPos : TEXCOORD1; //Screen position of pos
            };
 
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.projPos = ComputeScreenPos(o.pos);
 
                return o;
            }
 
            half4 frag(v2f i) : COLOR
            {
                float4 finalColor = _RegularColor;
 
                //Get the distance to the camera from the depth buffer for this point
                float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
                float partZ = i.projPos.z;
 
                //If the two are similar, then there is an object intersecting with our object
                float diff = (abs(sceneZ - partZ)) /
                    _HighlightThresholdMax;

 
                if(diff <= 1)
                {
                	half posSin = 0.5 * sin(_Time.x*_PulFreq) + 0.5;
                	half pulseMultiplier = posSin * (1 - _MinPulSize) + _MinPulSize;
                    finalColor = _HighlightColor * pulseMultiplier;
                }
 
                half4 c;
                c.r = finalColor.r;
                c.g = finalColor.g;
                c.b = finalColor.b;
                c.a = finalColor.a;
 
                return c;
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
 
            uniform sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform sampler2D _CameraDepthTexture; //Depth Texture
            uniform float _Cutoff;
 
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
 
                return tex;
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
 
            uniform sampler2D _SecondTex;
            float4 _SecondTex_ST;
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
                o.uv = TRANSFORM_TEX (v.texcoord, _SecondTex);
 
                return o;
            }
 
            half4 frag(v2f i) : COLOR
            {

            	half4 tex = tex2D (_SecondTex, i.uv);

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