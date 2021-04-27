Shader "Unlit/Stream"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Cutoff("Alpha Cutoff", Range(0.0,1.0)) = 0.5
	}
	SubShader
	{
		Tags { "Queue"="Geometry"}
		//LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha 
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 frag (v2f i) : SV_Target
			{
				// the 8 is for the speed of the stream
				float2 distuv = float2(i.uv.x, i.uv.y + _Time.x * 40);
				


				float4 color = tex2D(_MainTex, distuv);
				

				//color.a = step(0.0, i.uv.y -_Cutoff);
				
			
				return color;
			}
			ENDCG
		}
	}
}
