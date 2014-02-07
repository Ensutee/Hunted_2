Shader "Custom/Transparent Shadowcaster" {

Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
}

Subshader {
	UsePass "VertexLit/SHADOWCASTER"
	
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	LOD 200
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off Lighting Off ZWrite Off
	CGPROGRAM
	#pragma surface surf Lambert addshadow
	
	fixed4 _Color;
	
	struct Input {
		float2 uv_MainTex;
	};
	
	void surf (Input IN, inout SurfaceOutput o) {
		o.Alpha = _Color.a;
	}
	ENDCG
}

Fallback off
}