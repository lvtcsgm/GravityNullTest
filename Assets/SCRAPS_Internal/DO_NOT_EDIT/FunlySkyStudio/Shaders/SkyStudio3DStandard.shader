﻿// Copyright(c) 2017 Funly LLC
//
// Author: Jason Ederle
// Description: Renders a customizable sky for a 3D skybox sphere.
// Contact: jason@funly.io

Shader "Funly/Sky Studio/Skybox/3D Standard" {
  Properties {
    // Gradient Sky.
    _GradientSkyUpperColor("Sky Top Color", Color) = (.47, .45, .75, 1)            // Color of sky.
    _GradientSkyMiddleColor("Sky Middle Color", Color) = (1, 1, 1, 1)              // Color in the middle of sky 3 way gradient.
    _GradientSkyLowerColor("Sky Lower Color", Color) = (.7, .53, .69, 1)           // Color of horizon.
    _GradientFadeBegin("Horizon Fade Begin", Range(-1, 1)) = -.179                 // Position to begin horizon fade into sky.
    _GradientFadeEnd("Horizon Fade End", Range(-1, 1)) = .302                      // Position to end horizon fade into sky.
    _GradientFadeMiddlePosition("Horizon Fade Middle Position", Range(0, 1)) = .5  // Position of the middle gradient color.

    // Shrink stars closer to horizon.
    _HorizonScaleFactor("Star Horizon Scale Factor", Range(0, 1)) = .7

    // Cubemap background.
    [NoScaleOffset]_MainTex("Background Cubemap", CUBE) = "white" {}      // Cubemap for custom background behind stars.

    // Star fading.
    _StarFadeBegin("Star Fade Begin", Range(-1, 1)) = .067                // Height to begin star fade in.
    _StarFadeEnd("Star Fade End", Range(-1, 1)) = .36                     // Height where all stars are faded in at.

    // Star Layer 1.
    [NoScaleOffset]_StarLayer1Tex("Star 1 Texture", 2D) = "white" {}
    _StarLayer1Color("Star Layer 1 - Color", Color) = (1, 1, 1, 1)                              // Color tint for stars.
    _StarLayer1Density("Star Layer 1 - Star Density", Range(0, .05)) = .01                      // Space between stars.
    _StarLayer1MaxRadius("Star Layer 1 - Star Size", Range(0, .1)) = .007                       // Max radius of stars.
    _StarLayer1TwinkleAmount("Star Layer 1 - Twinkle Amount", Range(0, 1)) = .775               // Percent of star twinkle amount.
    _StarLayer1TwinkleSpeed("Star Layer 1 - Twinkle Speed", float) = 2.0                        // Twinkle speed.
    _StarLayer1RotationSpeed("Star Layer 1 - Rotation Speed", float) = 2                        // Rotation speed of stars.
    _StarLayer1EdgeFade("Star Layer 1 - Edge Feathering", Range(0.0001, .9999)) = .2            // Softness of star blending with background.
    _StarLayer1HDRBoost("Star Layer 1 - HDR Bloom Boost", Range(1, 10)) = 1.0                   // Boost star colors so they glow with bloom filters.
    _StarLayer1SpriteDimensions("Star Layer 1 Sprite Dimensions", Vector) = (0, 0, 0, 0)        // Dimensions of columns (x), and rows (y) in sprite sheet.
    _StarLayer1SpriteItemCount("Star Layer 1 Sprite Total Items", int) = 1                      // Total number of items in sprite sheet.
    _StarLayer1SpriteAnimationSpeed("Star Layer 1 Sprite Speed", int) = 1                       // Speed of the sprite sheet animation.
    [NoScaleOffset]_StarLayer1DataTex("Star Layer 1 - Data Image", 2D) = "black" {}             // Data image with star positions.
    
    // Star Layer 2. - See property descriptions from star layer 1.
    [NoScaleOffset]_StarLayer2Tex("Star 2 Texture", 2D) = "white" {}
    _StarLayer2Color("Star Layer 2 - Color", Color) = (1, .5, .96, 1)
    _StarLayer2Density("Star Layer 2 - Star Density", Range(0, .05)) = .01
    _StarLayer2MaxRadius("Star Layer 2 - Star Size", Range(0, .4)) = .014
    _StarLayer2TwinkleAmount("Star Layer 2 - Twinkle Amount", Range(0, 1)) = .875
    _StarLayer2TwinkleSpeed("Star Layer 2 - Twinkle Speed", float) = 3.0
    _StarLayer2RotationSpeed("Star Layer 2 - Rotation Speed", float) = 2
    _StarLayer2EdgeFade("Star Layer 2 - Edge Feathering", Range(0.0001, .9999)) = .2
    _StarLayer2HDRBoost("Star Layer 2 - HDR Bloom Boost", Range(1, 10)) = 1.0
    _StarLayer2SpriteDimensions("Star Layer 2 Sprite Dimensions", Vector) = (0, 0, 0, 0)
    _StarLayer2SpriteItemCount("Star Layer 2 Sprite Total Items", int) = 1
    _StarLayer2SpriteAnimationSpeed("Star Layer 2 Sprite Speed", int) = 1
    [NoScaleOffset]_StarLayer2DataTex("Star Layer 2 - Data Image", 2D) = "black" {}

    // Star Layer 3. - See property descriptions from star layer 1.
    [NoScaleOffset]_StarLayer3Tex("Star 3 Texture", 2D) = "white" {}
    _StarLayer3Color("Star Layer 3 - Color", Color) = (.22, 1, .55, 1)
    _StarLayer3Density("Star Layer 3 - Star Density", Range(0, .05)) = .01
    _StarLayer3MaxRadius("Star Layer 3 - Star Size", Range(0, .4)) = .01
    _StarLayer3TwinkleAmount("Star Layer 3 - Twinkle Amount", Range(0, 1)) = .7
    _StarLayer3TwinkleSpeed("Star Layer 3 - Twinkle Speed", float) = 1.0
    _StarLayer3RotationSpeed("Star Layer 3 - Rotation Speed", float) = 2
    _StarLayer3EdgeFade("Star Layer 3 - Edge Feathering", Range(0.0001, .9999)) = .2
    _StarLayer3HDRBoost("Star Layer 3 - HDR Bloom Boost", Range(1, 10)) = 1.0
    _StarLayer3SpriteDimensions("Star Layer 3 Sprite Dimensions", Vector) = (0, 0, 0, 0)
    _StarLayer3SpriteItemCount("Star Layer 3 Sprite Total Items", int) = 1
    _StarLayer3SpriteAnimationSpeed("Star Layer 3 Sprite Speed", int) = 1
    [NoScaleOffset]_StarLayer3DataTex("Star Layer 3 - Data Image", 2D) = "black" {}

    // Moon properties.
    [NoScaleOffset]_MoonTex("Moon Texture", 2D) = "white" {}               // Moon image.
    _MoonColor("Moon Color", Color) = (.66, .65, .55, 1)                   // Moon tint color.
    _MoonRadius("Moon Size", Range(0, 1)) = .1                             // Radius of the moon.
    _MoonEdgeFade("Moon Edge Feathering", Range(0.0001, .9999)) = .3       // Soften edges of moon texture.
    _MoonHDRBoost("Moon HDR Bloom Boost", Range(1, 10)) = 1                // Control brightness for HDR bloom filter.
    _MoonSpriteDimensions("Moon Sprite Dimensions", Vector) = (0, 0, 0, 0) // Dimensions of columns (x), and rows (y) in sprite sheet.
    _MoonSpriteItemCount("Moon Sprite Total Items", int) = 1               // Total number of items in sprite sheet.
    _MoonSpriteAnimationSpeed("Moon Sprite Speed", int) = 1                // Speed of the sprite sheet animation.
    _MoonPosition("Moon Position" , Vector) = (0, 0, 0, 0)                 // Moon Position.

    // Sun properties.
    [NoScaleOffset]_SunTex("Sun Texture", 2D) = "white" {}                // Sun image.
    _SunColor("Sun Color", Color) = (.66, .65, .55, 1)                    // Sun tint color.
    _SunRadius("Sun Size", Range(0, 1)) = .1                              // Radius of the Sun.
    _SunEdgeFade("Sun Edge Feathering", Range(0.0001, .9999)) = .3        // Soften edges of Sun texture.
    _SunHDRBoost("Sun HDR Bloom Boost", Range(1, 10)) = 1                 // Control brightness for HDR bloom filter.
    _SunSpriteDimensions("Sun Sprite Dimensions", Vector) = (0, 0, 0, 0)  // Dimensions of columns (x), and rows (y) in sprite sheet.
    _SunSpriteItemCount("Sun Sprite Total Items", int) = 1                // Total number of items in sprite sheet.
    _SunSpriteAnimationSpeed("Sun Sprite Speed", int) = 1                 // Speed of the sprite sheet animation.
    _SunPosition("Sun Position Data" , Vector) = (0, 0, 0, 0)             // Sun position.

    // Noise Cloud properties.
    [NoScaleOffset]_CloudNoiseTexture("Cloud Texture", 2D) = "black" {}         // Cloud noise texture.
    _CloudFadePosition("Cloud Fade Position", Range(0, .97)) = .74              // Position that the clouds will begin fading out at.
    _CloudFadeAmount("Cloud Fade Amount", Range(0, 1)) = .5                     // Amount of fade to clouds.
    _CloudDensity("Cloud Density", Range(0, 1)) = .25                           // Cloud density.
    _CloudSpeed("Cloud Speed", Range(0, 1)) = .1                                // Cloud speed.
    _CloudDirection("Cloud Direction", Range(0, 6.283)) = 1.0                   // Cloud direction.
    _CloudHeight("Cloud Height", Range(0, 1)) = .5                              // Cloud height, by scaling up/down texture.
    _CloudTextureTiling("Cloud Tiling", Range(.01, 10)) = 2                     // Cloud tiling which changes visible resolution.
    _CloudColor1("Cloud 1 Color", Color) = (1, 1, 1, 1)                         // Cloud color 1.
    _CloudColor2("Cloud 2 Color", Color) = (.6, .6, .6, 1)                      // Cloud color 2.

    // Cubemap Clouds.
    [NoScaleOffset]_CloudCubemapTexture("Cloud Cubemap", CUBE) = "clear" {}                       // Cloud custom texture.
    _CloudCubemapRotationSpeed("Cloud Cubemap Rotation Speed", Range(-1, 1)) = .01                // Rotation speed and direction.
    _CloudCubemapTintColor("Cloud Cubemap Tint Color", Color) = (1, 1, 1, 1)                      // Tint color.
    _CloudCubemapHeight("Cloud Cubemap Height", Range(-1, 1)) = 0                                 // Cloud height
    [NoScaleOffset]_CloudCubemapDoubleTexture("Cloud Double Cubemap", CUBE) = "clear" {}          // Cloud custom texture.
    _CloudCubemapDoubleLayerHeight("Cloud Cubemap Double Layer Offset", float) = 0                // Offset of the duplicate cloud layer.
    _CloudCubemapDoubleLayerRotationSpeed("Cloud Cubemap Double Layer Rotation Speed", Range(-1, 1)) = .02
    _CloudCubemapDoubleLayerTintColor("Cloud Cubemap Double Tint Color", Color) = (1, 1, 1, 1)

    // Cubemap Normal Clouds.
    [NoScaleOffset]_CloudCubemapNormalTexture("Cloud Cubemap Normal Texture", CUBE) = "clear" {}                      // Cubemap texture with normals.
    _CloudCubemapNormalAmbientIntensity("Cloud Ambient Light Intensity", Range(0, 1)) = .2                            // Ambient light intensity.
    _CloudCubemapNormalRotationSpeed("Cloud Cubemap Normal Rotation Speed", Range(-1, 1)) = .01                       // Rotation speed and direction.
    _CloudCubemapNormalLitColor("Cloud Cubemap Normal Lit Color", Color) = (1, 1, 1, 1)                               // Lit color.
    _CloudCubemapNormalShadowColor("Cloud Cubemap Normal Shadow Color", Color) = (0, 0, 0, 1)                         // Shadow color.
    _CloudCubemapNormalHeight("Cloud Cubemap Normal Height", Range(-1, 1)) = 0                                        // Cloud height
    _CloudCubemapNormalToLight("Cloud Cubemap Light Direction", Vector) = (0, 1, 0, 0)                                // Direction to light.
    [NoScaleOffset]_CloudCubemapNormalDoubleTexture("Cloud Cubemap Normal Double Cubemap", CUBE) = "clear" {}         // Cloud custom texture.
    _CloudCubemapNormalDoubleLayerHeight("Cloud Cubemap Normal Double Layer Offset", float) = 0                       // Offset of the duplicate cloud layer.
    _CloudCubemapNormalDoubleLayerRotationSpeed("Cloud Cubemap Normal Double Layer Rotation Speed", Range(-1, 1)) = .02
    _CloudCubemapNormalDoubleLitColor("Cloud Cubemap Normal Double Lit Color", Color) = (1, 1, 1, 1)                        // Double layer lit color.
    _CloudCubemapNormalDoubleShadowColor("Cloud Cubemap Normal Double Shadow Color", Color) = (0, 0, 0, 1)                  // Double layer shadow color.

    // 2D Custom Texture clouds.
    //[NoScaleOffset]_ArtCloudCustomTexture("Art Cloud Texture", 2D) = "black" {} // Art cloud custom texture.

    _HorizonFogColor("Fog Color", Color) = (1, 1, 1, 1)               // Fog color.
    _HorizonFogDensity("Fog Density", Range(0, 1)) = .12              // Density and visibility of the fog.
    _HorizonFogLength("Fog Height", Range(.03, 1)) = .1               // Height the fog reaches up into the skybox.

    _DebugPointsCount("Debug Points Count", Range(0, 100)) = 0       // Used for visualizing orbit paths in editor only.
    _DebugPointRadius("Debug Point Radius", Range(0, .1)) = .03      // Size of sphere point dots when visualized.

    // _AtmosphereNoiseTex("Atmosphere Tex", CUBE) = "white" {}
    // _AtmosphereSpeed("Atmosphere Speed", Range(0, 10)) = 1
  }

  SubShader {
    Tags { "RenderType"="Opaque" "Queue"="Background" "IgnoreProjector"="true" "PreviewType" = "Skybox" }
    LOD 100
    ZWrite Off
    Cull Off

    Pass {
      CGPROGRAM
      #pragma target 3.5
      #pragma multi_compile_fog

      #pragma shader_feature GRADIENT_BACKGROUND
      #pragma shader_feature STAR_LAYER_1
      #pragma shader_feature STAR_LAYER_2
      #pragma shader_feature STAR_LAYER_3
      #pragma shader_feature STAR_LAYER_1_CUSTOM_TEXTURE
      #pragma shader_feature STAR_LAYER_2_CUSTOM_TEXTURE
      #pragma shader_feature STAR_LAYER_3_CUSTOM_TEXTURE
      #pragma shader_feature STAR_LAYER_1_SPRITE_SHEET
      #pragma shader_feature STAR_LAYER_2_SPRITE_SHEET
      #pragma shader_feature STAR_LAYER_3_SPRITE_SHEET
      #pragma shader_feature MOON
      #pragma shader_feature MOON_CUSTOM_TEXTURE
      #pragma shader_feature MOON_SPRITE_SHEET
      #pragma shader_feature SUN
      #pragma shader_feature SUN_CUSTOM_TEXTURE
      #pragma shader_feature SUN_SPRITE_SHEET
      #pragma shader_feature HORIZON_FOG
      #pragma shader_feature CLOUDS
      #pragma shader_feature NOISE_CLOUDS
      #pragma shader_feature CUBEMAP_CLOUDS
      #pragma shader_feature CUBEMAP_NORMAL_CLOUDS
      #pragma shader_feature CUBEMAP_CLOUD_DOUBLE_LAYER
      #pragma shader_feature CUBEMAP_NORMAL_CLOUD_DOUBLE_LAYER
      #pragma shader_feature CUBEMAP_CLOUD_FORMAT_RGB
      #pragma shader_feature CUBEMAP_CLOUD_FORMAT_RGBA
      #pragma shader_feature CUBEMAP_CLOUD_DOUBLE_LAYER_CUSTOM_TEXTURE
      #pragma shader_feature CUBEMAP_NORMAL_CLOUD_DOUBLE_LAYER_CUSTOM_TEXTURE
      #pragma shader_feature SUN_ALPHA_BLEND
      #pragma shader_feature SUN_ROTATION
      #pragma shader_feature MOON_ALPHA_BLEND
      #pragma shader_feature MOON_ROTATION
      #pragma shader_feature RENDER_DEBUG_POINTS

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "Utility/SkyMathUtilities.cginc"
       
      struct appdata {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
      };

      struct v2f {
        float4 vertex : SV_POSITION;
        float3 smoothVertex : TEXCOORD2;

#if CLOUDS && NOISE_CLOUDS
        float4 cloudUVs : TEXCOORD3;
#endif
      };

      // Cubemap.
      samplerCUBE _MainTex;
      float4 _MainTex_ST;

      // Gradient sky.
      float _UseGradientSky;
      float4 _GradientSkyUpperColor;
      float4 _GradientSkyMiddleColor;
      float4 _GradientSkyLowerColor;
      float _GradientFadeMiddlePosition;

      float _GradientFadeBegin;
      float _GradientFadeEnd;

      float _StarFadeBegin;
      float _StarFadeEnd;

#ifdef STAR_LAYER_1
      // Star Layer 1
      sampler2D _StarLayer1Tex;
      float4 _StarLayer1Tex_ST;
      float4 _StarLayer1Color;
      float _StarLayer1MaxRadius;
      float _StarLayer1Density;
      float _StarLayer1TwinkleAmount;
      float _StarLayer1TwinkleSpeed;
      float _StarLayer1RotationSpeed;
      float _StarLayer1EdgeFade;
      sampler2D _StarLayer1DataTex;
      float4 _StarLayer1DataTex_ST;;
      float _StarLayer1HDRBoost;
  #ifdef STAR_LAYER_1_CUSTOM_TEXTURE
    #ifdef STAR_LAYER_1_SPRITE_SHEET
      float2 _StarLayer1SpriteDimensions;
      int _StarLayer1SpriteItemCount;
      int _StarLayer1SpriteAnimationSpeed;
    #endif
  #endif
#endif

#ifdef STAR_LAYER_2
      // Star Layer 2
      sampler2D _StarLayer2Tex;
      float4 _StarLayer2Tex_ST;
      float4 _StarLayer2Color;
      float _StarLayer2MaxRadius;
      float _StarLayer2Density;
      float _StarLayer2TwinkleAmount;
      float _StarLayer2TwinkleSpeed;
      float _StarLayer2RotationSpeed;
      float _StarLayer2EdgeFade;
      sampler2D _StarLayer2DataTex;
      float4 _StarLayer2DataTex_ST;;
      float _StarLayer2HDRBoost;
  #ifdef STAR_LAYER_2_CUSTOM_TEXTURE
    #ifdef STAR_LAYER_2_SPRITE_SHEET
      float2 _StarLayer2SpriteDimensions;
      int _StarLayer2SpriteItemCount;
      int _StarLayer2SpriteAnimationSpeed;
    #endif
  #endif
#endif

#ifdef STAR_LAYER_3
      // Star Layer 3
      sampler2D _StarLayer3Tex;
      float4 _StarLayer3Tex_ST;
      float4 _StarLayer3Color;
      float _StarLayer3MaxRadius;
      float _StarLayer3Density;
      float _StarLayer3TwinkleAmount;
      float _StarLayer3TwinkleSpeed;
      float _StarLayer3RotationSpeed;
      float _StarLayer3EdgeFade;
      sampler2D _StarLayer3DataTex;
      float4 _StarLayer3DataTex_ST;;
      float _StarLayer3HDRBoost;
  #ifdef STAR_LAYER_3_CUSTOM_TEXTURE
    #ifdef STAR_LAYER_3_SPRITE_SHEET
      float2 _StarLayer3SpriteDimensions;
      int _StarLayer3SpriteItemCount;
      int _StarLayer3SpriteAnimationSpeed;
    #endif
  #endif
#endif

      float _HorizonScaleFactor;

#ifdef MOON
      // Moon
  #ifdef MOON_CUSTOM_TEXTURE
      sampler2D _MoonTex;
      float _MoonRotationSpeed;
    #ifdef MOON_SPRITE_SHEET
      float2 _MoonSpriteDimensions;
      int _MoonSpriteItemCount;
      int _MoonSpriteAnimationSpeed;
    #endif
  #endif
      float4 _MoonColor;
      float _MoonRadius;
      float _MoonEdgeFade;
      float _MoonHDRBoost;
      float4 _MoonPosition;
#endif

#ifdef SUN
      // Sun
  #ifdef SUN_CUSTOM_TEXTURE
      sampler2D _SunTex;
      float _SunRotationSpeed;
    #ifdef SUN_SPRITE_SHEET
      float2 _SunSpriteDimensions;
      int _SunSpriteItemCount;
      int _SunSpriteAnimationSpeed;
    #endif
  #endif
      float4 _SunColor;
      float _SunRadius;
      float _SunEdgeFade;
      float _SunHDRBoost;
      float4 _SunPosition;
#endif

#ifdef CLOUDS
      // Generic cloud uniforms.
      float _CloudSpeed;
      float _CloudHeight;
      
#if NOISE_CLOUDS
      sampler2D _CloudNoiseTexture;
      float _CloudDensity;
      float _CloudDirection;
      float _CloudFadePosition;
      float _CloudFadeAmount;
      float _CloudTextureTiling;
      float4 _CloudColor1;
      float4 _CloudColor2;
#elif CUBEMAP_CLOUDS
      samplerCUBE _CloudCubemapTexture;
      float _CloudCubemapRotationSpeed;
      float4 _CloudCubemapTintColor;
      float _CloudCubemapHeight;

      #if CUBEMAP_CLOUD_DOUBLE_LAYER
        float _CloudCubemapDoubleLayerHeight;
        float _CloudCubemapDoubleLayerRotationSpeed;
        float4 _CloudCubemapDoubleLayerTintColor;

        #if CUBEMAP_CLOUD_DOUBLE_LAYER_CUSTOM_TEXTURE
          samplerCUBE _CloudCubemapDoubleTexture;
        #endif

      #endif // CUBEMAP_CLOUD_DOUBLE_LAYER
#elif CUBEMAP_NORMAL_CLOUDS
      samplerCUBE _CloudCubemapNormalTexture;
      float _CloudCubemapNormalAmbientIntensity;
      float _CloudCubemapNormalRotationSpeed;
      float _CloudCubemapNormalHeight;
      float4 _CloudCubemapNormalLitColor;
      float4 _CloudCubemapNormalShadowColor;
      float3 _CloudCubemapNormalToLight;

      #if CUBEMAP_NORMAL_CLOUD_DOUBLE_LAYER
      float _CloudCubemapNormalDoubleLayerHeight;
      float _CloudCubemapNormalDoubleLayerRotationSpeed;
      float4 _CloudCubemapNormalDoubleLitColor;
      float4 _CloudCubemapNormalDoubleShadowColor;

      #if CUBEMAP_NORMAL_CLOUD_DOUBLE_LAYER_CUSTOM_TEXTURE
        samplerCUBE _CloudCubemapNormalDoubleTexture;  
      #endif

      #endif // CUBEMAP_NORMAL_CLOUD_DOUBLE_LAYER
#endif

#endif

#if HORIZON_FOG
      float4 _HorizonFogColor;
      float _HorizonFogDensity;
      float _HorizonFogLength;
#endif

#if RENDER_DEBUG_POINTS
      // This is only used in the editor for debugging and will get compiled out.
      float4 _DebugPoints[100];
      int _DebugPointsCount;
      float _DebugPointRadius;
#endif

      // REMOVEME - Experiment test
      //samplerCUBE _AtmosphereNoiseTex;
      //float _AtmosphereSpeed;

      #define _MAX_CLOUD_COVERAGE 7
      #define _CLOUD_HEIGHT_LIMITS float2(30, 100)
      
      half4 AlphaBlendPartial(half4 top, half4 bottom) {
        half3 ca = top.xyz;
        float aa = top.w;
        half3 cb = bottom.xyz;
        float ab = bottom.w;

        half3 color = (ca * aa + cb * ab * (1 - aa)) / (aa + ab * (1 - aa));
        return half4(color, saturate(aa + ab));
      }

      // Does an over alpha blend.
      half4 AlphaBlend(half4 top, half4 bottom) {
        half3 ca = top.xyz;
        float aa = top.w;
        half3 cb = bottom.xyz;
        float ab = bottom.w;

        half3 color = (ca * aa + cb * ab * (1 - aa)) / (aa + ab * (1 - aa));
        return half4(color, 1);
      }

      float2 CalculateStarRotation(float3 star)
      {
        float3 starPos = float3(star.x, star.y, star.z);

        float yRotationAngle = AngleToReachTarget(starPos.xz, UNITY_HALF_PI);

        starPos = RotateAroundYAxis(starPos, yRotationAngle);

        float xRotationAngle = AngleToReachTarget(starPos.zy, 0.0f);

        return float2(xRotationAngle, yRotationAngle);
      }

      float2 GetUVsForSpherePoint(float3 fragPos, float radius, float3 targetPoint) {
        float2 bodyRotations = CalculateStarRotation(targetPoint);
        float3 projectedPosition = RotatePoint(fragPos, bodyRotations.x, bodyRotations.y);

        // Find our UV position.
        return clamp(float2(
          (projectedPosition.x + radius) / (2.0 * radius),
          (projectedPosition.y + radius) / (2.0 * radius)), 0, 1);
      }

      float4 GetDataFromTexture(sampler2D tex, float2 uv) {
        float4 col = tex2Dlod(tex, float4(uv.x, uv.y, 0.0f, 0.0f));
        //float4 col = tex2D(tex, uv);
        
        #if defined(UNITY_COLORSPACE_GAMMA) == false
        col.xyz = LinearToGammaSpace(col.xyz);
        #endif

        return col;
      }

      inline float4 GetStarDataFromTexture(sampler2D nearbyStarTexture, float2 uv) {
        float4 percentData = GetDataFromTexture(nearbyStarTexture, uv);

        float2 sphericalCoord = ConvertPercentToSphericalCoordinate(percentData.xy);
        return float4(sphericalCoord.x, sphericalCoord.y, percentData.z, 1.0f);
      }

      float2 AnimateStarRotation(float2 starUV, float rotationSpeed, float scale, float2 pivot) {
        return Rotate2d(starUV - pivot, rotationSpeed * _Time.y * scale) + pivot;
      }

      float GetStarRadius(float noise, float maxRadius, float twinkleAmount) {
        float noisePercent = noise;
        float minRadius = clamp((1 - twinkleAmount) * maxRadius, 0, maxRadius);
        return clamp(maxRadius * noise, minRadius, maxRadius) * _HorizonScaleFactor;
      }

      uint GetSpriteTargetIndex(int itemCount, int animationSpeed, float seed) {
        float delta = _Time.y + (10.0f * seed);
        float timePerFrame = 1.0f / (float)animationSpeed;
        int frameIndex = (int)(delta / timePerFrame);
        return (uint)abs(frameIndex % itemCount);
      }

      float2 GetSpriteItemSize(float2 dimensions) {
        return float2(1.0f / dimensions.x, (1.0f / dimensions.x) * (dimensions.x / dimensions.y));
      }

      float2 GetSpriteRotationOrigin(uint targetFrameIndex, float2 dimensions, float2 itemSize) {
        uint rows = (uint)dimensions.y;
        uint columns = (uint)dimensions.x;
        return float2(((float)(targetFrameIndex % columns) * itemSize.x + (itemSize.x / 2.0f)),
          (float)((rows - 1) - (targetFrameIndex / columns)) * itemSize.y + (itemSize.y / 2.0f));
      }

      float2 GetSpriteSheetCoords(float2 uv, float2 dimensions, uint targetFrameIndex, float2 itemSize, uint numItems) {
        uint rows = (uint)dimensions.y;
        uint columns = (uint)dimensions.x;

        float2 scaledUV = float2(uv.x * itemSize.x, uv.y * itemSize.y);
        float2 offset = float2(
          targetFrameIndex % columns * itemSize.x,
          ((rows - 1) - (targetFrameIndex / columns)) * itemSize.y);

        return scaledUV + offset;
      }

      half4 StarColorWithTexture(
          float3 pos,
          float2 starCoords,
          float2 starUV,
          sampler2D starTexture,
          float4 starColorTint,
          float starDensity,
          float radius,
          float twinkleAmount,
          float twinkleSpeed,
          float rotationSpeed,
          float edgeFade,
          sampler2D nearbyStarsTexture,
          float4 gridPointWithNoise) {
        float3 gridPoint = normalize(gridPointWithNoise.xyz);
        float distanceToCenter = distance(pos, gridPoint);

        half4 outputColor = tex2D(starTexture, starUV) * starColorTint;

        // Animate alpha with twinkle wave.
        half twinkleWavePercent = smoothstep(-1, 1, cos(gridPointWithNoise.w * (100 + _Time.y) * twinkleSpeed));
        outputColor *= clamp(twinkleWavePercent, (1 - twinkleAmount), 1);

        // If it's outside the radius, zero is multiplied to clear the color values.
        return outputColor * smoothstep(radius, radius * (1 - edgeFade), distanceToCenter);
      }

      half4 StarColorNoTexture(
          float3 pos,
          float4 starColorTint,
          float starDensity,
          float radius,
          float twinkleAmount,
          float twinkleSpeed,
          float edgeFade,
          sampler2D nearbyStarsTexture,
          float4 gridPointWithNoise) {
        float3 gridPoint = normalize(gridPointWithNoise.xyz);

        float distanceToCenter = distance(pos, gridPoint);

        // Apply a horizon scale so stars are less visible with distance.
        radius *= _HorizonScaleFactor;

        half4 outputColor = starColorTint;

        // Animate alpha with twinkle wave.
        half twinkleWavePercent = smoothstep(-1, 1, cos(gridPointWithNoise.w * (100 + _Time.y) * twinkleSpeed));
        outputColor *= clamp(twinkleWavePercent, (1 - twinkleAmount), 1);

        // If it's outside the radius, zero is multiplied to clear the color values.
        return outputColor * smoothstep(radius, radius * (1 - edgeFade), distanceToCenter);
      }

      half4 StarColorFromAllGrids(float3 pos) {
        float4 nearbyStar = float4(0, 0, 0, 0);
        float4 allStarColors = float4(0, 0, 0, 0);

        // Nearby star is unpacked direction and noise.
        float2 sphereFragCoord = DirectionToSphericalCoordinate(pos);
        float2 starTextureUV = ConvertSphericalCoordateToUV(sphereFragCoord);
        float4 nearbySphericalStar = float4(0, 0, 0, 0);
        float3 nearbyStarDirection = float3(0, 0, 0);

#ifdef STAR_LAYER_3
        nearbySphericalStar = GetStarDataFromTexture(_StarLayer3DataTex, starTextureUV);

        nearbyStarDirection = SphericalCoordinateToDirection(nearbySphericalStar.xy);
        nearbyStar = float4(nearbyStarDirection.x, nearbyStarDirection.y, nearbyStarDirection.z, nearbySphericalStar.z);
        
        if (distance(pos, nearbyStar) <= _StarLayer3MaxRadius) {
          float radius = GetStarRadius(nearbyStar.w, _StarLayer3MaxRadius, _StarLayer3TwinkleAmount);

  #ifdef STAR_LAYER_3_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, radius, nearbyStar.xyz);
          float2 pivot = float2(.5f, .5f);
    #if STAR_LAYER_3_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_StarLayer3SpriteItemCount, _StarLayer3SpriteAnimationSpeed, nearbyStar.w);
          float2 spriteItemSize = GetSpriteItemSize(_StarLayer3SpriteDimensions);

          texUV = GetSpriteSheetCoords(texUV, _StarLayer3SpriteDimensions, spriteFrameIndex, spriteItemSize, _StarLayer3SpriteItemCount);
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _StarLayer3SpriteDimensions, spriteItemSize);
    #endif
          texUV = AnimateStarRotation(texUV, _StarLayer3RotationSpeed * nearbyStar.w, 1, pivot);
          
          allStarColors += StarColorWithTexture(
            pos,
            starTextureUV,
            texUV,
            _StarLayer3Tex,
            _StarLayer3Color,
            _StarLayer3Density,
            radius,
            _StarLayer3TwinkleAmount,
            _StarLayer3TwinkleSpeed,
            _StarLayer3RotationSpeed,
            _StarLayer3EdgeFade,
            _StarLayer3DataTex,
            nearbyStar) * _StarLayer3HDRBoost;
            
  #else
          allStarColors += StarColorNoTexture(
            pos,
            _StarLayer3Color,
            _StarLayer3Density,
            radius,
            _StarLayer3TwinkleAmount,
            _StarLayer3TwinkleSpeed,
            _StarLayer3EdgeFade,
            _StarLayer3DataTex,
            nearbyStar) * _StarLayer3HDRBoost;
  #endif      
        }
#endif



#ifdef STAR_LAYER_2
        nearbySphericalStar = GetStarDataFromTexture(_StarLayer2DataTex, starTextureUV);

        nearbyStarDirection = SphericalCoordinateToDirection(nearbySphericalStar.xy);
        nearbyStar = float4(nearbyStarDirection.x, nearbyStarDirection.y, nearbyStarDirection.z, nearbySphericalStar.z);
        
        if (distance(pos, nearbyStar) <= _StarLayer2MaxRadius) {
          float radius = GetStarRadius(nearbyStar.w, _StarLayer2MaxRadius, _StarLayer2TwinkleAmount);

  #ifdef STAR_LAYER_2_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, radius, nearbyStar.xyz);
          float2 pivot = float2(.5f, .5f);
    #if STAR_LAYER_2_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_StarLayer2SpriteItemCount, _StarLayer2SpriteAnimationSpeed, nearbyStar.w);
          float2 spriteItemSize = GetSpriteItemSize(_StarLayer2SpriteDimensions);

          texUV = GetSpriteSheetCoords(texUV, _StarLayer2SpriteDimensions, spriteFrameIndex, spriteItemSize, _StarLayer2SpriteItemCount);
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _StarLayer2SpriteDimensions, spriteItemSize);
    #endif
          texUV = AnimateStarRotation(texUV, _StarLayer2RotationSpeed * nearbyStar.w, 1, pivot);
          
          allStarColors += StarColorWithTexture(
            pos,
            starTextureUV,
            texUV,
            _StarLayer2Tex,
            _StarLayer2Color,
            _StarLayer2Density,
            radius,
            _StarLayer2TwinkleAmount,
            _StarLayer2TwinkleSpeed,
            _StarLayer2RotationSpeed,
            _StarLayer2EdgeFade,
            _StarLayer2DataTex,
            nearbyStar) * _StarLayer2HDRBoost;
            
  #else
          allStarColors += StarColorNoTexture(
            pos,
            _StarLayer2Color,
            _StarLayer2Density,
            radius,
            _StarLayer2TwinkleAmount,
            _StarLayer2TwinkleSpeed,
            _StarLayer2EdgeFade,
            _StarLayer2DataTex,
            nearbyStar) * _StarLayer2HDRBoost;
  #endif      
        }
#endif
       

#ifdef STAR_LAYER_1
        nearbySphericalStar = GetStarDataFromTexture(_StarLayer1DataTex, starTextureUV);

        nearbyStarDirection = SphericalCoordinateToDirection(nearbySphericalStar.xy);
        nearbyStar = float4(nearbyStarDirection.x, nearbyStarDirection.y, nearbyStarDirection.z, nearbySphericalStar.z);
        
        if (distance(pos, nearbyStar) <= _StarLayer1MaxRadius) {
          float radius = GetStarRadius(nearbyStar.w, _StarLayer1MaxRadius, _StarLayer1TwinkleAmount);

  #ifdef STAR_LAYER_1_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, radius, nearbyStar.xyz);
          float2 pivot = float2(.5f, .5f);
    #if STAR_LAYER_1_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_StarLayer1SpriteItemCount, _StarLayer1SpriteAnimationSpeed, nearbyStar.w);
          float2 spriteItemSize = GetSpriteItemSize(_StarLayer1SpriteDimensions);

          texUV = GetSpriteSheetCoords(texUV, _StarLayer1SpriteDimensions, spriteFrameIndex, spriteItemSize, _StarLayer1SpriteItemCount);
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _StarLayer1SpriteDimensions, spriteItemSize);
    #endif
          texUV = AnimateStarRotation(texUV, _StarLayer1RotationSpeed * nearbyStar.w, 1, pivot);
          
          allStarColors += StarColorWithTexture(
            pos,
            starTextureUV,
            texUV,
            _StarLayer1Tex,
            _StarLayer1Color,
            _StarLayer1Density,
            radius,
            _StarLayer1TwinkleAmount,
            _StarLayer1TwinkleSpeed,
            _StarLayer1RotationSpeed,
            _StarLayer1EdgeFade,
            _StarLayer1DataTex,
            nearbyStar) * _StarLayer1HDRBoost;
            
  #else
          allStarColors += StarColorNoTexture(
            pos,
            _StarLayer1Color,
            _StarLayer1Density,
            radius,
            _StarLayer1TwinkleAmount,
            _StarLayer1TwinkleSpeed,
            _StarLayer1EdgeFade,
            _StarLayer1DataTex,
            nearbyStar) * _StarLayer1HDRBoost;
  #endif      
        }
#endif
        return allStarColors;
      }
      
      half4 FadeStarsColor(float verticalPosition, half4 starColor) {
        float fadeAmount = smoothstep(_StarFadeBegin, _StarFadeEnd, verticalPosition);
        return half4(starColor.xyz * fadeAmount, 1.0f);
      }

      v2f vert(appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.smoothVertex = v.vertex;

#ifdef CLOUDS
        
        // Apply the rotation for cloud direction.
      #if NOISE_CLOUDS
        float3 cloudWorldVertex = normalize(mul((float3x3)unity_ObjectToWorld, v.vertex.xyz));

        float computedHeight = lerp(_CLOUD_HEIGHT_LIMITS.x, _CLOUD_HEIGHT_LIMITS.y, 1 - _CloudHeight);
        cloudWorldVertex.y *= computedHeight * .2f;

        float cloudSpeed = _CloudSpeed * _Time;
        cloudWorldVertex.xz  = Rotate2d(cloudWorldVertex.xz, _CloudDirection);
        cloudWorldVertex = normalize(cloudWorldVertex);
        o.cloudUVs.xy = (cloudWorldVertex.xz * _CloudTextureTiling) + float2(cloudSpeed, cloudSpeed);
		    o.cloudUVs.zw = (cloudWorldVertex.xz * _CloudTextureTiling) + float2(cloudSpeed / 10.0f, cloudSpeed / 11.0f);
      #endif

#endif

        return o;
      }

      half4 OrbitBodyColorWithTextureUV(float3 pos, float3 orbitBodyPosition, 
          half4 orbitBodyTintColor, float orbitBodyRadius, float orbitBodyEdgeFade, sampler2D orbitBodyTex, float2 bodyUVs) {
        half4 color = tex2D(orbitBodyTex, bodyUVs) * orbitBodyTintColor;
        
        float fragDistance = distance(orbitBodyPosition, pos);

        float fadeEnd = orbitBodyRadius * (1 - orbitBodyEdgeFade);

        return smoothstep(orbitBodyRadius, fadeEnd, fragDistance) * color;
      }

      // Alpha premultiplied into color.
      half4 OrbitBodyColorNoTexture(float3 pos, float3 orbitBodyPosition, 
          half4 orbitBodyColor, float orbitBodyRadius, float orbitBodyEdgeFade) {
        half4 color = orbitBodyColor;
        
        float fragDistance = distance(orbitBodyPosition, pos);
        float fadeEnd = orbitBodyRadius * (1 - orbitBodyEdgeFade);

        return smoothstep(orbitBodyRadius, fadeEnd, fragDistance) * color;
      }

#if SUN
      half4 CalculateSunColor(float3 pos) {
        half4 sunColor = half4(0, 0, 0, 0);

        #ifdef SUN_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, _SunRadius, _SunPosition.xyz);
          float2 pivot = float2(.5f, .5f);

        #if SUN_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_SunSpriteItemCount, _SunSpriteAnimationSpeed, 0.0f);
          float2 spriteItemSize = GetSpriteItemSize(_SunSpriteDimensions.xy);

          texUV = GetSpriteSheetCoords(texUV, _SunSpriteDimensions, spriteFrameIndex, spriteItemSize, _SunSpriteItemCount);
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _SunSpriteDimensions, spriteItemSize);
        #endif

          #ifdef SUN_ROTATION
            texUV = AnimateStarRotation(texUV, _SunRotationSpeed, 1, pivot);
            sunColor = OrbitBodyColorWithTextureUV(pos,
              _SunPosition.xyz,
              _SunColor,
              _SunRadius,
              _SunEdgeFade,
              _SunTex,
              texUV) * _SunHDRBoost;
          #else
            sunColor = OrbitBodyColorWithTextureUV(pos,
              _SunPosition.xyz,
              _SunColor,
              _SunRadius,
              _SunEdgeFade,
              _SunTex,
              texUV) * _SunHDRBoost;
          #endif
        #else
        sunColor = OrbitBodyColorNoTexture(pos,
            _SunPosition.xyz,
            _SunColor,
            _SunRadius,
            _SunEdgeFade) * _SunHDRBoost;
        #endif

        return sunColor;
      }
#endif

#if MOON
      half4 CalculateMoonColor(float3 pos) {
        half4 moonColor = half4(0, 0, 0, 0);

        #ifdef MOON_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, _MoonRadius, _MoonPosition.xyz);
          float2 pivot = float2(.5f, .5f);

          #if MOON_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_MoonSpriteItemCount, _MoonSpriteAnimationSpeed, 0.0f);
          float2 spriteItemSize = GetSpriteItemSize(_MoonSpriteDimensions.xy);

          texUV = GetSpriteSheetCoords(texUV, _MoonSpriteDimensions, spriteFrameIndex, spriteItemSize, _MoonSpriteItemCount);  
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _MoonSpriteDimensions, spriteItemSize);
          #endif

          #ifdef MOON_ROTATION
          texUV = AnimateStarRotation(texUV, _MoonRotationSpeed, 1, pivot);

          moonColor = OrbitBodyColorWithTextureUV(pos,
            _MoonPosition.xyz,
            _MoonColor,
            _MoonRadius,
            _MoonEdgeFade,
            _MoonTex,
            texUV) * _MoonHDRBoost;
          #else
            moonColor = OrbitBodyColorWithTextureUV(pos,
              _MoonPosition.xyz,
              _MoonColor,
              _MoonRadius,
              _MoonEdgeFade,
              _MoonTex,
              texUV) * _MoonHDRBoost;
          #endif
        #else
        moonColor = OrbitBodyColorNoTexture(pos,
            _MoonPosition.xyz,
            _MoonColor,
            _MoonRadius,
            _MoonEdgeFade) * _MoonHDRBoost;
        #endif

        return moonColor;
      }
#endif

      half4 Calculate3WayGradientBackgroundAtPosition(float3 pos) {
        float2 sphereFragCoord = DirectionToSphericalCoordinate(pos);
        float verticalPosition = pos.y;

        // 3 way gradient.
        float middleGradientPosition = _GradientFadeBegin 
          + ((_GradientFadeEnd - _GradientFadeBegin) * _GradientFadeMiddlePosition);

        float4 lowerColor = _GradientSkyLowerColor;
        float4 middleColor = _GradientSkyMiddleColor;
        float4 upperColor = _GradientSkyUpperColor;

        float bottomColorPercent = smoothstep(_GradientFadeBegin, middleGradientPosition, verticalPosition);
        half4 bottomMixedColor = lerp(lowerColor, middleColor, bottomColorPercent);
        bottomMixedColor *= !step(middleGradientPosition, verticalPosition);

        float topColorPercent = smoothstep(middleGradientPosition, _GradientFadeEnd, verticalPosition);
        half4 topMixedColor = lerp(middleColor, upperColor, topColorPercent);
        topMixedColor *= step(middleGradientPosition, verticalPosition);

        return bottomMixedColor + topMixedColor;
      }

#ifdef CLOUDS

      //  Prototype - Ignore this - Clouds overhead with a custom texture.
      // half4 CalculateClouds(float4 cloudUVs, float3 vertexPos, float4 backgroundColor) {
			// 	half4 cloudColor = tex2D(_ArtCloudCustomTexture, cloudUVs.xy);
      //   half4 cloudColor2 = tex2D(_ArtCloudCustomTexture, cloudUVs.zw * .5f);
      //   half processedColor = pow(cloudColor, .9f);
      //   return backgroundColor + processedColor;
      // }

#if CUBEMAP_NORMAL_CLOUDS

      float4 SampleNormalCloudCubemap(float3 vertexPos, float rotationSpeed, float heightOffset, float rotationOffset, float3 shadowColor, float3 litColor) {
        float rotationAngle = (_Time.y * rotationSpeed) + rotationOffset;
        float3 rotatedDirection = RotateAroundYAxis(vertexPos, -rotationAngle);
        rotatedDirection.y += (-1.0f * heightOffset);

        float4 cloudColor = texCUBE(_CloudCubemapNormalTexture, rotatedDirection);
        float3 cloudFragWorldNormal = cloudColor.xyz * 2.0f - 1.0f;
        float3 cloudRotatedWorldNormal = RotateAroundYAxis(cloudFragWorldNormal, rotationAngle);

        float3 lightPosition = _CloudCubemapNormalToLight * 10.0f;
        float3 toLight = normalize(lightPosition - vertexPos.xyz);

        float lightDotProduct = dot(cloudRotatedWorldNormal, toLight);
        float lightPercent = (lightDotProduct + 1.0f) / 2.0f;

        float3 processedColor = lerp(shadowColor, litColor, saturate(_CloudCubemapNormalAmbientIntensity + lightPercent));

        return float4(processedColor, cloudColor.w);
      }

      // Clouds coming from a cubemap with normals.
      half4 RenderCubemapNormalClouds(float3 vertexPos, float4 backgroundColor)
      {        
        float4 cloudColor = SampleNormalCloudCubemap(vertexPos, _CloudCubemapNormalRotationSpeed, _CloudCubemapNormalHeight, 0,
          _CloudCubemapNormalShadowColor, _CloudCubemapNormalLitColor);
        
        #if CUBEMAP_NORMAL_CLOUD_DOUBLE_LAYER
          float4 cloudColor2 = SampleNormalCloudCubemap(vertexPos, _CloudCubemapNormalDoubleLayerRotationSpeed, 
            _CloudCubemapNormalDoubleLayerHeight, UNITY_HALF_PI, _CloudCubemapNormalDoubleShadowColor, _CloudCubemapNormalDoubleLitColor);

          float origAlpha = max(cloudColor.w, cloudColor2.w);
          cloudColor = saturate(AlphaBlend(cloudColor, cloudColor2));
          cloudColor.w = origAlpha;
        #endif

        return AlphaBlend(cloudColor, backgroundColor);
      }

#elif CUBEMAP_CLOUDS

      float4 SampleCloudCubemap(float3 vertexPos, float rotationSpeed, float heightOffset, float rotationOffset, samplerCUBE tex, float4 tintColor) {
        float rotationAngle = (_Time.y * rotationSpeed) + rotationOffset;
        float3 rotatedDirection = RotateAroundYAxis(vertexPos, -rotationAngle);
        rotatedDirection.y += (-1.0f * heightOffset);

        float4 color = texCUBE(tex, rotatedDirection) * tintColor;
        
        // TEST SOME NOISE
        /*
        float noiseRotationAngle = (_Time.y * _AtmosphereSpeed) + rotationOffset;
        float noiseRotationAngle2 = (sin(_Time.y * .5) * _AtmosphereSpeed) + 10.0f;
        float3 noise1 = texCUBE(_AtmosphereNoiseTex, RotateAroundYAxis(vertexPos, -noiseRotationAngle));
        float3 noise2 = texCUBE(_AtmosphereNoiseTex, RotateAroundYAxis(vertexPos, noiseRotationAngle2));

        color.w *= pow(noise1.r * noise2.r, 2);
        */

        return color;
      }

      // Clouds coming from a cubemap, no normals, unlit
      half4 RenderCubemapClouds(float3 vertexPos, float4 backgroundColor) {        
        float rotationAngle = _Time.y * _CloudCubemapRotationSpeed;
        float3 rotatedDirection = RotateAroundYAxis(vertexPos, -rotationAngle);
        rotatedDirection.y += (-1.0f * _CloudCubemapHeight);

        float4 cloudColor = SampleCloudCubemap(vertexPos, _CloudCubemapRotationSpeed, _CloudCubemapHeight,
          0, _CloudCubemapTexture, _CloudCubemapTintColor);
        
        #if CUBEMAP_CLOUD_FORMAT_RGB
          #if CUBEMAP_CLOUD_DOUBLE_LAYER
            
            #if CUBEMAP_CLOUD_DOUBLE_LAYER_CUSTOM_TEXTURE
            float4 cloudColor2 = SampleCloudCubemap(vertexPos, _CloudCubemapDoubleLayerRotationSpeed, _CloudCubemapDoubleLayerHeight,
              UNITY_HALF_PI, _CloudCubemapDoubleTexture, _CloudCubemapDoubleLayerTintColor);
            #else
            float4 cloudColor2 = SampleCloudCubemap(vertexPos, _CloudCubemapDoubleLayerRotationSpeed, _CloudCubemapDoubleLayerHeight,
              UNITY_HALF_PI, _CloudCubemapTexture, _CloudCubemapDoubleLayerTintColor);
            #endif // CUBEMAP_CLOUD_DOUBLE_LAYER_CUSTOM_TEXTURE

            return half4(cloudColor.xyz + cloudColor2.xyz + backgroundColor.xyz, 1.0f);
          #else // Else, no double layer.
            return half4(cloudColor.xyz + backgroundColor.xyz, 1.0f);
          #endif // CUBEMAP_CLOUD_DOUBLE_LAYER
        #else
          #if CUBEMAP_CLOUD_DOUBLE_LAYER
            #if CUBEMAP_CLOUD_DOUBLE_LAYER_CUSTOM_TEXTURE
            float4 cloudColor2 = SampleCloudCubemap(vertexPos, _CloudCubemapDoubleLayerRotationSpeed, _CloudCubemapDoubleLayerHeight,
              UNITY_HALF_PI, _CloudCubemapDoubleTexture, _CloudCubemapDoubleLayerTintColor);
            #else
            float4 cloudColor2 = SampleCloudCubemap(vertexPos, _CloudCubemapDoubleLayerRotationSpeed, _CloudCubemapDoubleLayerHeight,
              UNITY_HALF_PI, _CloudCubemapTexture, _CloudCubemapDoubleLayerTintColor);
            #endif

            float bestAlpha = max(cloudColor.w, cloudColor2.w);
            cloudColor = saturate(AlphaBlend(cloudColor, cloudColor2));
            cloudColor.w = bestAlpha;
          #endif
          
          return AlphaBlend(cloudColor, backgroundColor);
        #endif
      }

#elif NOISE_CLOUDS

      half4 RenderNoiseClouds(float4 cloudUVs, float3 vertexPos, float4 backgroundColor) {
				// Cloud noise.
				float4 tex1  = GetDataFromTexture(_CloudNoiseTexture, cloudUVs.xy);
        float4 tex2  = GetDataFromTexture(_CloudNoiseTexture, cloudUVs.zw);

        float noise1 = pow(tex1.g + tex2.g, 0.25f);
				float noise2 = pow(tex2.b * tex1.r, 0.5f);

        // Percent in the fadeout (0 means no fadeout, 1 means full fadeout - no clouds)
        float fadeOutPercent = smoothstep(_CloudFadePosition, 1, length(vertexPos.xz));

				_CloudColor1.rgb = pow(_CloudColor1.rgb, 2.2f);
        _CloudColor2.rgb = pow(_CloudColor2.rgb, 2.2f);

				float3 cloud1 = lerp(float3(0, 0, 0), _CloudColor2.rgb, noise1);
				float3 cloud2 = lerp(float3(0, 0, 0), _CloudColor1.rgb, noise2) * 1.5f;
				float3 cloud  = lerp(cloud1, cloud2, noise1 * noise2);

        // Cloud alpha.
        float outColorAlpha = 1.0f;
        float expandedDensity = _MAX_CLOUD_COVERAGE * (1.0f - _CloudDensity);
				float cloudAlpha = saturate(pow(noise1 * noise2, expandedDensity)) * pow(outColorAlpha, 0.35f);
        cloudAlpha *= 1.0f - fadeOutPercent * _CloudFadeAmount;
        cloudAlpha *= step(0.0f, vertexPos.y);
        
        float3 outColor = lerp(backgroundColor, cloud, cloudAlpha);

        return half4(outColor, 1.0f);
      }
#endif

#endif // CLOUDS

#ifdef RENDER_DEBUG_POINTS
      // Debug points are used for visualized spherical point keyframes in the editor only.
      half4 RenderDebugPoints(float3 pos) {
        half4 pointColor = half4(1, 0, 0, 1);
        half4 selectedPointColor = half4(0, 1, 0, 1);

        for (int i = 0; i < _DebugPointsCount; i++) {
          float4 debugPoint = _DebugPoints[i];
          float radius = debugPoint.w;
          if (distance(debugPoint.xyz, pos) <= _DebugPointRadius) {
            half4 color = pointColor;
            if (debugPoint.w > 0) {
              return selectedPointColor;
            } else {
              return pointColor;
            }
          }
        }

        return half4(0, 0, 0, 0);
      }
#endif

#ifdef HORIZON_FOG
      half4 ApplyHorizonFog(half4 skyColor, float3 vertexPos) {
        float fadePercent = smoothstep(1 - _HorizonFogLength, 1, length(vertexPos.xz));
        fadePercent *= _HorizonFogDensity;
        return lerp(skyColor, _HorizonFogColor, fadePercent);
      }
#endif

      half4 frag(v2f i) : SV_Target {
#ifdef GRADIENT_BACKGROUND
        half4 background = Calculate3WayGradientBackgroundAtPosition(i.smoothVertex);
#else
        half4 background = texCUBE(_MainTex, i.smoothVertex);
#endif

        float3 normalizedSmoothVertex = normalize(i.smoothVertex);
        int isMoonPixel = 0;
        int isSunPixel = 0;
        half4 sunColor = half4(0, 0, 0, 0);
        half4 moonColor = half4(0, 0, 0, 0);
#ifdef MOON
        // Check for moon at fragment.
        isMoonPixel = distance(i.smoothVertex, _MoonPosition.xyz) <= _MoonRadius;
        moonColor = CalculateMoonColor(normalizedSmoothVertex);
        moonColor *= isMoonPixel;
#endif

#ifdef SUN
        isSunPixel = distance(i.smoothVertex, _SunPosition.xyz) <= _SunRadius;
        sunColor = CalculateSunColor(normalizedSmoothVertex);
        sunColor *= isSunPixel;
#endif

        // Star color at current position.
        half4 starColor = StarColorFromAllGrids(normalize(i.smoothVertex));
        
        // Additive images shouldn't blend with stars since they may look transparent.
#if defined(MOON) && !defined(MOON_ALPHA_BLEND)
        starColor *= !isMoonPixel;
#endif

#if defined(SUN) && !defined(SUN_ALPHA_BLEND)
        starColor *= !isSunPixel;
#endif

        // Fade stars over the horizon.
        starColor = FadeStarsColor(i.smoothVertex.y, starColor);

#ifdef RENDER_DEBUG_POINTS
        half4 debugPointColor = RenderDebugPoints(normalize(i.smoothVertex.xyz));
        bool useDebugColor = step(.1f, length(debugPointColor));
        debugPointColor *= useDebugColor;
        background *= !useDebugColor;
        background = background + debugPointColor;
#endif

        // Merge the stars over the background color.
        half4 upperSkyColor = background + starColor;
        half4 finalColor = half4(0, 0, 0, 1);

#ifdef SUN_ALPHA_BLEND
          finalColor = AlphaBlend(sunColor, upperSkyColor);
#else
          finalColor = upperSkyColor + sunColor;
#endif

#ifdef MOON_ALPHA_BLEND
          finalColor = AlphaBlend(moonColor, finalColor);
#else
          finalColor += moonColor;
#endif

#ifdef CLOUDS
  #if NOISE_CLOUDS
        finalColor = RenderNoiseClouds(i.cloudUVs, i.smoothVertex, finalColor);
  #elif CUBEMAP_CLOUDS
        finalColor = RenderCubemapClouds(i.smoothVertex, finalColor);
  #elif CUBEMAP_NORMAL_CLOUDS
        finalColor = RenderCubemapNormalClouds(i.smoothVertex, finalColor);
  #endif

#endif

#ifdef HORIZON_FOG
        finalColor = ApplyHorizonFog(finalColor, i.smoothVertex);
#endif
        return finalColor;
      }
      ENDCG
    }
  }
  CustomEditor "DoNotModifyShaderEditor"
}
