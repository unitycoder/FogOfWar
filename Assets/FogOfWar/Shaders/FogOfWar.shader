Shader "UnityCoder/Fog Of War"
{
	Properties
	{
//		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		//Tags { "RenderType"="Opaque" }
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		//Cull Off
		Lighting Off
		ZWrite Off
		//Fog { Color(0,0,0,0) }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				//float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				//float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

//			sampler2D _MainTex;
//			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				return i.color;
			}
			ENDCG
		}
	}
}
