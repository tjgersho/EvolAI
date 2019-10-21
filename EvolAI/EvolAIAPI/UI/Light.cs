﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace GodAIAPI.UI
{
    class Light
    {
        public Light(Vector3 position, Vector3 color, float diffuseintensity = 1.0f, float ambientintensity = 1.0f)
        {
            Position = position;
            Color = color;

            DiffuseIntensity = diffuseintensity;
            AmbientIntensity = ambientintensity;

            Type = LightType.Point;
            Direction = new Vector3(0, 0, 1);
            ConeAngle = 15.0f;
        }
        public float LinearAttenuation;
        public float QuadraticAttenuation;

        public Vector3 Position;
        public Vector3 Color;
        public float DiffuseIntensity;
        public float AmbientIntensity;

        public LightType Type;
        public Vector3 Direction;
        public float ConeAngle;
    }

    enum LightType { Point, Spot, Directional }
}
