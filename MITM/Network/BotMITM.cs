#region License GNU GPL
// BotMITM.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using BiM.Behaviors;

namespace BiM.MITM.Network
{
    public class BotMITM : Bot
    {
        public BotMITM(ConnectionMITM connection, NetworkMessageDispatcher dispatcher)
             : base (dispatcher)
        {
            Connection = connection;   
        }

        public ConnectionMITM Connection
        {
            get;
            private set;
        }

        public bool ExpectedDisconnection
        {
            get;
            set;
        }

        public bool Identified
        {
            get;
            set;
        }

        public void ChangeConnection(ConnectionMITM connection)
        {
            Connection = connection;
        }

        public override void Stop()
        {
            if (!Running)
                return;

            if (Connection != null)
                Connection.Disconnect();

            base.Stop();
        }

        public override void Dispose()
        {
            if (Connection != null)
                Connection.Disconnect();

            base.Dispose();
        }
    }
}