using System;
using System.ComponentModel;

namespace BiM.Game.World
{
    public class WorldMap : INotifyPropertyChanged
    {
        public SuperArea GetSuperArea(int id)
        {
            throw new NotImplementedException();
        }

        public Area GetArea(int id)
        {
            throw new NotImplementedException();
        }

        public SubArea GetSubArea(int id)
        {
            throw new NotImplementedException();
        }

        public Map GetMap(int id)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}