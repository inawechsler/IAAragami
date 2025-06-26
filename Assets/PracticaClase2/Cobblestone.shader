// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Cobblestone"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 32
		_TessMin( "Tess Min Distance", Float ) = 20
		_TessMax( "Tess Max Distance", Float ) = 32
		_TessPhongStrength( "Phong Tess Strength", Range( 0, 1 ) ) = 1
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_waterMetallic("waterMetallic", Float) = 0
		_waterSmoothness("waterSmoothness", Float) = 0
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_HMForce("HMForce", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction tessphong:_TessPhongStrength 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _HMForce;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample3;
		uniform float _waterMetallic;
		uniform float _waterSmoothness;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;
		uniform float _TessPhongStrength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 uv_TextureSample1 = v.texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 lerpResult31 = lerp( tex2Dlod( _TextureSample1, float4( uv_TextureSample1, 0, 0.0) ) , v.color , v.color);
			v.vertex.xyz += ( lerpResult31 * float4( float3(0,1,0) , 0.0 ) * _HMForce ).rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 lerpResult25 = lerp( tex2D( _TextureSample0, uv_TextureSample0 ) , tex2D( _TextureSample2, uv_TextureSample2 ) , i.vertexColor.r);
			float4 color33 = IsGammaSpace() ? float4(0,0.6636367,1,0) : float4(0,0.3979391,1,0);
			float2 temp_cast_0 = (0.1).xx;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float2 panner40 = ( 1.0 * _Time.y * temp_cast_0 + ase_vertex3Pos.xy);
			float4 lerpResult35 = lerp( lerpResult25 , ( color33 * tex2D( _TextureSample3, panner40 ) ) , i.vertexColor.b);
			o.Albedo = lerpResult35.rgb;
			o.Metallic = ( i.vertexColor.b * _waterMetallic );
			o.Smoothness = ( i.vertexColor.b * _waterSmoothness );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;511;1546;483;2319.473;697.5563;1.565725;True;True
Node;AmplifyShaderEditor.PosVertexDataNode;41;-1519.443,-568.5273;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-1388.521,-381.2844;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;40;-1153.805,-546.2957;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;33;-731.2186,-731.8062;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;0,0.6636367,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;24;-826.7009,73.8724;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;-835.1741,-119.5612;Inherit;True;Property;_TextureSample2;Texture Sample 2;9;0;Create;True;0;0;0;False;0;False;-1;None;b7f95f4f22e318d4e9234f1adb954c3c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-833.6296,-328.0387;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;c3b562978e16564429f5d179766acff3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-896.9566,574.6653;Inherit;True;Property;_TextureSample1;Texture Sample 1;10;0;Create;True;0;0;0;False;0;False;-1;None;0e209168a408d9f4e88dfa3e673ab22f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;32;-815.622,-542.7404;Inherit;True;Property;_TextureSample3;Texture Sample 3;6;0;Create;True;0;0;0;False;0;False;-1;None;b412308b9317f5943ae8f434a7e259e9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-398.8692,417.4075;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-420.3261,-635.4485;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;12;-822.0215,782.5291;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;36;-193.5143,179.9093;Inherit;False;Property;_waterMetallic;waterMetallic;7;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-274.1673,792.0251;Inherit;True;Property;_HMForce;HMForce;11;0;Create;True;0;0;0;False;0;False;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;123.6861,176.0091;Inherit;False;Property;_waterSmoothness;waterSmoothness;8;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;-311.077,-83.75604;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;118.486,127.9093;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-144.144,531.8683;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;35;-116.5374,-446.3144;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-201.3146,146.1094;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;361.4,-61.09999;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Cobblestone;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;32;20;32;True;1;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;0;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;41;0
WireConnection;40;2;42;0
WireConnection;32;1;40;0
WireConnection;31;0;7;0
WireConnection;31;1;24;0
WireConnection;31;2;24;0
WireConnection;34;0;33;0
WireConnection;34;1;32;0
WireConnection;25;0;1;0
WireConnection;25;1;23;0
WireConnection;25;2;24;1
WireConnection;38;0;24;3
WireConnection;38;1;39;0
WireConnection;27;0;31;0
WireConnection;27;1;12;0
WireConnection;27;2;14;0
WireConnection;35;0;25;0
WireConnection;35;1;34;0
WireConnection;35;2;24;3
WireConnection;37;0;24;3
WireConnection;37;1;36;0
WireConnection;0;0;35;0
WireConnection;0;3;37;0
WireConnection;0;4;38;0
WireConnection;0;11;27;0
ASEEND*/
//CHKSM=FEBB18E64834D6E4E3E8CC2A3C6AC5C47671A848