Shader "Custom/HiddenText" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		//Lighting Off ZWrite Off ZTest Off
		
		CGPROGRAM
		#pragma surface surf SimpleLambert alpha
		#pragma debug

	    
		half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
            half NdotL = dot (s.Normal, lightDir);
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
            c.a = s.Alpha *_LightColor0.b;
            return c;
        }
      
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			//o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
