namespace Supercell.Laser.Logic.Message.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AnalyticEventMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 10110;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
