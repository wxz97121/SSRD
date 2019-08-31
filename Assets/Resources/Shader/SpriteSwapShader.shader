Shader "Custom/SpriteSwapShader"
{
		Properties
		{
			[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
			_Color("Tint", Color) = (1,1,1,1)
			[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
			_OldColor("OldColor",Color) = (1,1,1,1)
			_NewColor("NewColor",Color) = (1,1,1,1)
			_DistToSwap("Dist",float) = -0.1

			_OldColor2("OldColor",Color) = (1,1,1,1)
			_NewColor2("NewColor",Color) = (1,1,1,1)
			_DistToSwap2("Dist",float) = -0.1


			_OldColor3("OldColor",Color) = (1,1,1,1)
			_NewColor3("NewColor",Color) = (1,1,1,1)
			_DistToSwap3("Dist",float) = -0.1
		}

			SubShader
			{
				Tags
				{
					"Queue" = "Transparent"
					"IgnoreProjector" = "True"
					"RenderType" = "Transparent"
					"PreviewType" = "Plane"
					"CanUseSpriteAtlas" = "True"
				}

				Cull Off
				Lighting Off
				ZWrite Off
				Blend One OneMinusSrcAlpha

				Pass
				{
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma multi_compile _ PIXELSNAP_ON
					#include "UnityCG.cginc"

					struct appdata_t
					{
						float4 vertex   : POSITION;
						float4 color    : COLOR;
						float2 texcoord : TEXCOORD0;
					};

					struct v2f
					{
						float4 vertex   : SV_POSITION;
						fixed4 color : COLOR;
						float2 texcoord  : TEXCOORD0;
					};

					fixed4 _Color;
					fixed4 _OldColor;
					fixed4 _NewColor;
					float _DistToSwap;

					fixed4 _OldColor2;
					fixed4 _NewColor2;
					float _DistToSwap2;

					fixed4 _OldColor3;
					fixed4 _NewColor3;
					float _DistToSwap3;
					

					v2f vert(appdata_t IN)
					{
						v2f OUT;
						OUT.vertex = UnityObjectToClipPos(IN.vertex);
						OUT.texcoord = IN.texcoord;
						OUT.color = IN.color * _Color;
						#ifdef PIXELSNAP_ON
						OUT.vertex = UnityPixelSnap(OUT.vertex);
						#endif

						return OUT;
					}

					sampler2D _MainTex;
					sampler2D _AlphaTex;
					float _AlphaSplitEnabled;

					fixed4 SampleSpriteTexture(float2 uv)
					{
						fixed4 color = tex2D(_MainTex, uv);

		#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
						if (_AlphaSplitEnabled)
							color.a = tex2D(_AlphaTex, uv).r;
		#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

						return color;
					}

					fixed4 frag(v2f IN) : SV_Target
					{
						fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
						if (length(c.rgb - _OldColor.rgb) < _DistToSwap) c = saturate(c + (_NewColor - _OldColor));
						else if (length(c.rgb - _OldColor2.rgb) < _DistToSwap2) c = saturate(c + (_NewColor2 - _OldColor2));
						else if (length(c.rgb - _OldColor3.rgb) < _DistToSwap3) c = saturate(c + (_NewColor3 - _OldColor3));

						c.rgb *= c.a;
						
						return c;
					}
				ENDCG
				}
			}
	}