Shader "Unlit/color_shader"
{
	Properties
	{
		_Color("Color", color) = (1,1,1,1)
	}
		SubShader
	{
		Tags
		{
			"RenderType" = "Overlay"

		}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			float4 _Color;

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
				float4 color = _Color;
                return color;
            }
            ENDCG
        }
    }
}
