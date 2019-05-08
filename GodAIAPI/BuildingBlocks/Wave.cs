using GodAIAPI.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace GodAIAPI.BuildingBlocks
{
    public abstract class Wave : ISoup
    {
        /// <summary>
        /// Type of soup ingreadient
        /// </summary>
        public BuildingBlockType BlockType => BuildingBlockType.Wave;

        public WaveProps waveProps { get; set; }

    }
     
}
