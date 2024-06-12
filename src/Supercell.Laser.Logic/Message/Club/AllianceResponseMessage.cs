﻿namespace Supercell.Laser.Logic.Message.Club
{
    public class AllianceResponseMessage : GameMessage
    {
        public int ResponseType;

        public override void Encode()
        {
            Stream.WriteVInt(ResponseType);
        }

        public override int GetMessageType()
        {
            return 24333;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
