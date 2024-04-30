Shader "Custom/SceneObjectURP"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderQueue" = "2000" }

        // Define LOD and Lightmode
        LOD 100
        Pass
        {
            HLSLPROGRAM

            // Include URP Core HLSL files
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Define structs for vertex and fragment shaders
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Material properties
            float4 _BaseColor;
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // Vertex shader
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            // Fragment shader
            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _BaseColor;
                return color;
            }

            ENDHLSL
        }
    }
        FallBack "Diffuse"
}
