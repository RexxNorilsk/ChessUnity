using UnityEngine;
using System.Collections.Generic;
using Interfaces;
using Entities.Base;

public class Queen : IChessMover {
    
    public IEnumerable<PositionOnGrid> AvaliableMoves(PositionOnGrid position) {
        List<PositionOnGrid> moves = new List<PositionOnGrid>();
        for(int i = 1; i < 8; i++) {
            moves.Add(new PositionOnGrid(position.X + i, position.Y + i));
            moves.Add(new PositionOnGrid(position.X + i, position.Y - i));
            moves.Add(new PositionOnGrid(position.X - i, position.Y + i));
            moves.Add(new PositionOnGrid(position.X - i, position.Y - i));
            moves.Add(new PositionOnGrid(position.X + i, position.Y));
            moves.Add(new PositionOnGrid(position.X - i, position.Y));
            moves.Add(new PositionOnGrid(position.X, position.Y + i));
            moves.Add(new PositionOnGrid(position.X, position.Y - i));
        }
        return moves;
    }
}