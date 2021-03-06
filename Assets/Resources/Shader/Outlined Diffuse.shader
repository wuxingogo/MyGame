﻿Shader "Custom/Outlined Diffuse"
 {
     Properties
     {
         _Color("Main Color", Color) = (.5,.5,.5,1)
         _OutlineColor("Outline Color", Color) = (0,1,0,1)
         _Outline("Outline width", Range(0.002, 0.03)) = 0.01
         _MainTex("Base (RGB)", 2D) = "white" { }
     }
 
     SubShader
     {
         Tags{ "RenderType" = "Opaque" }
         UsePass "Diffuse/FORWARD"
         Pass
         {
             Name "OUTLINE"
             Tags{ "LightMode" = "Always" }
 
             Cull Front
             ZWrite On
             ColorMask RGB
             Blend SrcAlpha OneMinusSrcAlpha
 
             CGPROGRAM
                 #pragma vertex vert
                 #pragma fragment frag
 
                 uniform float _Outline;
                 uniform float4 _OutlineColor;
     
                 struct appdata {
                     float4 vertex : POSITION;
                     float3 normal : NORMAL;
                 };
 
                 float4 vert(appdata v) : SV_POSITION
                 {
                     float4 pos = mul(UNITY_MATRIX_MVP, v.vertex);
                     float3 norm = mul((float3x3)UNITY_MATRIX_MV, v.normal);
                     norm.x *= UNITY_MATRIX_P[0][0];
                     norm.y *= UNITY_MATRIX_P[1][1];
                     pos.xy += norm.xy * pos.z * _Outline;
                     return pos;
                 }
 
                 float4 frag() : SV_TARGET
                 {
                     return  _OutlineColor;
                 }
             ENDCG
         }
     }
     Fallback "Diffuse"
 }