namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicClaimTailRewardCommand : Command
    {
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);

            stream.ReadVInt();
            stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            if (homeMode.Home.BrawlPassTokens < (33450 + 500)) return -1;

            homeMode.Home.BrawlPassTokens -= 500;
            LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
            DeliveryUnit unit = new DeliveryUnit(12);
            homeMode.SimulateGatcha(unit);
            command.DeliveryUnits.Add(unit);
            command.Execute(homeMode);

            AvailableServerCommandMessage message = new AvailableServerCommandMessage();
            message.Command = command;
            homeMode.GameListener.SendMessage(message);

            return 0;
        }

        public override int GetCommandType()
        {
            return 535;
        }
    }
}
