namespace MensMorris.Game.ViewModel
{
    /// <summary>
    /// View model for the connection between two board positions
    /// </summary>
    public class ConnectionVM
    {
        /// <summary>
        /// Gets the board position at the start
        /// </summary>
        public PositionVM FirstPosition { get; private set; }
        /// <summary>
        /// Gets board position at the end
        /// </summary>
        public PositionVM SecondPosition { get; private set; }

        /// <summary>
        /// Creates a new view model for the connection between two board positions
        /// </summary>
        /// <param name="pos1">The position at the start</param>
        /// <param name="pos2">The position at the end</param>
        public ConnectionVM(PositionVM pos1, PositionVM pos2)
        {
            this.FirstPosition = pos1;
            this.SecondPosition = pos2;
        }


    }
}
