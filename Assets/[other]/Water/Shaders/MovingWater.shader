Shader "Custom/MovingWater"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _WaveSourcePosition("Wave Source Position", Vector) = (0, 0, 0)
        _WaveFrequency("Wave Frequency", Float) = 0.53
        _WaveHeight("Wave Height", Float) = 0.48
        _WaveLength("Wave Length", Float) = 0.71
        _InitialHeigth("Initial Heigth", Float) = 0.71
        _FunctionRange("Noise Function Range", Float) = 0.71
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        #pragma vertex vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        float3 _WaveSourcePosition;
        float _WaveFrequency;
        float _WaveHeight;
        float _WaveLength;
        float _InitialHeigth;
        float _FunctionRange;

        struct Input
        {
            bool moved;
            float4 worldPosition;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;


        void vert(inout appdata_full v, out Input o) {
            if (v.vertex.y > 0.25f)
            {
                float4 modifiedPos = v.vertex;
                modifiedPos.y = 0;
                float dist = distance(modifiedPos, _WaveSourcePosition);
                dist = (dist % _WaveLength) / _WaveLength;
                modifiedPos.y = _WaveHeight * sin(_Time * 3.14f * 2.0f * _WaveFrequency
                    + (3.14f * 2.0f * dist));
                modifiedPos.y += 1.0f + _InitialHeigth * (sin(_FunctionRange * (-modifiedPos.x + modifiedPos.y)) +
                    cos(_FunctionRange * 0.5f * modifiedPos.y));

                v.vertex = modifiedPos;
                o.moved = true;
            }
            o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Normal = normalize(cross(ddy(IN.worldPosition), ddx(IN.worldPosition)));
            
            // Albedo comes from a texture tinted by color
            o.Albedo = _Color.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
