Shader "Custom/ProgressBarShader"
{
    Properties
{
    _ColorStart ("Start Color", Color) = (0,1,0,1) // Green
    _ColorEnd ("End Color", Color) = (1,0,0,1)   // Red
    _Progress ("Progress", Range(0,1)) = 1.0      // Progress value (0 to 1)
    _MainTex ("Base Texture", 2D) = "white" { }
}
SubShader
{
    Tags { "RenderType"="Opaque" }
    Pass
    {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float4 color : COLOR;
            float2 uv : TEXCOORD0; // For texture coordinates
        };

        struct v2f
        {
            float4 pos : POSITION;
            float2 uv : TEXCOORD0;
        };

        // Declare properties
        float _Progress;
        float4 _ColorStart;
        float4 _ColorEnd;

        v2f vert(appdata v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;  // Pass UV coordinates through to fragment shader
            return o;
        }

        half4 frag(v2f i) : COLOR
        {
            // Convert from 0-1 UV to -1 to 1 range for circle positioning
            float2 uv = i.uv * 2.0 - 1.0;  // Map from [0, 1] to [-1, 1]

            // Calculate distance from the center (0,0)
            float dist = length(uv);

            // If the fragment is outside the circle, discard it
            if (dist > 0.5)
                discard;

            // Calculate the angle of the fragment from the center
            float angle = atan2(uv.y, uv.x) / 3.14159; // Angle from center in [-1, 1]

            // Normalize angle so it starts from 0 at the top and goes clockwise
            if (angle < 0.0)
                angle += 2.0; // Normalize angle to [0, 2]

            // Map the angle to the range [0, 1] for progress
            float normalizedAngle = angle / 2.0; // Normalize angle to [0, 1]

            // Interpolate the color from green to red based on the progress
            half4 color = lerp(_ColorStart, _ColorEnd, normalizedAngle);

            // Only draw if the angle is less than the current progress
            if (normalizedAngle > _Progress)
                discard;

            return color;
        }
        ENDCG
    }
}
FallBack "Diffuse"

}
