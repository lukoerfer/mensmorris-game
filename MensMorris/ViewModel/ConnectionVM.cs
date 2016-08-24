namespace MensMorris.Game.ViewModel
{
    public class ConnectionVM
    {
        private BoardVM Parent;

        public PositionVM FirstPosition { get; private set; }
        public PositionVM SecondPosition { get; private set; }

        public ConnectionVM(BoardVM parent, PositionVM pos1, PositionVM pos2)
        {
            this.Parent = parent;
            this.FirstPosition = pos1;
            this.SecondPosition = pos2;
        }


    }
}
