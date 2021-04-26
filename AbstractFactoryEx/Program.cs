using System;
//생성 패턴 중 하나..자주 쓰이는 패턴 중에 하나
//생성할 때 관련있거나 상속받은 클래스에 해당하는 객체를 만들 때 특정 클래스에 속하지 않는 인퍼테이스를 제공하는 형식

namespace CreationPattern.AbstractFactoryEx
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

    class Program
    {

        static void Main(string[] args)
        {

        }
    }
}
