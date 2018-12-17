﻿Shader "Unlit/MandelbrotShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color Texture", 2D) = "white"{}
		_maxIter("Max Iterations", Float) = 512
		_aspectRatio("AspectRatio", Float) = 1
		_ST("ScaleOffset", Vector) = (2.5, 2.5, -0.5, 0.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _Color;
			float4 _ST;
			float _maxIter;
			float _aspectRatio;

			float permute(float x) {
				return fmod(
					x*x*34.0 + x,
					289.0
				);
			}

			float3 permute(float3 x) {
				return fmod(
					x*x*34.0 + x,
					289.0
				);
			}

			float4 permute(float4 x) {
				return fmod(
					x*x*34.0 + x,
					289.0
				);
			}



			float taylorInvSqrt(float r) {
				return 1.79284291400159 - 0.85373472095314 * r;
			}

			float4 taylorInvSqrt(float4 r) {
				return 1.79284291400159 - 0.85373472095314 * r;
			}



			float4 grad4(float j, float4 ip)
			{
				const float4 ones = float4(1.0, 1.0, 1.0, -1.0);
				float4 p, s;
				p.xyz = floor(frac(j * ip.xyz) * 7.0) * ip.z - 1.0;
				p.w = 1.5 - dot(abs(p.xyz), ones.xyz);

				// GLSL: lessThan(x, y) = x < y
				// HLSL: 1 - step(y, x) = x < y

				//s = float4(
				//    1 - step(0.0, p)
				//);
				//p.xyz = p.xyz + (s.xyz * 2 - 1) * s.www;

				p.xyz -= sign(p.xyz) * (p.w < 0);

				return p;
			}



			// ----------------------------------- 2D -------------------------------------

			float snoise(float2 v)
			{
				const float4 C = float4(
					0.211324865405187, // (3.0-sqrt(3.0))/6.0
					0.366025403784439, // 0.5*(sqrt(3.0)-1.0)
					-0.577350269189626, // -1.0 + 2.0 * C.x
					0.024390243902439  // 1.0 / 41.0
					);

				// First corner
				float2 i = floor(v + dot(v, C.yy));
				float2 x0 = v - i + dot(i, C.xx);

				// Other corners
					// float2 i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
					// Lex-DRL: afaik, step() in GPU is faster than if(), so:
					// step(x, y) = x <= y

					//int xLessEqual = step(x0.x, x0.y); // x <= y ?
					//int2 i1 =
					//    int2(1, 0) * (1 - xLessEqual) // x > y
					//    + int2(0, 1) * xLessEqual // x <= y
					//;
					//float4 x12 = x0.xyxy + C.xxzz;
					//x12.xy -= i1;

				float4 x12 = x0.xyxy + C.xxzz;
				int2 i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
				x12.xy -= i1;

				// Permutations
				i = fmod(i, 289.0); // Avoid truncation effects in permutation
				float3 p = permute(
					permute(
						i.y + float3(0.0, i1.y, 1.0)
					) + i.x + float3(0.0, i1.x, 1.0)
				);

				float3 m = max(
					0.5 - float3(
						dot(x0, x0),
						dot(x12.xy, x12.xy),
						dot(x12.zw, x12.zw)
						),
					0.0
				);
				m = m * m;
				m = m * m;

				// Gradients: 41 points uniformly over a line, mapped onto a diamond.
				// The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)

				float3 x = 2.0 * frac(p * C.www) - 1.0;
				float3 h = abs(x) - 0.5;
				float3 ox = floor(x + 0.5);
				float3 a0 = x - ox;

				// Normalise gradients implicitly by scaling m
				// Approximation of: m *= inversesqrt( a0*a0 + h*h );
				m *= 1.79284291400159 - 0.85373472095314 * (a0*a0 + h * h);

				// Compute final noise value at P
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot(m, g);
			}

			// ----------------------------------- 3D -------------------------------------

			float snoise(float3 v)
			{
				const float2 C = float2(
					0.166666666666666667, // 1/6
					0.333333333333333333  // 1/3
					);
				const float4 D = float4(0.0, 0.5, 1.0, 2.0);

				// First corner
				float3 i = floor(v + dot(v, C.yyy));
				float3 x0 = v - i + dot(i, C.xxx);

				// Other corners
				float3 g = step(x0.yzx, x0.xyz);
				float3 l = 1 - g;
				float3 i1 = min(g.xyz, l.zxy);
				float3 i2 = max(g.xyz, l.zxy);

				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy; // 2.0*C.x = 1/3 = C.y
				float3 x3 = x0 - D.yyy;      // -1.0+3.0*C.x = -0.5 = -D.y

			// Permutations
				i = fmod(i, 289.0);
				float4 p = permute(
					permute(
						permute(
							i.z + float4(0.0, i1.z, i2.z, 1.0)
						) + i.y + float4(0.0, i1.y, i2.y, 1.0)
					) + i.x + float4(0.0, i1.x, i2.x, 1.0)
				);

				// Gradients: 7x7 points over a square, mapped onto an octahedron.
				// The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
				float n_ = 0.142857142857; // 1/7
				float3 ns = n_ * D.wyz - D.xzx;

				float4 j = p - 49.0 * floor(p * ns.z * ns.z); // mod(p,7*7)

				float4 x_ = floor(j * ns.z);
				float4 y_ = floor(j - 7.0 * x_); // mod(j,N)

				float4 x = x_ * ns.x + ns.yyyy;
				float4 y = y_ * ns.x + ns.yyyy;
				float4 h = 1.0 - abs(x) - abs(y);

				float4 b0 = float4(x.xy, y.xy);
				float4 b1 = float4(x.zw, y.zw);

				//float4 s0 = float4(lessThan(b0,0.0))*2.0 - 1.0;
				//float4 s1 = float4(lessThan(b1,0.0))*2.0 - 1.0;
				float4 s0 = floor(b0)*2.0 + 1.0;
				float4 s1 = floor(b1)*2.0 + 1.0;
				float4 sh = -step(h, 0.0);

				float4 a0 = b0.xzyw + s0.xzyw*sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw*sh.zzww;

				float3 p0 = float3(a0.xy, h.x);
				float3 p1 = float3(a0.zw, h.y);
				float3 p2 = float3(a1.xy, h.z);
				float3 p3 = float3(a1.zw, h.w);

				//Normalise gradients
				float4 norm = taylorInvSqrt(float4(
					dot(p0, p0),
					dot(p1, p1),
					dot(p2, p2),
					dot(p3, p3)
					));
				p0 *= norm.x;
				p1 *= norm.y;
				p2 *= norm.z;
				p3 *= norm.w;

				// Mix final noise value
				float4 m = max(
					0.6 - float4(
						dot(x0, x0),
						dot(x1, x1),
						dot(x2, x2),
						dot(x3, x3)
						),
					0.0
				);
				m = m * m;
				return 42.0 * dot(
					m*m,
					float4(
						dot(p0, x0),
						dot(p1, x1),
						dot(p2, x2),
						dot(p3, x3)
						)
				);
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = (v.uv - 0.5) * _ST.xy + _ST.zw;
				o.uv.x *= _aspectRatio;
                return o;
            }

			float4 HSVtoRGB(float3 HSV) {
				float4 RGB = 0;

				float C = HSV.z * HSV.y;
				float H = HSV.x * 6;
				float X = C * (1 - abs(fmod(H, 2) - 1));
				if (HSV.y != 0) {
					float I = floor(H);
					if (I == 0) { RGB = float4(C, X, 0, 1); }
					else if (I == 1) { RGB = float4(X, C, 0, 1); }
					else if (I == 2) { RGB = float4(0, C, X, 1); }
					else if (I == 3) { RGB = float4(0, X, C, 1); }
					else if (I == 4) { RGB = float4(X, 0, C, 1); }
					else { RGB = float4(C, 0, X, 1); }
				}
				float M = HSV.z - C;
				return RGB + M;
			}

			float4 mandelbrot(float cR, float cI) {
				float zR = cR, zI = cI;
				int iteration;
				for (iteration = 0; iteration < _maxIter; iteration++) {
					float tempZR = zR * zR - zI * zI;
					float tempZI = 2.0f * zR * zI;
					zR = tempZR + cR;
					zI = tempZI + cI;
					if (zR * zR + zI * zI > 4) break;
				}

				float4 color = 0;
				if (iteration < _maxIter) {
					float h = (iteration / (float)_maxIter);
					float s = 1;
					float v = 1;
					color = HSVtoRGB(float3(h,s,v));
				}
				return color;
			}

            fixed4 frag (v2f i) : SV_Target
            {
				return mandelbrot(i.uv.x, i.uv.y);
            }
            ENDCG
        }
    }
}
