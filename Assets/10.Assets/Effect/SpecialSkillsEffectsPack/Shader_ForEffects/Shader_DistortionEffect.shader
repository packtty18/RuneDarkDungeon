Shader "GAPH Custom Shader/Distortion Effect URP" {
    Properties {
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _Mask ("Mask",2D) = "black"{}
        _NormalMap ("Normalmap", 2D) = "bump" {}
        _DistortFactor ("Distortion", Float) = 10
        _InvFade ("Soft Particles Factor", Range(0,10)) = 1.0
    }
    SubShader{
        Tags{ 
            "Queue" = "Transparent"  
            "IgnoreProjector" = "True"  
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off
        
        Pass{
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_particles
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            
            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord: TEXCOORD0;
                half4 color : COLOR;
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float2 uvnormal : TEXCOORD1;
                float2 uvmask : TEXCOORD2;
                half4 color : COLOR;
            };
            
            CBUFFER_START(UnityPerMaterial)
                half4 _TintColor;
                float _DistortFactor;
                float _InvFade;
                float4 _NormalMap_ST;
                float4 _Mask_ST;
            CBUFFER_END
            
            TEXTURE2D(_Mask);
            SAMPLER(sampler_Mask);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.color = v.color;
                o.uvnormal = TRANSFORM_TEX(v.texcoord, _NormalMap);
                o.uvmask = TRANSFORM_TEX(v.texcoord, _Mask);
                return o;
            }
            
            half4 frag(v2f i) : SV_Target
            {
                // Soft Particles
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                
                #ifdef SOFTPARTICLES_ON
                    float sceneDepth = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);
                    float particleDepth = LinearEyeDepth(i.screenPos.z / i.screenPos.w, _ZBufferParams);
                    float fade = saturate(_InvFade * (sceneDepth - particleDepth));
                    i.color.a *= fade;
                #endif
                
                // Normal map for distortion - 타입 수정!
                float3 normalTex = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uvnormal));
                float2 normal = normalTex.xy;
                
                // Distortion
                float2 distortUV = screenUV + normal * _DistortFactor * 0.01;
                half3 distortColor = SampleSceneColor(distortUV);
                
                // Mask
                half4 mask = SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uvmask);
                
                // Final color composition
                half4 finalColor = half4(distortColor, 1.0);
                finalColor.rgb *= _TintColor.rgb * i.color.rgb;
                finalColor.a = _TintColor.a * i.color.a * mask.a;
                
                return finalColor;
            }
            ENDHLSL
        }
    }
    
    FallBack "Transparent/VertexLit"
}