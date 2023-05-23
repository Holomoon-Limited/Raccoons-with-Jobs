Shader "Doodle_Maskable"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _DoodleSpeed("DoodleSpeed", Float) = 1
        _DoodleIntensity("DoodleIntensity", Float) = 5

        _Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15

        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"=""
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest [unity_GUIZTestMode]
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

        
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEUNLIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _DoodleSpeed;
        float _DoodleIntensity;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        struct Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float
        {
        float3 ObjectSpacePosition;
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float(float _DoodleSpeed, float _DoodleIntensity, Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float IN, out float3 OutVector3_1)
        {
        float _Property_4eddabf893fe414d917bc9731c953f23_Out_0 = _DoodleSpeed;
        float _Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2;
        Unity_Multiply_float_float(_Property_4eddabf893fe414d917bc9731c953f23_Out_0, IN.TimeParameters.x, _Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2);
        float _Floor_197deb787790465d8c23ae89daae2d7e_Out_1;
        Unity_Floor_float(_Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2, _Floor_197deb787790465d8c23ae89daae2d7e_Out_1);
        float2 _TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Floor_197deb787790465d8c23ae89daae2d7e_Out_1.xx), _TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3);
        float _GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2;
        Unity_GradientNoise_float(_TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3, 2, _GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2);
        float _Property_a83634d80368411795a9a875cd4f4db8_Out_0 = _DoodleIntensity;
        float _Divide_4246a5e04e104783a498bd55044a3560_Out_2;
        Unity_Divide_float(_GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2, _Property_a83634d80368411795a9a875cd4f4db8_Out_0, _Divide_4246a5e04e104783a498bd55044a3560_Out_2);
        float _Split_c8452f42ebad4f9484487ea393863eb2_R_1 = IN.ObjectSpacePosition[0];
        float _Split_c8452f42ebad4f9484487ea393863eb2_G_2 = IN.ObjectSpacePosition[1];
        float _Split_c8452f42ebad4f9484487ea393863eb2_B_3 = IN.ObjectSpacePosition[2];
        float _Split_c8452f42ebad4f9484487ea393863eb2_A_4 = 0;
        float _Add_a7ebbf37358e44a29f6b4217516a3096_Out_2;
        Unity_Add_float(_Divide_4246a5e04e104783a498bd55044a3560_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_R_1, _Add_a7ebbf37358e44a29f6b4217516a3096_Out_2);
        float _Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1;
        Unity_Negate_float(_Property_4eddabf893fe414d917bc9731c953f23_Out_0, _Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1);
        float _Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2;
        Unity_Multiply_float_float(_Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1, IN.TimeParameters.x, _Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2);
        float _Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1;
        Unity_Floor_float(_Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2, _Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1);
        float2 _TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1.xx), _TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3);
        float _GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2;
        Unity_GradientNoise_float(_TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3, 2, _GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2);
        float _Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2;
        Unity_Divide_float(_GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2, _Property_a83634d80368411795a9a875cd4f4db8_Out_0, _Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2);
        float _Add_e13be87beb414a429daad3e5ae06d0da_Out_2;
        Unity_Add_float(_Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_G_2, _Add_e13be87beb414a429daad3e5ae06d0da_Out_2);
        float4 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGBA_4;
        float3 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5;
        float2 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RG_6;
        Unity_Combine_float(_Add_a7ebbf37358e44a29f6b4217516a3096_Out_2, _Add_e13be87beb414a429daad3e5ae06d0da_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_B_3, 0, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGBA_4, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RG_6);
        OutVector3_1 = _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_ec7efc460c9f42a29f9778306e6847a8_Out_0 = _DoodleSpeed;
            float _Property_dd2fe2b26fae4440ac2d6908b35e99f4_Out_0 = _DoodleIntensity;
            Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float _Doodle_02e279e52a7044cf9de71dd2f0a28087;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.ObjectSpacePosition = IN.ObjectSpacePosition;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.uv0 = IN.uv0;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.TimeParameters = IN.TimeParameters;
            float3 _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1;
            SG_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float(_Property_ec7efc460c9f42a29f9778306e6847a8_Out_0, _Property_dd2fe2b26fae4440ac2d6908b35e99f4_Out_0, _Doodle_02e279e52a7044cf9de71dd2f0a28087, _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1);
            description.Position = _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.tex, _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.samplerstate, _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_R_4 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.r;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_G_5 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.g;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_B_6 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.b;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_A_7 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.a;
            surface.BaseColor = (_SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.xyz);
            surface.Alpha = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_A_7;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
            output.TimeParameters =                             _TimeParameters.xyz;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
            ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
            // Render State
            Cull Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass

            Stencil
            {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            ColorMask [_ColorMask]
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _DoodleSpeed;
        float _DoodleIntensity;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        struct Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float
        {
        float3 ObjectSpacePosition;
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float(float _DoodleSpeed, float _DoodleIntensity, Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float IN, out float3 OutVector3_1)
        {
        float _Property_4eddabf893fe414d917bc9731c953f23_Out_0 = _DoodleSpeed;
        float _Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2;
        Unity_Multiply_float_float(_Property_4eddabf893fe414d917bc9731c953f23_Out_0, IN.TimeParameters.x, _Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2);
        float _Floor_197deb787790465d8c23ae89daae2d7e_Out_1;
        Unity_Floor_float(_Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2, _Floor_197deb787790465d8c23ae89daae2d7e_Out_1);
        float2 _TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Floor_197deb787790465d8c23ae89daae2d7e_Out_1.xx), _TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3);
        float _GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2;
        Unity_GradientNoise_float(_TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3, 2, _GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2);
        float _Property_a83634d80368411795a9a875cd4f4db8_Out_0 = _DoodleIntensity;
        float _Divide_4246a5e04e104783a498bd55044a3560_Out_2;
        Unity_Divide_float(_GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2, _Property_a83634d80368411795a9a875cd4f4db8_Out_0, _Divide_4246a5e04e104783a498bd55044a3560_Out_2);
        float _Split_c8452f42ebad4f9484487ea393863eb2_R_1 = IN.ObjectSpacePosition[0];
        float _Split_c8452f42ebad4f9484487ea393863eb2_G_2 = IN.ObjectSpacePosition[1];
        float _Split_c8452f42ebad4f9484487ea393863eb2_B_3 = IN.ObjectSpacePosition[2];
        float _Split_c8452f42ebad4f9484487ea393863eb2_A_4 = 0;
        float _Add_a7ebbf37358e44a29f6b4217516a3096_Out_2;
        Unity_Add_float(_Divide_4246a5e04e104783a498bd55044a3560_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_R_1, _Add_a7ebbf37358e44a29f6b4217516a3096_Out_2);
        float _Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1;
        Unity_Negate_float(_Property_4eddabf893fe414d917bc9731c953f23_Out_0, _Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1);
        float _Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2;
        Unity_Multiply_float_float(_Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1, IN.TimeParameters.x, _Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2);
        float _Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1;
        Unity_Floor_float(_Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2, _Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1);
        float2 _TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1.xx), _TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3);
        float _GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2;
        Unity_GradientNoise_float(_TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3, 2, _GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2);
        float _Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2;
        Unity_Divide_float(_GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2, _Property_a83634d80368411795a9a875cd4f4db8_Out_0, _Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2);
        float _Add_e13be87beb414a429daad3e5ae06d0da_Out_2;
        Unity_Add_float(_Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_G_2, _Add_e13be87beb414a429daad3e5ae06d0da_Out_2);
        float4 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGBA_4;
        float3 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5;
        float2 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RG_6;
        Unity_Combine_float(_Add_a7ebbf37358e44a29f6b4217516a3096_Out_2, _Add_e13be87beb414a429daad3e5ae06d0da_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_B_3, 0, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGBA_4, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RG_6);
        OutVector3_1 = _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_ec7efc460c9f42a29f9778306e6847a8_Out_0 = _DoodleSpeed;
            float _Property_dd2fe2b26fae4440ac2d6908b35e99f4_Out_0 = _DoodleIntensity;
            Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float _Doodle_02e279e52a7044cf9de71dd2f0a28087;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.ObjectSpacePosition = IN.ObjectSpacePosition;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.uv0 = IN.uv0;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.TimeParameters = IN.TimeParameters;
            float3 _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1;
            SG_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float(_Property_ec7efc460c9f42a29f9778306e6847a8_Out_0, _Property_dd2fe2b26fae4440ac2d6908b35e99f4_Out_0, _Doodle_02e279e52a7044cf9de71dd2f0a28087, _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1);
            description.Position = _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.tex, _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.samplerstate, _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_R_4 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.r;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_G_5 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.g;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_B_6 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.b;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_A_7 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.a;
            surface.Alpha = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_A_7;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
            output.TimeParameters =                             _TimeParameters.xyz;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
            ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
            // Render State
            Cull Back
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass

            Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _DoodleSpeed;
        float _DoodleIntensity;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        struct Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float
        {
        float3 ObjectSpacePosition;
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float(float _DoodleSpeed, float _DoodleIntensity, Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float IN, out float3 OutVector3_1)
        {
        float _Property_4eddabf893fe414d917bc9731c953f23_Out_0 = _DoodleSpeed;
        float _Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2;
        Unity_Multiply_float_float(_Property_4eddabf893fe414d917bc9731c953f23_Out_0, IN.TimeParameters.x, _Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2);
        float _Floor_197deb787790465d8c23ae89daae2d7e_Out_1;
        Unity_Floor_float(_Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2, _Floor_197deb787790465d8c23ae89daae2d7e_Out_1);
        float2 _TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Floor_197deb787790465d8c23ae89daae2d7e_Out_1.xx), _TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3);
        float _GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2;
        Unity_GradientNoise_float(_TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3, 2, _GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2);
        float _Property_a83634d80368411795a9a875cd4f4db8_Out_0 = _DoodleIntensity;
        float _Divide_4246a5e04e104783a498bd55044a3560_Out_2;
        Unity_Divide_float(_GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2, _Property_a83634d80368411795a9a875cd4f4db8_Out_0, _Divide_4246a5e04e104783a498bd55044a3560_Out_2);
        float _Split_c8452f42ebad4f9484487ea393863eb2_R_1 = IN.ObjectSpacePosition[0];
        float _Split_c8452f42ebad4f9484487ea393863eb2_G_2 = IN.ObjectSpacePosition[1];
        float _Split_c8452f42ebad4f9484487ea393863eb2_B_3 = IN.ObjectSpacePosition[2];
        float _Split_c8452f42ebad4f9484487ea393863eb2_A_4 = 0;
        float _Add_a7ebbf37358e44a29f6b4217516a3096_Out_2;
        Unity_Add_float(_Divide_4246a5e04e104783a498bd55044a3560_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_R_1, _Add_a7ebbf37358e44a29f6b4217516a3096_Out_2);
        float _Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1;
        Unity_Negate_float(_Property_4eddabf893fe414d917bc9731c953f23_Out_0, _Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1);
        float _Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2;
        Unity_Multiply_float_float(_Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1, IN.TimeParameters.x, _Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2);
        float _Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1;
        Unity_Floor_float(_Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2, _Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1);
        float2 _TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1.xx), _TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3);
        float _GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2;
        Unity_GradientNoise_float(_TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3, 2, _GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2);
        float _Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2;
        Unity_Divide_float(_GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2, _Property_a83634d80368411795a9a875cd4f4db8_Out_0, _Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2);
        float _Add_e13be87beb414a429daad3e5ae06d0da_Out_2;
        Unity_Add_float(_Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_G_2, _Add_e13be87beb414a429daad3e5ae06d0da_Out_2);
        float4 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGBA_4;
        float3 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5;
        float2 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RG_6;
        Unity_Combine_float(_Add_a7ebbf37358e44a29f6b4217516a3096_Out_2, _Add_e13be87beb414a429daad3e5ae06d0da_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_B_3, 0, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGBA_4, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RG_6);
        OutVector3_1 = _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_ec7efc460c9f42a29f9778306e6847a8_Out_0 = _DoodleSpeed;
            float _Property_dd2fe2b26fae4440ac2d6908b35e99f4_Out_0 = _DoodleIntensity;
            Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float _Doodle_02e279e52a7044cf9de71dd2f0a28087;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.ObjectSpacePosition = IN.ObjectSpacePosition;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.uv0 = IN.uv0;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.TimeParameters = IN.TimeParameters;
            float3 _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1;
            SG_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float(_Property_ec7efc460c9f42a29f9778306e6847a8_Out_0, _Property_dd2fe2b26fae4440ac2d6908b35e99f4_Out_0, _Doodle_02e279e52a7044cf9de71dd2f0a28087, _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1);
            description.Position = _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.tex, _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.samplerstate, _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_R_4 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.r;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_G_5 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.g;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_B_6 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.b;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_A_7 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.a;
            surface.Alpha = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_A_7;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
            output.TimeParameters =                             _TimeParameters.xyz;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
            ENDHLSL
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass

            Stencil{
    Ref [_Stencil]
    Comp [_StencilComp]
    Pass [_StencilOp]
    ReadMask [_StencilReadMask]
    WriteMask [_StencilWriteMask]
}
ColorMask [_ColorMask]
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEFORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _DoodleSpeed;
        float _DoodleIntensity;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Floor_float(float In, out float Out)
        {
            Out = floor(In);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        struct Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float
        {
        float3 ObjectSpacePosition;
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float(float _DoodleSpeed, float _DoodleIntensity, Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float IN, out float3 OutVector3_1)
        {
        float _Property_4eddabf893fe414d917bc9731c953f23_Out_0 = _DoodleSpeed;
        float _Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2;
        Unity_Multiply_float_float(_Property_4eddabf893fe414d917bc9731c953f23_Out_0, IN.TimeParameters.x, _Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2);
        float _Floor_197deb787790465d8c23ae89daae2d7e_Out_1;
        Unity_Floor_float(_Multiply_aa8c3e08cace4e468978ba60c9864777_Out_2, _Floor_197deb787790465d8c23ae89daae2d7e_Out_1);
        float2 _TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Floor_197deb787790465d8c23ae89daae2d7e_Out_1.xx), _TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3);
        float _GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2;
        Unity_GradientNoise_float(_TilingAndOffset_4ead06f3047f49baaf994063c320b77b_Out_3, 2, _GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2);
        float _Property_a83634d80368411795a9a875cd4f4db8_Out_0 = _DoodleIntensity;
        float _Divide_4246a5e04e104783a498bd55044a3560_Out_2;
        Unity_Divide_float(_GradientNoise_a223daf9c78c4e0bade3d6e3f0b8f829_Out_2, _Property_a83634d80368411795a9a875cd4f4db8_Out_0, _Divide_4246a5e04e104783a498bd55044a3560_Out_2);
        float _Split_c8452f42ebad4f9484487ea393863eb2_R_1 = IN.ObjectSpacePosition[0];
        float _Split_c8452f42ebad4f9484487ea393863eb2_G_2 = IN.ObjectSpacePosition[1];
        float _Split_c8452f42ebad4f9484487ea393863eb2_B_3 = IN.ObjectSpacePosition[2];
        float _Split_c8452f42ebad4f9484487ea393863eb2_A_4 = 0;
        float _Add_a7ebbf37358e44a29f6b4217516a3096_Out_2;
        Unity_Add_float(_Divide_4246a5e04e104783a498bd55044a3560_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_R_1, _Add_a7ebbf37358e44a29f6b4217516a3096_Out_2);
        float _Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1;
        Unity_Negate_float(_Property_4eddabf893fe414d917bc9731c953f23_Out_0, _Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1);
        float _Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2;
        Unity_Multiply_float_float(_Negate_6ce1bd66c0d64adbb7d1745eb0387daf_Out_1, IN.TimeParameters.x, _Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2);
        float _Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1;
        Unity_Floor_float(_Multiply_00bc8bc608964c2d86d216b97e67af8c_Out_2, _Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1);
        float2 _TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Floor_297cc8b8ebfe410a980469be36de3d2d_Out_1.xx), _TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3);
        float _GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2;
        Unity_GradientNoise_float(_TilingAndOffset_c460854cb1474951bb05d9a61258cf36_Out_3, 2, _GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2);
        float _Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2;
        Unity_Divide_float(_GradientNoise_19c460fdbae444d3ab3e336100c1f690_Out_2, _Property_a83634d80368411795a9a875cd4f4db8_Out_0, _Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2);
        float _Add_e13be87beb414a429daad3e5ae06d0da_Out_2;
        Unity_Add_float(_Divide_7d1fee6aca8148dabf1bfd2564dcf9b8_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_G_2, _Add_e13be87beb414a429daad3e5ae06d0da_Out_2);
        float4 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGBA_4;
        float3 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5;
        float2 _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RG_6;
        Unity_Combine_float(_Add_a7ebbf37358e44a29f6b4217516a3096_Out_2, _Add_e13be87beb414a429daad3e5ae06d0da_Out_2, _Split_c8452f42ebad4f9484487ea393863eb2_B_3, 0, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGBA_4, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5, _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RG_6);
        OutVector3_1 = _Combine_88c99d3c8f50434b9b6ef9fc4d611b36_RGB_5;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_ec7efc460c9f42a29f9778306e6847a8_Out_0 = _DoodleSpeed;
            float _Property_dd2fe2b26fae4440ac2d6908b35e99f4_Out_0 = _DoodleIntensity;
            Bindings_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float _Doodle_02e279e52a7044cf9de71dd2f0a28087;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.ObjectSpacePosition = IN.ObjectSpacePosition;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.uv0 = IN.uv0;
            _Doodle_02e279e52a7044cf9de71dd2f0a28087.TimeParameters = IN.TimeParameters;
            float3 _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1;
            SG_Doodle_ef42389ade05b5c4fb161e977b59cd4f_float(_Property_ec7efc460c9f42a29f9778306e6847a8_Out_0, _Property_dd2fe2b26fae4440ac2d6908b35e99f4_Out_0, _Doodle_02e279e52a7044cf9de71dd2f0a28087, _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1);
            description.Position = _Doodle_02e279e52a7044cf9de71dd2f0a28087_OutVector3_1;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.tex, _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.samplerstate, _Property_9f901ce7da4844d88ea33ab3ac1aee0f_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_R_4 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.r;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_G_5 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.g;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_B_6 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.b;
            float _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_A_7 = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.a;
            surface.BaseColor = (_SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_RGBA_0.xyz);
            surface.Alpha = _SampleTexture2D_1af0dadb809a40489bfd467fbbad622e_A_7;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
            output.TimeParameters =                             _TimeParameters.xyz;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}