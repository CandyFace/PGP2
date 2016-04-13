Shader "Shaders/Volume/BasicFogVolumeMaterial" {
  Properties {
    _xy ("XY", 2D) = "white" {}
    _center ("Center", Vector) = (0, 0, 0, 0)
    _size ("Size", Float) = 0
    _seed ("Seed", Float) = 0
  }
  SubShader {
    Blend SrcAlpha OneMinusSrcAlpha
    Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
    CGPROGRAM

    #pragma surface surf Lambert alpha

    uniform float3 _center;
    uniform float _size;
    uniform float _seed;

        sampler2D _xy;

    struct Input {
        float3 worldPos;
    };

    float time() {
      return _seed + _Time[1] / 8;
    }

    float calc_offset_xy(float3 a) {
      return tex2D(
        _xy,
        float2(
          sin(time() + a[0] / _size),
          sin(time() + a[1] / _size))
        )[0];
    }

    float calc_offset_yz(float3 a) {
      return tex2D(
        _xy,
        float2(
          sin(time() + a[1] / _size),
          sin(time() + a[2] / _size))
        )[0];
    }

    float calc_offset_zx(float3 a) {
      return tex2D(
        _xy,
        float2(
          sin(time() + a[1] / _size),
          sin(time() + a[2] / _size))
        )[0];
    }

    // Calculate edge size
    float edge(float3 a) {
      return calc_offset_xy(a) + _size * calc_offset_zx(a);
    }

    // Distance between two points
    float distance(float3 a, float3 b) {
      float x = (a[0] - b[0]) * (a[0] - b[0]);
      float y = (a[1] - b[1]) * (a[1] - b[1]);
      float z = (a[2] - b[2]) * (a[2] - b[2]);
      return sqrt(x + y + z);
    }

    // Calculate the edge distance as a fractional value
    float edge_fraction(float3 a) {
      return 1.0 - distance(a, _center) / edge(a);
    }

    // Calculate the relative distance from the center point
    void surf (Input IN, inout SurfaceOutput o) {
      float alpha = edge_fraction(IN.worldPos);
      o.Albedo = fixed3(0, 0, 0);
      o.Alpha = 0;
      if (alpha > 0) {
        o.Albedo = float3(alpha, alpha, alpha);
        o.Alpha = alpha * 0.5;
      }
    }

    ENDCG
  }
  Fallback "Diffuse"
}