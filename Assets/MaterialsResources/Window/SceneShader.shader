Shader "Custom/SceneShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Center("Center", Vector) = (0.5, 0.5, 0, 0)
        _Size("Size", Float) = 0.1
    }
        SubShader
        {
            Tags { "IgnoreProjector" = "True" "RenderType" = "Opaque" "RenderQueue" = "2000"}
            LOD 200

            Blend SrcAlpha OneMinusSrcAlpha
            AlphaTest Greater 0.5

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

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Center; // Center of the hole (x, y)
                float _Size; // Size of the hole

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    float2 holeCenter = _Center.xy;
                    float halfSize = _Size * 0.5;

                    // Check if current fragment is within the square defined by center and size
                    if (abs(i.uv.x - holeCenter.x) < halfSize && abs(i.uv.y - holeCenter.y) < halfSize)
                    {
                        discard; // Discard the fragment to create a hole
                    }

                    return col;
                }
                ENDCG
            }
        }
}
