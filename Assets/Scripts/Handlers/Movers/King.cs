using UnityEngine;
using System.Collections.Generic;
using Interfaces;
using Entities.Base;

public class King : IChessMover {
    
    public IEnumerable<PositionOnGrid> AvaliableMoves(PositionOnGrid position) {
        List<PositionOnGrid> moves = new List<PositionOnGrid>();
        moves.Add(new PositionOnGrid(position.X + 1, position.Y));
        moves.Add(new PositionOnGrid(position.X - 1, position.Y));
        moves.Add(new PositionOnGrid(position.X, position.Y + 1));
        moves.Add(new PositionOnGrid(position.X, position.Y - 1));
        moves.Add(new PositionOnGrid(position.X + 1, position.Y + 1));
        moves.Add(new PositionOnGrid(position.X - 1, position.Y + 1));
        moves.Add(new PositionOnGrid(position.X + 1, position.Y - 1));
        moves.Add(new PositionOnGrid(position.X - 1, position.Y - 1));
        return moves;
    }
}