namespace QuoridorDelta.Model
{
    public class Pawn
    {
        public Coords Coords { get; set; }

        public Pawn(Coords coords) => Coords = coords;
    };
}