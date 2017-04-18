Shader "Custom/Shader2" {
	Properties
	{
		_Color ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{

CGPROGRAM
#pragma fragmentoption ARB_precision_hint_fastest
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

// uniforms
uniform fixed4 _Color;

		struct vertexInput
		{
			float4 vertex : POSITION; //position in object coordinates
			float4 tangent : TANGENT; 
			float3 normal : NORMAL; // surface normal vector
			float4 texcoord : TEXCOORD0; // 0th set of texture coordinates
			float4 texcoord1 : TEXCOORD1; // 1st set of texture coordinates
			fixed4 color : COLOR; //vertex color
		};

		struct fragmentInput
		{
			float4 pos : SV_POSITION;
			float4 color : COLOR0;
		};

		fragmentInput vert(vertexInput i)
		{
			fragmentInput o;
			o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
			o.color = _Color;

			o.color = i.texcoord;
			//o.color = i.texcoord1;
			//o.color = i.vertex;
			//o.color = i.vertex + float4(0.5, 0.5, 0.5, 0.0);
			//o.color = i.tangent;
			//o.color = float4(i.normal * 0.5 + 0.5, 1.0);
			//o.color = i.color;

			return o;
		}


		half4 frag(fragmentInput i) : COLOR
		{
			return i.color;
		}
		ENDCG
			}
	}
		FallBack "Diffuse"
}
