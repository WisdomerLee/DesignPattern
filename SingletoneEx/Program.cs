using System;
using System.Collections.Generic;
//클래스 객체를 단 하나만 만들고 다른 곳곳에서 공용으로 쓰는 변수들을 접근할 수 있게 함
namespace CreationPattern.SingletoneExa
{
    #region 구조
    class SingletonExMain
    {
        static void Main()
        {
            Singleton s1 = Singleton.Instance();
            Singleton s2 = Singleton.Instance();

            if(s1 == s2)
            {
                Console.WriteLine("두 객체는 같은 객체입니다");

            }
            Console.ReadKey();
        }
    }
    class Singleton
    {
        static Singleton _instance;
        protected Singleton()
        {

        }
        public static Singleton Instance()
        {
            if(_instance == null)
            {
                _instance = new Singleton();
            }
            return _instance;
        }
    }

    #endregion
    #region 실제 예시
    class SingletonRealMain
    {
        static void Main()
        {
            LoadBalancer b1 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b2 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b3 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b4 = LoadBalancer.GetLoadBalancer();

            if(b1 == b2 && b2 ==b3 && b3 == b4)
            {
                Console.WriteLine("모두 같은 객체");
            }

            LoadBalancer balancer = LoadBalancer.GetLoadBalancer();
            for(int i =0; i<15; i++)
            {
                string server = balancer.Server;
                Console.WriteLine("Dispatch Request to :" + server);
            }

            Console.ReadKey();
        }
    }
    class LoadBalancer
    {
        static LoadBalancer _instance;
        List<string> _servers = new List<string>();
        Random _random = new Random();
        //동기화 자료 동시 접근 제한을 위한 처리
        static object syncLock = new object();

        protected LoadBalancer()
        {
            _servers.Add("ServerI");
            _servers.Add("ServerII");
            _servers.Add("ServerIII");
            _servers.Add("ServerIV");
            _servers.Add("ServerV");
        }

        public static LoadBalancer GetLoadBalancer()
        {
            if(_instance == null)
            {
                lock (syncLock)
                {
                    if(_instance == null)
                    {
                        _instance = new LoadBalancer();
                    }
                }
            }

            return _instance;
        }

        public string Server
        {
            get
            {
                int r = _random.Next(_servers.Count);
                return _servers[r].ToString();
            }
        }
    }
    #endregion
    #region 최적화된 코드

    class SingletonOptimizeMain
    {
        static void Main()
        {
            var b1 = LoadBalancerOpt.GetLoadBalancer();
            var b2 = LoadBalancerOpt.GetLoadBalancer();
            var b3 = LoadBalancerOpt.GetLoadBalancer();
            var b4 = LoadBalancerOpt.GetLoadBalancer();

            if(b1 == b2 && b2 == b3 && b3 == b4)
            {
                Console.WriteLine("같은 객체");
            }

            var balancer = LoadBalancerOpt.GetLoadBalancer();
            for(int i = 0; i<15; i++)
            {
                string serverName = balancer.NextServer.Name;
                Console.WriteLine("해당 서버에서 반응" + serverName);
            }

            Console.ReadKey();
        }
    }

    public sealed class LoadBalancerOpt
    {
        //static이 붙은 것들은 가장 먼저 초기화 되는 경향이 있음.. 클래스가 처음 올라가면 맨 처음에 초기화 되는 필드.. 스레드 접근성에서 안전함
        static readonly LoadBalancerOpt _instance = new LoadBalancerOpt();

        List<Server> _servers { get; set; }
        Random _random = new Random();

        LoadBalancerOpt()
        {
            _servers = new List<Server>
            {
                new Server{ Name = "ServerI", Ip = "120.14.220.18"},
                new Server{ Name = "ServerII", Ip = "120.14.220.19"},
                new Server{ Name = "ServerIII", Ip = "120.14.220.20"},
                new Server{ Name = "ServerIV", Ip = "120.14.220.21"},
                new Server{ Name = "ServerV", Ip = "120.14.220.22"},
            };
        }
        public static LoadBalancerOpt GetLoadBalancer()
        {
            return _instance;
        }
        public Server NextServer
        {
            get
            {
                int random = _random.Next(_servers.Count);
                return _servers[random];
            }
        }
    }

    public class Server
    {
        public string Name { get; set; }

        public string Ip { get; set; }

    }
    #endregion
    class Program
    {
        
    }
}
