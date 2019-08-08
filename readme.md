# JungleWar
一个双人实时第三人称射击游戏，客户端使用unity开发，服务端使用C#和net core开发

# 运行截图
![](https://s2.ax1x.com/2019/08/08/e7Ylt0.png)
WASD控制方向，鼠标左键射击🏹

# 开始使用

## 启动服务器

```bash
cd ./JungleWarServer/bin/Debug/netcoreapp2.1/
dotnet ./JungleWarServer.dll
```

## 启动客户端
1.使用unity打开JungleWarClient工程目录，修改Assets/Scripts/NetFrame/Net.cs文件，代码如下
```csharp
public class Net : MonoBehaviour
{
    private const string IP = "127.0.0.1";
    private const int PORT = 8899;
    private const int BUFFER_SIZE = 2048;
    ...
```
将Net类中的IP地址修改为启动服务器的IP地址，回到unity点击运行或Build后双击exe运行即可


## 注意❗

由于在实现该项目时，net core没有找到连接MySQL数据库的官方dll，故服务器并没有做连接数据库的处理，账号都是预设在内存中的，可以登录使用的账号有如下三个：  
账号：123 密码：123  
账号：111 密码：111  
账号：222 密码：222  

若想添加自定义账号，可在JungleWarServer\DataBase\DB.cs文件中修改，案例如下
```csharp
public class DB
    {
        // 存储已登录的客户端对应的登录用户名
        // 在登录的同时添加 下线的同时删除
        private static Dictionary<UserToken, string> token_2_username = new Dictionary<UserToken, string>();
        // 存储所有用户信息
        private static Dictionary<string, UserInfo> username_2_info = new Dictionary<string, UserInfo>();

        static DB()
        {
            UserInfo info = new UserInfo("123", "123");
            username_2_info.Add("123", info);
            UserInfo info2 = new UserInfo("111", "111");
            username_2_info.Add("111", info2);
            UserInfo info3 = new UserInfo("222", "222");
            username_2_info.Add("222", info3);
        }

        ...
```