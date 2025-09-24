using System.Collections.Generic;
using Entities.Base;

namespace Interfaces {
    public interface IChessMover
    {
        IEnumerable<PositionOnGrid> AvaliableMoves(PositionOnGrid position);
    }
}


