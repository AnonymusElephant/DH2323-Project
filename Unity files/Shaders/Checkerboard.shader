Shader "CustomRenderTexture/Checkerboard"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1,1,1,1)      // White
        _Color2 ("Color 2", Color) = (0.5,0.5,0.5,1)  // Gray
        _SquareSize ("Square Size", Float) = 0.1     // Set to 1x1
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                // We don't need the mesh's UVs (TEXCOORD0) for this effect
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 worldPos : TEXCOORD0; // Pass world position to the fragment shader
            };

            float _SquareSize;
            fixed4 _Color1;
            fixed4 _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Transform the vertex position to world space and pass its XZ coordinates
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Use the world position passed from the vertex shader
                float2 checkerCoord = floor(i.worldPos / _SquareSize);

                // Sum the integer coordinates and take the modulo 2.
                // This logic now works for both positive and negative coordinates.
                float check = fmod(checkerCoord.x + checkerCoord.y, 2.0);
                
                // When 'check' is negative, fmod can return a negative result.
                // We use abs() to ensure the pattern is consistent.
                if (abs(check) > 0.1)
                {
                    return _Color2; // Gray
                }
                else
                {
                    return _Color1; // White
                }
            }
            ENDCG
        }
    }

		Fallback "VertexLit"
}
