using System;
using System.Collections.Generic;
//생성 패턴 모음..//참고 : dofactory.com/net/에 있는 부분 참고: 진성준 개발자님을 통해 알게 된 사이트.. 몹시 유용함
//객체 생성과 조합을 캡슐화 하여 특정 객체가 생성, 변경되어도 프로그램 구조 자체에 영향을 크게 주지 않도록 유연성을 제공함
namespace CreationPattern
{
    //생성 패턴 중 하나..자주 쓰이는 패턴 중에 하나
    //많은 서브 클래스를 특정 그룹으로 묶어 한 번에 교체할 수 있도록 만든 것
    namespace AbstractFactory
    {
        #region 기본 틀
        /// <summary>
        /// AbstractFactory(뭍공장)
        ///  - 생성될  가상프로덕트의 행동 인터페이스를 들고 있는 곳
        /// ConcreteFactory(미국공장, 유럽공장)
        ///  - 콘크리트 제품 생성하는 동작이 정의된 곳
        /// AbstractProduct(초식동물, 육식동물)
        ///  - 제품타입이 정의된 인터페이스를 들고 있는 곳
        /// Product (야생몬스터, 사자, 늑대)
        ///  - 콘크리트 공장에서 만드는 제품, 제품 타입이 정의된 인터페이스를 상속
        /// Client(동물원)
        ///  - 가상공장, 가상제품 인터페이스를 활용하는 곳
        /// </summary>
        class AbstractFactoryMain
        {
            //콘솔 앱 진입점
            public static void Main()
            {
                //가상공장1
                AbstractFactory factory1 = new ConcreteFactory1();
                Client client1 = new Client(factory1);
                client1.Run();
                //가상공장2
                AbstractFactory factory2 = new ConcreteFactory2();
                Client client2 = new Client(factory2);
                client2.Run();

                Console.ReadKey();
            }
        }
        //가상
        abstract class AbstractFactory
        {
            public abstract AbstractProductA CreateProductA();
            public abstract AbstractProductB CreateProductB();
        }

        class ConcreteFactory1 : AbstractFactory
        {
            public override AbstractProductA CreateProductA()
            {
                return new ProductA1();
            }
            public override AbstractProductB CreateProductB()
            {
                return new ProductB1();
            }
        }

        class ConcreteFactory2 : AbstractFactory
        {
            public override AbstractProductA CreateProductA()
            {
                return new ProductA2();
            }

            public override AbstractProductB CreateProductB()
            {
                return new ProductB2();
            }
        }

        abstract class AbstractProductA
        {

        }

        abstract class AbstractProductB
        {
            public abstract void Interact(AbstractProductA a);
        }

        class ProductA1 : AbstractProductA
        {

        }
        class ProductB1 : AbstractProductB
        {
            public override void Interact(AbstractProductA a)
            {
                Console.WriteLine(this.GetType().Name + "Interacts with " + a.GetType().Name);
            }
        }
        class ProductA2 : AbstractProductA
        {

        }
        class ProductB2 : AbstractProductB
        {
            public override void Interact(AbstractProductA a)
            {
                Console.WriteLine(this.GetType().Name + "Interacts with " + a.GetType().Name);
            }
        }

        class Client
        {
            AbstractProductA _abstractProductA;
            AbstractProductB _abstractProductB;

            public Client(AbstractFactory factory)
            {
                _abstractProductA = factory.CreateProductA();
                _abstractProductB = factory.CreateProductB();
            }

            public void Run()
            {
                _abstractProductB.Interact(_abstractProductA);
            }
        }
        #endregion

        #region 실제 사용 예시
        class AbstractFactoryRealMain
        {
            //콘솔 앱 진입점
            public static void Main()
            {
                ContinentFactory europe = new EuropeFactory();
                AnimalWorld world = new AnimalWorld(europe);
                world.RunFoodChain();

                ContinentFactory america = new AmericaFactory();
                world = new AnimalWorld(america);
                world.RunFoodChain();

                Console.ReadKey();
            }

        }

        abstract class ContinentFactory
        {
            public abstract Herbivore CreateHerbivore();
            public abstract Carnivore CreateCarnivore();
        }

        class EuropeFactory : ContinentFactory
        {
            public override Herbivore CreateHerbivore()
            {
                return new WildBeast();
            }

            public override Carnivore CreateCarnivore()
            {
                return new Lion();
            }
        }

        class AmericaFactory : ContinentFactory
        {
            public override Carnivore CreateCarnivore()
            {
                return new Wolf();
            }

            public override Herbivore CreateHerbivore()
            {
                return new Bison();
            }
        }

        abstract class Herbivore
        {

        }

        abstract class Carnivore
        {
            public abstract void Eat(Herbivore h);
        }

        class WildBeast : Herbivore
        {

        }

        class Lion : Carnivore
        {
            public override void Eat(Herbivore h)
            {
                Console.WriteLine(this.GetType().Name + "eats" + h.GetType().Name);
            }
        }

        class Bison : Herbivore
        {

        }

        class Wolf : Carnivore
        {
            public override void Eat(Herbivore h)
            {
                Console.WriteLine(this.GetType().Name + "eats" + h.GetType().Name);
            }
        }

        class AnimalWorld
        {
            Herbivore _herbivore;
            Carnivore _carnivore;

            public AnimalWorld(ContinentFactory factory)
            {
                _carnivore = factory.CreateCarnivore();
                _herbivore = factory.CreateHerbivore();
            }

            public void RunFoodChain()
            {
                _carnivore.Eat(_herbivore);
            }
        }

        #endregion
    }

    //생성 패턴: 객체 재현과 객체 생성을 분리하는 방식, 객체 생성 방식은 동일하나 서로 다른 형태를 가진 객체를 만들어낼 수 있음
    namespace Builder
    {
        #region 구조
        /// <summary>
        /// Builder(VehicleBuilder) : 생산품 오브젝트의 추상 인터페이스 정의
        /// ConcreteBuilder(MotorCycleBuilder, CarBuilder, ScooterBuilder) : 빌더 인터페이스를 넣어 생성하는 어셈블리 파트, 생성된 것들을 추적함, 생산품 인터페이스 정의
        /// Director(shop) : Builder의 인터페이스로 오브젝트 생성
        /// Product(Vehicle) : ConcreteBuilderd가 만든 생산품을 내부 어셈블리에서 정의한 방식으로 표현, 정의함, 
        /// 
        /// </summary>

        public class BuilderMainStructure
        {
            public static void Main()
            {
                Director director = new Director();

                Builder b1 = new ConcreteBuilder1();
                Builder b2 = new ConcreteBuilder2();

                director.Construct(b1);
                Product p1 = b1.GetResult();
                p1.Show();

                director.Construct(b2);
                Product p2 = b2.GetResult();
                p2.Show();

                Console.ReadKey();
            }
        }
        class Director
        {
            public void Construct(Builder builder)
            {
                builder.BuildPartA();
                builder.BuildPartB();
            }
        }

        abstract class Builder
        {
            public abstract void BuildPartA();
            public abstract void BuildPartB();
            public abstract Product GetResult();
        }

        class ConcreteBuilder1 : Builder
        {
            Product _product = new Product();
            public override void BuildPartA()
            {
                _product.Add("PartA");
            }
            public override void BuildPartB()
            {
                _product.Add("PartB");
            }
            public override Product GetResult() => _product;
        }

        class ConcreteBuilder2 : Builder
        {
            Product _product = new Product();
            public override void BuildPartA()
            {
                _product.Add("PartX");
            }

            public override void BuildPartB()
            {
                _product.Add("PartY");
            }

            public override Product GetResult() => _product;
        }

        class Product
        {
            List<string> _parts = new List<string>();

            public void Add(string part)
            {
                _parts.Add(part);
            }

            public void Show()
            {
                Console.WriteLine("\nProduct Parts ---");
                foreach (string part in _parts)
                {
                    Console.WriteLine(part);
                }
            }
        }




        #endregion
        #region 실제 예시 중 하나
        public class BuilderMainRealE
        {
            public static void Main()
            {
                VehicleBuilder builder;

                Shop shop = new Shop();

                builder = new ScooterBuilder();
                shop.Construct(builder);
                builder.Vehicle.Show();

                builder = new CarBuilder();
                shop.Construct(builder);
                builder.Vehicle.Show();

                builder = new MotorCycleBuilder();
                shop.Construct(builder);
                builder.Vehicle.Show();

                Console.ReadKey();
            }
        }

        class Shop
        {
            public void Construct(VehicleBuilder vehicleBuilder)
            {
                vehicleBuilder.BuildFrame();
                vehicleBuilder.BuildEngine();
                vehicleBuilder.BuildWheels();
                vehicleBuilder.BuildDoors();
            }
        }

        abstract class VehicleBuilder
        {
            protected Vehicle vehicle;

            public Vehicle Vehicle
            {
                get
                {
                    return vehicle;
                }
            }

            public abstract void BuildFrame();
            public abstract void BuildEngine();
            public abstract void BuildWheels();
            public abstract void BuildDoors();

        }

        class MotorCycleBuilder : VehicleBuilder
        {
            public MotorCycleBuilder()
            {
                vehicle = new Vehicle("MotorCycle");
            }
            public override void BuildDoors()
            {
                vehicle["frame"] = "MotorCycle Frame";
            }

            public override void BuildEngine()
            {
                vehicle["engine"] = "500 cc";
            }

            public override void BuildFrame()
            {
                vehicle["wheels"] = "2";
            }

            public override void BuildWheels()
            {
                vehicle["doors"] = "0";
            }
        }


        class CarBuilder : VehicleBuilder
        {
            public CarBuilder()
            {
                vehicle = new Vehicle("Car");
            }

            public override void BuildDoors()
            {
                vehicle["doors"] = "4";
            }

            public override void BuildEngine()
            {
                vehicle["engine"] = "2500cc";
            }

            public override void BuildFrame()
            {
                vehicle["frame"] = "Car Frame";
            }

            public override void BuildWheels()
            {
                vehicle["wheels"] = "4";

            }
        }

        class ScooterBuilder : VehicleBuilder
        {
            public ScooterBuilder()
            {
                vehicle = new Vehicle("Scooter");
            }

            public override void BuildDoors()
            {
                vehicle["doors"] = "0";
            }

            public override void BuildEngine()
            {
                vehicle["engine"] = "50 cc";
            }

            public override void BuildFrame()
            {
                vehicle["frame"] = "Scooter Frame";
            }

            public override void BuildWheels()
            {
                vehicle["wheels"] = "2";
            }
        }

        class Vehicle
        {
            string _vehicleType;
            Dictionary<string, string> _parts = new Dictionary<string, string>();

            public Vehicle(string vehicleType)
            {
                this._vehicleType = vehicleType;
            }

            //Indexer
            public string this[string key]
            {
                get { return _parts[key]; }
                set { _parts[key] = value; }
            }

            public void Show()
            {
                Console.WriteLine("\n----------------");
                Console.WriteLine($"Vehicle Type: {_vehicleType}");
                Console.WriteLine($"Frame : {_parts["frame"]}");
                Console.WriteLine($"Engine : {_parts["engine"]}");
                Console.WriteLine($"Wheels : {_parts["wheels"]}");
                Console.WriteLine($"Doors : {_parts["doors"]}");
            }
        }

        #endregion
    }
    //객체 생성 처리를 서브 클래스로 분리하여 처리하도록 캡슐화하는 패턴
    //
    namespace FactoryMethod
    {
        #region 구조
        /// <summary>
        /// Product(Page): FactoryMethod로 만들어질 객체의 인터페이스 정의
        /// ConcreteProduct(SkillPage, EducationPage, ExperiencePage): 생성 객체 인터페이스 적용
        /// Creator(Document): 어느 타입의 생성 객체를 반환하는지를 결정하는 FactoryMethod정의, 기본 FactoryMethod를 정의하여 기본 객체 생성 방식을 지정할 수 있음
        /// ConcreteCreator(Report, Resume) : FactoryMethod를 덮어쓰고 실제 객체를 생성
        /// </summary>

        class FactoryMethodStructuralMain
        {
            static void Main()
            {

                Creator[] creators = new Creator[2];
                creators[0] = new ConcreteCreatorA();
                creators[1] = new ConcreteCreatorB();

                foreach (var creator in creators)
                {
                    Product product = creator.FactoryMethod();
                    Console.WriteLine($"{product.GetType().Name}생성됨 ");
                }

                Console.ReadKey();
            }
        }

        abstract class Product
        {

        }

        class ConcreteProductA : Product
        {

        }
        class ConcreteProductB : Product
        {

        }

        abstract class Creator
        {
            public abstract Product FactoryMethod();
        }

        class ConcreteCreatorA : Creator
        {
            public override Product FactoryMethod()
            {
                return new ConcreteProductA();
            }
        }

        class ConcreteCreatorB : Creator
        {
            public override Product FactoryMethod()
            {
                return new ConcreteProductB();
            }
        }


        #endregion
        #region 실제 예시
        class FactoryMethodRealMain
        {
            static void Main()
            {
                Document[] documents = new Document[2];

                documents[0] = new Resume();
                documents[1] = new Report();

                foreach (var document in documents)
                {
                    Console.WriteLine("\n" + document.GetType().Name + "---");
                    foreach (var page in document.Pages)
                    {
                        Console.WriteLine(" " + page.GetType().Name);
                    }
                }

                Console.ReadKey();
            }
        }

        abstract class Page
        {

        }

        class SkillsPage : Page
        {

        }

        class EducationPage : Page
        {

        }

        class ExperiencePage : Page
        {

        }
        class IntroductionPage : Page
        {

        }

        class ResultsPage : Page
        {

        }

        class ConclusionPage : Page
        {

        }
        class SummaryPage : Page
        {

        }

        class BibliographyPage : Page
        {

        }

        abstract class Document
        {
            List<Page> _pages = new List<Page>();

            public Document()
            {
                this.CreatePages();
            }

            public List<Page> Pages
            {
                get { return _pages; }
            }

            public abstract void CreatePages();
        }

        class Resume : Document
        {
            public override void CreatePages()
            {
                Pages.Add(new SkillsPage());
                Pages.Add(new EducationPage());
                Pages.Add(new ExperiencePage());
            }
        }

        class Report : Document
        {
            public override void CreatePages()
            {
                Pages.Add(new IntroductionPage());
                Pages.Add(new ResultsPage());
                Pages.Add(new ConclusionPage());
                Pages.Add(new SummaryPage());
                Pages.Add(new BibliographyPage());
            }
        }
        #endregion
    }
    //특정 종류의 오브젝트를 프로토타입 계통의 객체를 통해 만들고 이 프로토타입을 복사하는 형태로 새 객체를 만드는 방식
    namespace Prototype
    {
        #region 구조
        /// <summary>
        /// Prototype (ColorPrototype) : 자기자신을 복사하는 인터페이스 포함
        /// ConcretePrototype (Color) : 자기자신을 복사하는 인터페이스를 물려받아 함수로 제공
        /// Client (ColorManager) : 프로토타입에서 복제하는 함수를 이용하여 새 객체 생성
        /// 
        /// </summary>
        class ProtoTypeExMain
        {
            static void Main()
            {
                ConcretePrototype1 p1 = new ConcretePrototype1("1");
                ConcretePrototype1 c1 = (ConcretePrototype1)p1.Clone();
                Console.WriteLine($"복제: {c1.Id}");

                ConcretePrototype2 p2 = new ConcretePrototype2("2");
                ConcretePrototype2 c2 = (ConcretePrototype2)p2.Clone();
                Console.WriteLine($"복제: {c2.Id}");

                Console.ReadKey();
            }
        }

        abstract class Prototype
        {
            string _id;

            public Prototype(string id)
            {
                this._id = id;
            }

            public string Id
            {
                get { return _id; }
            }

            public abstract Prototype Clone();
        }

        class ConcretePrototype1 : Prototype
        {
            public ConcretePrototype1(string id) : base(id)
            {

            }

            public override Prototype Clone()
            {
                return (Prototype)this.MemberwiseClone();
            }
        }

        class ConcretePrototype2 : Prototype
        {
            public ConcretePrototype2(string id) : base(id)
            {

            }

            public override Prototype Clone()
            {
                return (Prototype)this.MemberwiseClone();
            }

        }

        #endregion
        #region 실제 예시

        class PrototypeRealExMain
        {
            static void Main()
            {
                ColorManager colorManager = new ColorManager();

                colorManager["red"] = new Color(255, 0, 0);
                colorManager["green"] = new Color(0, 255, 0);
                colorManager["blue"] = new Color(0, 0, 255);

                colorManager["angry"] = new Color(255, 54, 0);
                colorManager["peace"] = new Color(128, 211, 128);
                colorManager["flame"] = new Color(211, 34, 20);

                Color color1 = colorManager["red"].Clone() as Color;
                Color color2 = colorManager["peace"].Clone() as Color;
                Color color3 = colorManager["flame"].Clone() as Color;

                Console.ReadKey();
            }
        }


        abstract class ColorPrototype
        {
            public abstract ColorPrototype Clone();

        }

        class Color : ColorPrototype
        {
            int _red;
            int _green;
            int _blue;

            public Color(int red, int green, int blue)
            {
                this._red = red;
                this._green = green;
                this._blue = blue;
            }

            public override ColorPrototype Clone()
            {
                Console.WriteLine($"빛깔 복사 {_red,3},{_green,3},{_blue,3}");
                return this.MemberwiseClone() as ColorPrototype;
            }
        }

        class ColorManager
        {
            Dictionary<string, ColorPrototype> _colors = new Dictionary<string, ColorPrototype>();

            public ColorPrototype this[string key]
            {
                get { return _colors[key]; }
                set { _colors.Add(key, value); }
            }
        }



        #endregion
    }
    //전역변수를 사용하지 않고 객체 자체를 하나만 생성하도록 하여 생성된 객체는 어디서든 참조가능하도록 하는 것
    namespace Singleton
    {
        #region 구조
        class SingletonExMain
        {
            static void Main()
            {
                Singleton s1 = Singleton.Instance();
                Singleton s2 = Singleton.Instance();

                if (s1 == s2)
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
                if (_instance == null)
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

                if (b1 == b2 && b2 == b3 && b3 == b4)
                {
                    Console.WriteLine("모두 같은 객체");
                }

                LoadBalancer balancer = LoadBalancer.GetLoadBalancer();
                for (int i = 0; i < 15; i++)
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
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
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

                if (b1 == b2 && b2 == b3 && b3 == b4)
                {
                    Console.WriteLine("같은 객체");
                }

                var balancer = LoadBalancerOpt.GetLoadBalancer();
                for (int i = 0; i < 15; i++)
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
    }
}
