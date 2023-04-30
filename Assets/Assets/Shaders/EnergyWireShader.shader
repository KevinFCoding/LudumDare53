Shader "Feynallein/EnergyWireShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PulseEmissionCoeff("Pulse Emission Coeff", Range(1, 10)) = 2
        _BPM("BPM", float) = 0
        _WaveEmissionCoeff("Wave Emission Coeff", Range(1, 10)) = 2
        _WaveSpeed("Wave Speed", float) = 2
        _WaveRate("Wave Rate", float) = 2
        _Color("Color", Color) = (1, 1, 1, 1)
        _BeatToPulse("Beat to Pulse", float) = 1
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
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _PulseColor;
            float _PulseEmissionCoeff;
            float _WaveEmissionCoeff;
            float _WaveSpeed;
            float _WaveRate;
            float _BPM;
            float _BeatToPulse;
            fixed4 _Color;
            
            /// <summary>
            /// This function is a sawtooth-like made by myself. Using a sawtooth allows us to have this "tail" effect after each wave, as the lerp will have a decreasing value over the tail.
            /// We display the wave when this function returns more than 0, thefore we can have an infinite amout of waves at the same time
            /// </summary>
            float SmoothSawtooth(float2 x) {
                float t = _Time.y * _WaveSpeed - x.y * _WaveRate;
                return pow(cos(t - floor(t)), 5);
            }

            float pulse(float bpm, float beatToPulseTo) {
                return saturate(cos((_Time.y * UNITY_PI * bpm / (beatToPulseTo * 60)) % UNITY_PI));
            }

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {

                // Two lerps:
                // First lerp is between either the 2nd lerp or the "wave" depending on the function detailed in fun
                // The second lerp is for the pulse, based on the value from the pulse function (that returns a value between 0 and 1 
                fixed4 col = lerp(lerp(_Color, _Color * _PulseEmissionCoeff, pulse(_BPM, _BeatToPulse)), _Color * _WaveEmissionCoeff, SmoothSawtooth(i.uv));

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
