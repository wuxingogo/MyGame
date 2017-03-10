
Shader "Custom/Effect/X Specular Flat No Texture" {
	Properties{
		_BumpMap("Normalmap", 2D) = "bump" {}
		_MaskMap("MaskMap", 2D) = "mask"{}
		_Decay("Decay", float) = 0.5
	}

	SubShader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Transparent+100" }
			GrabPass{
			Name "BASE"
			Tags{ "LightMode" = "Always" }
	}

	Cull Off
	Lighting Off
	ZWrite On
	Fog{ Mode Off }

	CGPROGRAM
		#pragma surface surf Lambert alpha
		#pragma target 3.0
		#pragma debug

		sampler2D _BumpMap;
		sampler2D _GrabTexture;
		sampler2D _MaskMap;
		float _Decay;


		struct Input {
			float4 screenPos;
			float2 uv_MainTex;
			float2 uv_BumpMap;

		};


		void surf(Input IN, inout SurfaceOutput o) {
			
			fixed3 nor = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));


			float4 screenUV2 = IN.screenPos;


			screenUV2.y = (screenUV2.y - screenUV2.w*_Decay) + screenUV2.w * _Decay;
			screenUV2.xy = screenUV2.xy / screenUV2.w *_Decay;
			screenUV2.xy += nor.xy;

			fixed4 trans = tex2D(_GrabTexture, float2(screenUV2.x, 1 - screenUV2.y));
			fixed4 maskCol = tex2D(_MaskMap, IN.uv_BumpMap);
			o.Emission = trans.rgb;
			o.Normal = nor.rgb;
			
			o.Alpha = maskCol.a;

		}
	ENDCG
	}

	FallBack "Transparent/VertexLit"
}