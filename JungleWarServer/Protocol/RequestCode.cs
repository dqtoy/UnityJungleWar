namespace JungleWarServer.Protocol
{
    /// <summary>
    /// 每个RequestCode表示一个Handler
    /// </summary>
    public enum RequestCode
    {
        NONE,
        USER,
        ROOM,
        GAME
    }
}