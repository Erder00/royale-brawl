namespace Supercell.Laser.Logic.Home.Quest
{
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Helper;

    public class Quest
    {
        public int MissionType { get; set; }
        public int CurrentGoal { get; set; }
        public int QuestGoal { get; set; }
        public int GameModeVariation { get; set; }
        public bool QuestSeen { get; set; }
        public int CharacterId { get; set; }
        public int Reward { get; set; }
        public int Progress { get; set; }

        public Quest Clone()
        {
            return new Quest()
            {
                MissionType = MissionType,
                CurrentGoal = CurrentGoal,
                QuestGoal = QuestGoal,
                GameModeVariation = GameModeVariation,
                QuestSeen = QuestSeen,
                CharacterId = CharacterId,
                Reward = Reward,
                Progress = Progress
            };
        }

        public void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteVInt(0);  // Unknown
            encoder.WriteVInt(0);  // Unknown
            encoder.WriteVInt(MissionType);  // Mission Type

            encoder.WriteVInt(CurrentGoal);  // Achieved Goal
            encoder.WriteVInt(QuestGoal);  // Quest Goal

            encoder.WriteVInt(Reward); // Tokens Reward

            encoder.WriteVInt(0);  // Unknown

            encoder.WriteVInt(0);  // Current level
            encoder.WriteVInt(0);  // Max level

            encoder.WriteVInt(-1);  // Quest Timer

            encoder.WriteBoolean(false); // unknown
            encoder.WriteBoolean(QuestSeen); // quest seen

            ByteStreamHelper.WriteDataReference(encoder, CharacterId);

            encoder.WriteVInt(GameModeVariation);  // Game mode variation
            encoder.WriteVInt(Progress);  // Progress
        }
    }
}
