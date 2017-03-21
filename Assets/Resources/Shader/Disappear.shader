Shader "Unlit/disappear"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_TimeScale ("TimeScale", float) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType" = "Transparent"  }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100
//		ColorMask 0
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _TimeScale;
			
			v2f vert (appdata v)
			{
				v2f o;
//				float3 look = normalize(ObjSpaceViewDir(v.vertex).xyz);
//			float3 up = float3(0.0f, 1.0f, 0.0f);
//			float3 right = normalize(cross(look, up));
//			float4x4 rotationMatrix =
//			float4x4(float4(right.x, up.x, -look.x, 0), float4(right.y, up.y, -look.y, 0), float4(right.z, up.z, -look.z, 0),  float4(v.vertex.xyz, 1));
//			float4 finalposition = mul(rotationMatrix, v.vertex);              
//			float4 pos = float4(finalposition.xyz, 1);
//			o.vertex = mul(UNITY_MATRIX_MVP, pos);


				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				fixed4 col = tex2D(_MainTex,  i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col * _Color;
			}
			ENDCG
		}
	}
}
