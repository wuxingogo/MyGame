Shader "Custom/Relfection/Transparent Bumped Specular Flat"
{
	Properties{
		_Offset("Offset", Vector) = (1,1,1,1)
		_Scale("Scale", float) = 1
		_MaskTex("Mask", 2D) = "mask"{}
	}
    SubShader
    {
        // Draw ourselves after all opaque geometry
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        // Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }
        Cull Off
//        Blend SrcAlpha OneMinusSrcAlpha
        // Render the object with the texture generated above, and invert the colors
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD1;
            };


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata v) {
                v2f o;
                // use UnityObjectToClipPos from UnityCG.cginc to calculate 
                // the clip-space of the vertex
                o.pos = UnityObjectToClipPos(v.vertex);
                // use ComputeGrabScreenPos function from UnityCG.cginc
                // to get the correct texture coordinate
                o.grabPos = ComputeGrabScreenPos(o.pos);
                o.uv = v.uv;
                return o;
            }
            sampler2D _MaskTex;
            sampler2D _BackgroundTexture;
            float4 _Offset;
            float _Scale;

            fixed4 frag(v2f i) : SV_Target
            {
            	fixed4 maskCol = tex2D(_MaskTex, i.uv);
                half4 bgcolor = tex2Dproj(_BackgroundTexture , (i.grabPos + float4(maskCol.a,maskCol.a,0,0)));
                return bgcolor;
            }
            ENDCG
        }

    }
}