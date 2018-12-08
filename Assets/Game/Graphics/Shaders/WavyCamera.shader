Shader "Custom/WavyCamera"
{
	Properties
	{
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _TexScaling ("TexScaling", Vector) = (0,0,0,0)
		_Waving ("Waving", Vector) = (0,0,0,0)
		_WaveResolution ("WaveResolution", Vector) = (1,1,1,1)
	}
	
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
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
			float4 _Waving, _WaveResolution, _TexScaling;

			fixed4 frag (v2f i) : SV_Target
			{
                float2 uv = i.uv;
                uv.x = _TexScaling.x + uv.x * _TexScaling.z + cos(_WaveResolution.x + i.uv.y * _WaveResolution.z + _Waving.x * _Time[1]) * _Waving.z; 
                uv.y = _TexScaling.y + i.uv.y * _TexScaling.w + sin(_WaveResolution.y + i.uv.x * _WaveResolution.w + _Waving.y * _Time[1]) * _Waving.w;
				fixed4 col = tex2D(_MainTex, uv);
				return col;
			}
			ENDCG
		}
	}
}
