// 位置を受け取り、物体までの距離を計算する関数
// 質点の距離関数 `length(p)` から半径を引くと、球の距離関数が定義できる

float Dist(float3 p)
{
    return length(p) - 0.1;
}

// 法線を計算する
float3 CalcNormal(float3 p)
{
    // 距離関数の勾配を取って正規化すると法線が計算できる
    float2 ep = float2(0, 0.001);
    return normalize(
        float3(
            Dist(p + ep.yxx) - Dist(p),
            Dist(p + ep.xyx) - Dist(p),
            Dist(p + ep.xxy) - Dist(p)
        )
    );
}

// マーチングループの本体
void RayMarching_float(
    float3 RayPosition,
    float3 RayDirection,
    float3 LightDir,
    out bool Hit,
    out float3 HitPosition,
    out float3 HitNormal,
    out float3 alpha)
{
    const float c_rayMaxDist = 10000.0f;

    //まずは普通のレイマーチング
    float3 pos = RayPosition;
    float rayHitDistance = 0;
    Hit = false;
    for (int i = 0; i < 15; ++i)
    {
        float d = Dist(pos);
        pos += d * RayDirection;
        rayHitDistance += d;

        if (d < 0.001)
        {
            Hit = true;
            HitPosition = pos;
            HitNormal = CalcNormal(pos);
            break;
        }
    }
    if (Hit == false)
    {
        rayHitDistance = c_rayMaxDist;
    }
    float3 defaultcolor = float3(1, 0, 0);
    if (Hit)
    {
        defaultcolor = dot(HitNormal, LightDir);
    }
    //ここからゴッドレイ計算
    float fogLitPercent = 0.0f;

    //影の中にいるか
    float isShadow = 0;
    for (int i = 0; i < 60; ++i)
    {
        float3 testpos = RayPosition + RayDirection * 100 * (i / 60);

        for (int k = 0; k < 64; ++k)
        {
            float d = length(testpos) - 2.0;
            testpos += d * LightDir;

            if (d < 0.001)
            {
                isShadow = 1;
                break;
            }
        }
        float inShadow = (isShadow == 1.0f) ? 1.0f : 0.0f;
        float progress = 1.0f / float(i + 1);
        // fogLitPercent = lerp(fogLitPercent, inShadow, progress);
        fogLitPercent += 0.01 * inShadow;
    }
    // float3 fogColor = lerp(float3(0, 0, 0), float3(1, 1, 1), fogLitPercent);
    // float absorb = exp(-rayHitDistance * 0.015);
    // float3 color = lerp(fogColor, defaultcolor, absorb);
    alpha = float3(fogLitPercent, fogLitPercent, fogLitPercent);
}
