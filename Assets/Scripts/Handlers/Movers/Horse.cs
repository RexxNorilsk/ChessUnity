using UnityEngine;
using System.Collections.Generic;
using Interfaces;
using Entities.Base;

public class Horse : IChessMover {
    
    public IEnumerable<PositionOnGrid> AvaliableMoves(PositionOnGrid position) {
        List<PositionOnGrid> moves = new List<PositionOnGrid>();
        moves.Add(new PositionOnGrid(position.X + 1, position.Y + 2));
        moves.Add(new PositionOnGrid(position.X + 1, position.Y - 2));
        moves.Add(new PositionOnGrid(position.X - 1, position.Y + 2));
        moves.Add(new PositionOnGrid(position.X - 1, position.Y - 2));
        moves.Add(new PositionOnGrid(position.X + 2, position.Y + 1));
        moves.Add(new PositionOnGrid(position.X + 2, position.Y - 1));
        moves.Add(new PositionOnGrid(position.X - 2, position.Y + 1));
        moves.Add(new PositionOnGrid(position.X - 2, position.Y - 1));
        return moves;
    }
}