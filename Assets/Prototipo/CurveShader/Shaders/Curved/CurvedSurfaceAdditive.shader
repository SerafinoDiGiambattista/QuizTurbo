Shader "Curved/CurvedSurfaceAdditive" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" { }     
    }

    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

        Blend SrcAlpha One

        Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		
        CGPROGRAM

        //#pragma exclude_renderers flash
        #pragma surface surf Lambert vertex:vert noforwardadd
		           
        sampler2D _MainTex;           

        struct Input {
            float2 uv_MainTex;
            float4 color : Color;
		};

		float3 _Curvature;
		
		void vert (inout appdata_full v, out Input o)
		{			
			UNITY_INITIALIZE_OUTPUT(Input,o);
            o.color = v.color;

			// for Curved
			float4 vPos = mul( unity_ObjectToWorld, v.vertex);

			float dist = vPos.z - _WorldSpaceCameraPos.z;
			float addY = dist * dist;
			vPos.y -= addY * _Curvature.y;

			dist = vPos.x - _WorldSpaceCameraPos.x;
			float addHY = dist * dist;
			vPos.y -= addHY* _Curvature.x ;

			// for corner
			vPos.x += addY * _Curvature.z;

			vPos = mul ( unity_WorldToObject, vPos);
			v.vertex = vPos;
		}	
        
		fixed4 _Color;
		
		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * IN.color.rgb;			
			o.Alpha = c.a * IN.color.a;
		}
		ENDCG
    }
    FallBack "Diffuse"
}