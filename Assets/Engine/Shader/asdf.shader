// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/Default"
{
    Properties
    {
         _PaletteNum("Palette Number", Float) = 1
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        _PaletteTex("Palette", 2D) = "white" {}
        
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        
            // Was an include
            #ifndef UNITY_SPRITES_INCLUDED
                #define UNITY_SPRITES_INCLUDED

                #include "UnityCG.cginc"

                #ifdef UNITY_INSTANCING_ENABLED

                    UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
                    // SpriteRenderer.Color while Non-Batched/Instanced.
                    UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
                    // this could be smaller but that's how bit each entry is regardless of type
                    UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
                    UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

                    #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
                    #define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

                #endif // instancing

                CBUFFER_START(UnityPerDrawSprite)
            
                #ifndef UNITY_INSTANCING_ENABLED
                    fixed4 _RendererColor;
                    fixed2 _Flip;
                #endif
                float _EnableExternalAlpha;
                CBUFFER_END

                // Material Color.
                fixed4 _Color;

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
                {
                    return float4(pos.xy * flip, pos.z, 1.0);
                }

                v2f SpriteVert(appdata_t IN)
                {
                    v2f OUT;

                    UNITY_SETUP_INSTANCE_ID(IN);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                    OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
                    OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                    OUT.texcoord = IN.texcoord;
                    OUT.color = IN.color *_Color* _RendererColor;
                    OUT.color = IN.color;
                    #ifdef PIXELSNAP_ON
                    OUT.vertex = UnityPixelSnap(OUT.vertex);
                    #endif

                    return OUT;
                }

                sampler2D _MainTex;
                sampler2D _AlphaTex;
                sampler2D _PaletteTex;
                int _PaletteNum;
                fixed4 SampleSpriteTexture(float2 uv)
                {
                    fixed4 color = tex2D(_MainTex, uv);

                #if ETC1_EXTERNAL_ALPHA
                    fixed4 alpha = tex2D(_AlphaTex, uv);
                    color.a = lerp(color.a, alpha.r, _EnableExternalAlpha);
                #endif
                    //color = float4(1,0,0,color.a)
                    return color;
                }

                fixed4 SpriteFrag(v2f IN) : SV_Target
                {
  
                    fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                    fixed4 asdf = c;
                    // ***********************************************************************
                    // Loop through each color on the palette, seeking a match
                    // This is not the most efficient way to do palettes
                    // Because of branching logic
                    // But for the limited use of this shader, the tradeoff is worth it
                    // For quicker and simpler workflow
                    // ***********************************************************************
                    [loop]
                    for (int i = 0; i < 256; i++)
                    {
                        // If matched to current pixel color, apply the selected palette
                        if (all(asdf.rgb == tex2D(_PaletteTex, float2((float)i / 256, 10)))) {
                            asdf.rgb = tex2D(_PaletteTex, float2((float)i / 256, (10.0 - _PaletteNum) / 10));
                            //asdf.rgb = float3(1, 0, 0);
                            c.rgb = asdf * c.a;
                            return c;
                        }
                    }
                    c.rgb = asdf * c.a;
                    return c;

                }

            #endif 

        ENDCG
        }
    }
}
