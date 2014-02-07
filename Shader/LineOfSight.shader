Shader "Hidden/Light Map" {
Properties {
	_MainTex ("View", 2D) = "View" {}
	_LightTex ("Light Map", 2D) = "Light Map" {}
}

SubShader {
	Tags {"Queue" = "Geometry"}
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _LightTex;

fixed4 frag (v2f_img i) : COLOR
{
	fixed4 original = tex2D(_MainTex, i.uv);
	fixed4 output = tex2D(_LightTex, i.uv);
	original.rgb = original.rgb *output.g;
	original.a = 1 -(original.a *output.b);
	return original;
}
ENDCG

	}
}

Fallback off

}