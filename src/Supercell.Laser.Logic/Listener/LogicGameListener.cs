namespace Supercell.Laser.Logic.Listener
{
    using Supercell.Laser.Logic.Message;

    public abstract class LogicGameListener
    {
        public int HandledInputs;

        public abstract void SendMessage(GameMessage message);
        public abstract void SendTCPMessage(GameMessage message);
    }
}
