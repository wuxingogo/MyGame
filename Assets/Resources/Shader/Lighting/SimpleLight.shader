Shader "Unlit/SimpleLight"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" // for _LightColor0
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;

				float4 vertex : SV_POSITION;
				fixed4 diff : COLOR0; // diffuse lighting color
				fixed4 color : COLOR1;
				float3 normal : NORMAL;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
//				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = v.texcoord;

				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                // factor in the light color
                o.diff = _LightColor0;
                o.color = _LightColor0;
                o.normal = v.normal;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
//				col.xyz *= i.diff
//				half NdotL = dot (i.vertex, lightDir);
//				col.xyz *= _LightColor0.rgb;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				half3 worldNormal = UnityObjectToWorldNormal(i.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				return fixed4(nl, nl, nl, 1);
			}
			ENDCG
		}
	}
}
