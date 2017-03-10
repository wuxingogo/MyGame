﻿Shader "TFTM/Outline/Basic-Alpha" {
 Properties {
     _Color ("Main Color", Color) = (.5,.5,.5,1)
     _MainTex ("Base (RGB)", 2D) = "white" {}
 }


SubShader {
     Tags { "RenderType"="Opaque" "Queue"="Transparent" }
     Pass {
         Name "BASE"
         Blend SrcAlpha OneMinusSrcAlpha
         //Blend DstColor SrcColor
         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         #pragma fragmentoption ARB_precision_hint_fastest 
         #include "UnityCG.cginc"
         sampler2D _MainTex;
         float4 _MainTex_ST;
         float4 _Color;
         struct appdata {
             float4 vertex : POSITION;
             float2 texcoord : TEXCOORD0;
             float3 normal : NORMAL;
         };

         struct v2f {
             float4 pos : POSITION;
             float2 texcoord : TEXCOORD0;
         };
         v2f vert (appdata v)
         {
             v2f o;
             o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
             o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
             return o;
         }
         float4 frag (v2f i) : COLOR
         {
             float4 col = _Color * tex2D(_MainTex, i.texcoord);
             return float4(2.0f * col.rgb, col.a);
         }
         ENDCG            
     }
 } 
 SubShader {
     Tags { "RenderType"="Opaque" "Queue"="Transparent"}
     Pass {
         Name "BASE"
         SetTexture [_MainTex] {
             constantColor [_Color]
             Combine texture * constant
         } 
     }
 } 

 Fallback "VertexLit"
 }