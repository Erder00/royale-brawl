namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicGatchaCommand : Command
    {
        public int BoxIndex;

        public LogicGatchaCommand() : base()
        {
            ;
        }

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            BoxIndex = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            if (BoxIndex == 3)
            {
                if (!homeMode.Avatar.UseDiamonds(80)) return -1;

                LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                DeliveryUnit unit = new DeliveryUnit(11);
                homeMode.SimulateGatcha(unit);
                command.DeliveryUnits.Add(unit);
                command.Execute(homeMode);

                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                homeMode.GameListener.SendMessage(message);
            }
            else if (BoxIndex == 1)
            {
                if (!homeMode.Avatar.UseDiamonds(30)) return -1;

                LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                DeliveryUnit unit = new DeliveryUnit(12);
                homeMode.SimulateGatcha(unit);
                command.DeliveryUnits.Add(unit);
                command.Execute(homeMode);

                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                homeMode.GameListener.SendMessage(message);
            }
            else if (BoxIndex == 4)
            {
                if (!homeMode.Avatar.UseStarTokens(10)) return -1;

                LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                DeliveryUnit unit = new DeliveryUnit(12);
                homeMode.SimulateGatcha(unit);
                command.DeliveryUnits.Add(unit);
                command.Execute(homeMode);

                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                homeMode.GameListener.SendMessage(message);
            }
            else if (BoxIndex == 5)
            {
                if (!homeMode.Avatar.UseTokens(100)) return -1;

                LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                DeliveryUnit unit = new DeliveryUnit(10);
                homeMode.SimulateGatcha(unit);
                command.DeliveryUnits.Add(unit);
                command.Execute(homeMode);

                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                homeMode.GameListener.SendMessage(message);
            }
            else
            {
                Debugger.Warning("Gatcha: Unknown box index: " + BoxIndex);
            }

            return 0;
        }

        public override int GetCommandType()
        {
            return 500;
        }
    }
}
