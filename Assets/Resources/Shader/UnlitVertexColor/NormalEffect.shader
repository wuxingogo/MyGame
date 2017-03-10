Shader "Custom/Vertex/NormalEffect"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Normal("Normal", 2D) = "white"{}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma target 3.0
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
	            float4 color : COLOR;
			};

			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
	            o.pos = UnityObjectToClipPos(v.vertex );
	            // calculate bitangent
	            float3 bitangent = cross( v.normal, v.tangent.xyz ) * v.tangent.w;
	            o.color.xyz = bitangent * 0.5 + 0.5;
	            o.color.w = 1.0;
	            return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}
	}
}
