Shader "Custom/GlowCustom"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range(0, 1)) = 0.5
        _GlowThickness ("Glow Thickness", Range(0, 10)) = 1
        _FanAngle ("Fan Angle", Range(0, 360)) = 90
        _RotationSpeed ("Rotation Speed", Range(0.01, 5)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 200

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off

            HLSLPROGRAM
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
                float fanEffect : TEXCOORD1;
                float angleDiff : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _GlowColor;
            float _GlowIntensity;
            float _GlowThickness;
            float _FanAngle;
            float _RotationSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // UV 좌표 변환
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
                float2 center = float2(0.5, 0.5); // 텍스처의 중심점

                // 중심을 기준으로 한 UV 좌표 계산
                float2 delta = uv - center;

                // 회전 각도 계산
                float time = _Time.y * _RotationSpeed;
                float angleOffset = time * 360.0;
                float angle = atan2(delta.y, delta.x) * 180.0 / 3.14159;
                float fanAngle = angle - angleOffset;

                // 각도를 0~360 범위로 변환
                fanAngle = fmod(fanAngle + 360.0, 360.0);

                // 부채꼴의 범위 내에 있는지 계산
                float angleDiff = abs(fanAngle) - (_FanAngle * 0.5);
                float withinFan = step(angleDiff, _GlowThickness);

                // 부채꼴 영역만 글로우 효과가 적용되도록 설정
                o.uv = uv;
                o.fanEffect = withinFan;
                o.angleDiff = angleDiff;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texcol = tex2D(_MainTex, i.uv);

                // 글로우 효과 적용
                float glow = smoothstep(0.0, 1.0, 1.0 - abs(i.angleDiff / (_FanAngle / 2.0)));
                glow *= _GlowIntensity; // 부채꼴 내에서만 글로우 적용

                fixed4 glowColor = _GlowColor * glow;
                return texcol + glowColor;
            }
            ENDHLSL
        }
    }
}