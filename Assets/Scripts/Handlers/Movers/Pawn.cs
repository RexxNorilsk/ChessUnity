using UnityEngine;
using System.Collections.Generic;
using Interfaces;
using Entities.Base;

public class Pawn : IChessMover {
    
    public IEnumerable<PositionOnGrid> AvaliableMoves(PositionOnGrid position) {
        List<PositionOnGrid> moves = new List<PositionOnGrid>();
        moves.Add(new PositionOnGrid(position.X, position.Y + 1));
        if(position.Y == 2) {
            moves.Add(new PositionOnGrid(position.X, position.Y + 2));
        }
        return moves;
    }
}