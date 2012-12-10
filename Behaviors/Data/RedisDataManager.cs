#region License GNU GPL
// RedisDataManager.cs
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

using BiM.Core.Reflection;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace BiM.Behaviors.Data
{
    public abstract class RedisDataManager<T> : Singleton<T> 
        where T : class
    {
        private readonly PooledRedisClientManager m_clientManager = new PooledRedisClientManager("localhost");

        protected PooledRedisClientManager ClientManager
        {
            get { return m_clientManager; }
        }

        protected IRedisClient GetClient()
        {
            return m_clientManager.GetClient();
        }
    }
}