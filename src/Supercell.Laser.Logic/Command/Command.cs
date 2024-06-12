namespace Supercell.Laser.Logic.Command
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public abstract class Command
    {
        public virtual void Encode(ByteStream stream)
        {
            stream.WriteVInt(1);

            stream.WriteVInt(1);

            stream.WriteVInt(0);
            stream.WriteVInt(0);
        }

        public virtual void Decode(ByteStream stream)
        {
            stream.ReadVInt();

            stream.ReadVInt();

            stream.ReadVInt();
            stream.ReadVInt();
        }

        public virtual bool CanExecute(HomeMode homeMode)
        {
            return true;
        }

        public abstract int Execute(HomeMode homeMode);
        public abstract int GetCommandType();
    }
}
