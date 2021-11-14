namespace UnityGame
{
    public interface IGameMessage
    {

    }

    public class GamePausedMessage : IGameMessage
    {

    }

    public class GameUnpausedMessage : IGameMessage
    {

    }

    public class GameStartedMessage : IGameMessage
    {

    }

    public class GameDefeatMessage : IGameMessage
    {

    }

    public class BallDestroyedMessage : IGameMessage
    {
        public bool destroyedByUser;
        public BallModel model;
        public int instanceId;

        public BallDestroyedMessage(BallModel model, bool destroyedByUser, int instanceId)
        {
            this.model = model;
            this.destroyedByUser = destroyedByUser;
            this.instanceId = instanceId;
        }
    }
}

