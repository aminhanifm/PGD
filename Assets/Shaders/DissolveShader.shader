Shader "Custom/DissolveShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TransitionTex("Transition Texture", 2D) = "white" {}
		_Color("Screen Color", Color) = (1,1,1,1)
		_Cutoff("Cutoff", Range(0, 1)) = 0
		[MaterialToggle] _Distort("Distort", Float) = 0
		_Fade("Fade", Range(0, 1)) = 0
		_X("X", Float) = 0
		_Y("Y", Float) = 0
		_scaleX("X", Float) = 0
		_scaleY("Y", Float) = 0
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			// No culling or depth
			Cull Off ZWrite Off ZTest Always
			Blend One OneMinusSrcAlpha

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
					float2 uv1 : TEXCOORD1;
					float4 vertex : SV_POSITION;
				};

				float4 _MainTex_TexelSize;
				float4 _TransitionTex_TexelSize;
				float _scaleY; float _scaleX;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					o.uv1 = v.uv;

					o.uv1.x *= _scaleX;
					float baruX = (_scaleX - 1) * 0.5;
					o.uv1.x -= baruX;

					o.uv1.y *= _scaleY;
					float baruY = (_scaleY - 1) * 0.5;
					o.uv1.y -= baruY;

					/*float tesscale = 1;
					float tesscale2 = 1;
					o.uv1.x *= tesscale;
					o.uv1.y *= tesscale2;
					o.uv1.x -= (tesscale-1)/2;
					o.uv1.y -= (tesscale2 - 1) / 2;*/

					#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv1.y = 1 - o.uv1.y;
					#endif

					return o;
				}

				sampler2D _TransitionTex;
				int _Distort;
				float _Fade;

				sampler2D _MainTex;
				float _Cutoff;
				fixed4 _Color;
				float _Y; float _X;

				fixed4 simplefrag(v2f i) : SV_Target
				{
					if (i.uv.x < _Cutoff)
						return _Color;

					return tex2D(_MainTex, i.uv);
				}

				fixed4 simplefragopen(v2f i) : SV_Target
				{
					if (0.5 - abs(i.uv.y - 0.5) < abs(_Cutoff) * 0.5)
						return _Color;

					return tex2D(_MainTex, i.uv);
				}

				fixed4 simpleTexture(v2f i) : SV_Target
				{
					fixed4 transit = tex2D(_TransitionTex, i.uv);

					if (transit.b < _Cutoff)
						return _Color;

					return tex2D(_MainTex, i.uv);
				}

				fixed4 frag(v2f i) : SV_Target
				{	
					fixed2 direction = float2(_X, _Y);
					fixed4 col = tex2D(_MainTex, i.uv);
					fixed4 transit = tex2D(_TransitionTex, i.uv1 - direction);


					if (transit.b > _Cutoff) {

						if (i.uv1.x > 1 + direction.x || i.uv1.x < direction.x)  return col;
						else if (i.uv1.y > 1 + direction.y || i.uv1.y < direction.y)  return col;
						else
						{
							col.r *= _Fade;
							col.g *= _Fade;
							col.b *= _Fade;
							col.a = _Fade;
						}
					}

					return col;
				}
				ENDCG
			}
		}
}