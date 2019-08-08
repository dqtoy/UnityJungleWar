namespace JungleWarServer.DataBase
{
    public class UserInfo
    {
        public string username;
        public string password;
        public int totalcount;
        public int wincount;

        public UserInfo(string username, string password)
        {
            this.username = username;
            this.password = password;
            totalcount = 0; wincount = 0;
        }
    }
}