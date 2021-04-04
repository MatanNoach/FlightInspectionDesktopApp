using System;

namespace FlightInspectionDesktopApp.Player
{
    class PlayerModel
    {
        private static PlayerModel playerModelInst;
        private FGModelImp fgModel;
        private PlayerModel()
        {
        }
        public static PlayerModel Instance
        {
            get
            {
                if (playerModelInst == null)
                {
                    throw new Exception("PlayerModel was not created");
                }
                return playerModelInst;
            }
        }
        public void CreateModel()
        {
            if (playerModelInst != null)
            {
                throw new Exception("PlayerModel was already created");
            }
            playerModelInst = new PlayerModel();
        }
        public void Play()
        {
            dataModel
        }
    }
}
