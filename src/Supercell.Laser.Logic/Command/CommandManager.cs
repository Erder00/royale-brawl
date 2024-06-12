namespace Supercell.Laser.Logic.Command
{
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class CommandManager
    {
        private static Dictionary<int, Type> CommandTypes;

        static CommandManager()
        {
            CommandTypes = new Dictionary<int, Type>()
            {
                {500, typeof(LogicGatchaCommand)},
                {505, typeof(LogicSetPlayerThumbnailCommand)},
                {506, typeof(LogicSelectSkinCommand)},
                {515, typeof(LogicClearShopTickersCommand)},
                {517, typeof(LogicClaimRankUpRewardCommand)},
                {519, typeof(LogicPurchaseOfferCommand)},
                {520, typeof(LogicLevelUpCommand)},
                {521, typeof(LogicPurchaseHeroLvlUpMaterialCommand)},
                {525, typeof(LogicSelectCharacterCommand)},
                {533, typeof(LogicQuestsSeenCommand)},
                {534, typeof(LogicPurchaseBrawlPassCommand)},
                {535, typeof(LogicClaimTailRewardCommand)},
                {536, typeof(LogicPurchaseBrawlPassProgressCommand)}
            };
        }

        public static Command DecodeCommand(ByteStream stream)
        {
            int type = stream.ReadVInt();
            Command command = CommandManager.CreateCommand(type);
            if (command == null)
            {
                Debugger.Warning("Command is unhandled: " + type);
                return null;
            }

            command.Decode(stream);
            return command;
        }

        public static Command CreateCommand(int type)
        {
            if (CommandTypes.ContainsKey(type))
            {
                return (Command)Activator.CreateInstance(CommandTypes[type]);
            }
            return null;
        }
    }
}
