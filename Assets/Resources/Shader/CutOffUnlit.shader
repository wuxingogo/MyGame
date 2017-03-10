Shader "Custom/CutOffLightNoEffect" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_MainColor ("Main Color", Color) = (1, 1, 1, 1)
		_Cutoff("Cutoff Value", Range(0,1)) = 0.5  
    }
    
    SubShader {							
         Tags { "RenderType" = "Opaque" }
        Cull off	

		CGPROGRAM
		#pragma surface surf SimpleLambert 

          half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
              half4 c;
              c.rgb = s.Albedo;
              return c;
          }
  
		sampler2D _MainTex;
		float4 _MainColor;
		float _Cutoff;  
		struct Input
		{
		    float2 uv_MainTex;	
		};
		
      	void surf (Input IN, inout SurfaceOutput o) 
      	{
			half4 c = tex2D (_MainTex,IN.uv_MainTex);
			clip (c.a - _Cutoff);
			o.Albedo = c.rgb * _MainColor.rgb;
      	}
      
		ENDCG
		
		
	Pass {  
        Name "Caster"  
        Tags { "LightMode" = "ShadowCaster" }  
        Offset 1, 1  
        Cull Off  
  
        Fog {Mode Off}  
        ZWrite On ZTest LEqual Cull Off  
  
        CGPROGRAM  
        #pragma vertex vert  
        #pragma fragment frag  
        #pragma multi_compile_shadowcaster  
        #pragma fragmentoption ARB_precision_hint_fastest  
        #include "UnityCG.cginc"  
		uniform float4 _MainTex_ST;  
        struct v2f {   
            V2F_SHADOW_CASTER;
            float2  uv : TEXCOORD1;  
        };  
  
        v2f vert( appdata_base v )  
        {  
            v2f o;  
            TRANSFER_SHADOW_CASTER(o)  
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);  
            return o;  
        }           
        uniform sampler2D _MainTex;  
        uniform fixed _Cutoff;  
        uniform fixed4 _Color;  
          
        float4 frag( v2f i ) : COLOR  
        {  
            fixed4 texcol = tex2D( _MainTex, i.uv );  
            clip( texcol.a - _Cutoff );  
              
            SHADOW_CASTER_FRAGMENT(i)  
        }  
        ENDCG  
  
    }  
    }
    FallBack "Diffuse"
}