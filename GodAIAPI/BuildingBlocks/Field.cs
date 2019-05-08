using GodAIAPI.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace GodAIAPI.BuildingBlocks
{
    public abstract class Field : ISoup
    {
        /// <summary>
        /// Type of soup ingreadient
        /// </summary>
        public BuildingBlockType BlockType => BuildingBlockType.Field;

        /// <summary>
        /// Returns the universal field value for a UniversalPos
        /// </summary>
        /// <param name="upos">Universal Position parameter for the field. (X,Y,Z,t) </param>
        /// <returns></returns>
        public virtual double GetFieldValue(UniversalPosition upos)
        {
            return 1;
        }
    }
}
