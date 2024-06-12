namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicClaimRankUpRewardCommand : Command
    {
        public int MilestoneId { get; set; }
        public int UnknownDataId { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);

            MilestoneId = stream.ReadVInt();
            UnknownDataId = ByteStreamHelper.ReadDataReference(stream);
            Unk2 = stream.ReadVInt();
            //Unk3 = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            Debugger.Print($"Claim rankup reward: milestone: {MilestoneId}, data: {UnknownDataId}, unk2: {Unk2}, unk3: {Unk3}");

            if (MilestoneId == 6)
            {
                string name = $"goal_6_{homeMode.Home.TrophyRoadProgress - 1}";


                MilestoneData milestoneData = DataTables.Get(DataType.Milestone).GetData<MilestoneData>(name);
                if (milestoneData == null)
                {
                    Debugger.Error($"Milestone data is NULL - {name}");
                    return -111;
                }

                if (homeMode.Avatar.HighestTrophies < milestoneData.Progress + milestoneData.ProgressStart)
                {
                    Debugger.Warning($"current progress: {homeMode.Avatar.HighestTrophies}, required progress: {milestoneData.Progress + milestoneData.ProgressStart}");
                    return -2;
                }

                if (ProcessReward(homeMode, milestoneData, false, 6, homeMode.Home.TrophyRoadProgress + 1) != 0) return -3;

                homeMode.Home.TrophyRoadProgress++;
            }
            else if (MilestoneId == 9 || MilestoneId == 10)
            {
                string name = $"Goal_{MilestoneId}_0_{(MilestoneId == 10 ? homeMode.Home.BrawlPassProgress : homeMode.Home.PremiumPassProgress) - 1}";
                if (MilestoneId == 9 && !homeMode.Home.HasPremiumPass)
                {
                    return -5;
                }

                MilestoneData milestoneData = DataTables.Get(DataType.Milestone).GetData<MilestoneData>(name);
                if (milestoneData == null)
                {
                    Debugger.Error($"Milestone data is NULL - {name}");
                    return -111;
                }

                if (homeMode.Home.BrawlPassTokens < milestoneData.Progress + milestoneData.ProgressStart)
                {
                    Debugger.Warning($"current progress: {homeMode.Home.BrawlPassTokens}, required progress: {milestoneData.Progress + milestoneData.ProgressStart}");
                    return -2;
                }

                if (ProcessReward(homeMode, milestoneData, false, MilestoneId, MilestoneId == 10 ? homeMode.Home.BrawlPassProgress + 1 : homeMode.Home.PremiumPassProgress + 1) != 0) return -3;

                if (MilestoneId == 9)
                    homeMode.Home.PremiumPassProgress++;
                else
                    homeMode.Home.BrawlPassProgress++;
            }
            else
            {
                return -1;
            }

            return 0;
        }

        private int ProcessReward(HomeMode homeMode, MilestoneData milestoneData, bool useSecondaryReward, int track, int idx)
        {
            int type = useSecondaryReward ? milestoneData.SecondaryLvlUpRewardType : milestoneData.PrimaryLvlUpRewardType;
            string data = !useSecondaryReward ? milestoneData.PrimaryLvlUpRewardData : milestoneData.SecondaryLvlUpRewardData;
            int count = useSecondaryReward ? milestoneData.SecondaryLvlUpRewardCount : milestoneData.PrimaryLvlUpRewardCount;
            switch (type)
            {
                case 1:
                    {
                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                        DeliveryUnit unit = new DeliveryUnit(100);

                        GatchaDrop drop = new GatchaDrop(7);
                        drop.Count = count;
                        unit.AddDrop(drop);

                        command.DeliveryUnits.Add(unit);
                        command.RewardTrackType = track;
                        command.RewardForRank = idx;
                        command.Execute(homeMode);

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = command;
                        homeMode.GameListener.SendMessage(message);

                        break;
                    }
                case 3:
                    {
                        CharacterData character = DataTables.Get(DataType.Character).GetData<CharacterData>(data);
                        if (homeMode.Avatar.HasHero(character.GetGlobalId()))
                        {
                            return ProcessReward(homeMode, milestoneData, true, track, idx);
                        }

                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                        DeliveryUnit unit = new DeliveryUnit(100);

                        GatchaDrop drop = new GatchaDrop(1);
                        drop.DataGlobalId = character.GetGlobalId();
                        drop.Count = 1;
                        unit.AddDrop(drop);

                        command.DeliveryUnits.Add(unit);
                        command.RewardTrackType = track;
                        command.RewardForRank = idx;
                        command.Execute(homeMode);

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = command;
                        homeMode.GameListener.SendMessage(message);

                        break;
                    }
                case 6:
                    {
                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                        DeliveryUnit unit = new DeliveryUnit(10);
                        homeMode.SimulateGatcha(unit);
                        command.DeliveryUnits.Add(unit);
                        command.RewardTrackType = track;
                        command.RewardForRank = idx;
                        command.Execute(homeMode);

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = command;
                        homeMode.GameListener.SendMessage(message);
                        break;
                    }
                case 9:
                    {
                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                        DeliveryUnit unit = new DeliveryUnit(10);
                        homeMode.SimulateGatcha(unit);
                        command.DeliveryUnits.Add(unit);
                        command.RewardTrackType = track;
                        command.RewardForRank = idx;
                        command.Execute(homeMode);

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = command;
                        homeMode.GameListener.SendMessage(message);
                        break;
                    }
                case 4:
                case 10:
                    {
                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                        DeliveryUnit unit = new DeliveryUnit(11);
                        homeMode.SimulateGatcha(unit);
                        command.DeliveryUnits.Add(unit);
                        command.RewardTrackType = track;
                        command.RewardForRank = idx;
                        command.Execute(homeMode);

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = command;
                        homeMode.GameListener.SendMessage(message);
                        break;
                    }
                case 12:
                    {
                        CharacterData character = DataTables.Get(DataType.Character).GetDataWithId<CharacterData>(UnknownDataId);

                        if (character == null)
                        {
                            return ProcessReward(homeMode, milestoneData, true, track, idx);
                        }

                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                        DeliveryUnit unit = new DeliveryUnit(100);

                        GatchaDrop drop = new GatchaDrop(6);
                        drop.DataGlobalId = character.GetGlobalId();
                        drop.Count = count;
                        unit.AddDrop(drop);

                        command.DeliveryUnits.Add(unit);
                        command.RewardTrackType = track;
                        command.RewardForRank = idx;
                        command.Execute(homeMode);

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = command;
                        homeMode.GameListener.SendMessage(message);

                        break;
                    }
                case 13:
                    break;
                case 14:
                    {
                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                        DeliveryUnit unit = new DeliveryUnit(12);
                        homeMode.SimulateGatcha(unit);
                        command.DeliveryUnits.Add(unit);
                        command.RewardTrackType = track;
                        command.RewardForRank = idx;
                        command.Execute(homeMode);

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = command;
                        homeMode.GameListener.SendMessage(message);
                        break;
                    }
                case 16:
                    {
                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                        DeliveryUnit unit = new DeliveryUnit(100);

                        GatchaDrop drop = new GatchaDrop(8);
                        drop.Count = count;
                        unit.AddDrop(drop);

                        command.DeliveryUnits.Add(unit);
                        command.RewardTrackType = track;
                        command.RewardForRank = idx;
                        command.Execute(homeMode);

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = command;
                        homeMode.GameListener.SendMessage(message);

                        break;
                    }
                case 18: // Quests unlocked!
                    {
                        homeMode.Home.Quests = new Quests();
                        homeMode.Home.Quests.AddRandomQuests(homeMode.Avatar.Heroes, 8);

                        LogicHeroWinQuestsChangedCommand cmd = new LogicHeroWinQuestsChangedCommand();
                        cmd.Quests = homeMode.Home.Quests;

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = cmd;
                        homeMode.GameListener.SendMessage(message);

                        break;
                    }
                case 19: // Pins
                    {
                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
                        DeliveryUnit unit = new DeliveryUnit(100);

                        GatchaDrop drop = new GatchaDrop(11);
                        drop.PinGlobalId = DataTables.Get(DataType.Emote).GetData<EmoteData>(data).GetGlobalId();
                        drop.Count = 1;
                        unit.AddDrop(drop);

                        command.DeliveryUnits.Add(unit);
                        command.RewardTrackType = track;
                        command.RewardForRank = idx;
                        command.Execute(homeMode);

                        AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                        message.Command = command;
                        homeMode.GameListener.SendMessage(message);
                    }
                    break;
                default:
                    {
                        Debugger.Error("Unknown reward type: " + type);
                        return -3;
                    }
            }

            return 0;
        }

        public override int GetCommandType()
        {
            return 517;
        }
    }
}
