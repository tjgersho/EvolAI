using System;
using System.Collections.Generic;
using System.Text;
using GodAIAPI.Descriptors;
using GodAIAPI.UI;
using OpenTK;

namespace GodAIAPI.BuildingBlocks
{
    public abstract class Particle : SimpleSphere, ISoup //ColorCube, ISoup
    {
        public Guid Guid = Guid.NewGuid();
        /// <summary>
        /// Type of soup ingreadient
        /// </summary>
        public BuildingBlockType BlockType => BuildingBlockType.Particle;

        private UniversalPosition uPos = new UniversalPosition();

        /// <summary>
        /// Particle position (X,Y,Z,t)
        /// </summary>
        public UniversalPosition GetUPos()
        {
            return uPos;
        }

        /// <summary>
        /// Particle position (X,Y,Z,t)
        /// </summary>
        public void SetUPos(UniversalPosition value)
        {
            Position.X = (float)value.X;
            Position.Y = (float)value.Y;
            Position.Z = (float)value.Z;

            uPos.X = value.X;
            uPos.Y = value.Y;
            uPos.Z = value.Z;
        }
        public UniversalVelocity UVel { get; set; } = new UniversalVelocity();
        public UniversalAcceleration UAccel { get; set; } = new UniversalAcceleration();


        //public override Vector3[] GetColorData()
        //{
        //    return new Vector3[] {
        //        new Vector3( 0.5f, 0.5f, 1f),
        //        new Vector3( 0.5f, 0.5f, 1f),
        //        new Vector3( 0.5f, 0.5f, 1f),
        //        new Vector3( 0.5f, 0.5f, 1f),
        //        new Vector3( 0.5f, 0.5f, 1f),
        //        new Vector3( 0.5f, 0.5f, 1f),
        //        new Vector3( 0.5f, 0.5f, 1f),
        //        new Vector3( 0.5f, 0.5f, 1f)
        //    };
        //}

    }
}
