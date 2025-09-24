using System.Collections.Generic;
using Assets.Scripts.Entities.Base;

public interface IChessMover
{
    IEnumerable<PositionOnGrid> AvaliableMoves(PositionOnGrid position);
}


