using System;
using System.Collections.Generic;
//구조 패턴들 모음
//클래스나 객체를 조합하여 더 큰 구조를 만드는 패턴
//서로 다른 인터페이스를 지닌 2개의 객체를 묶어 단일 인터페이스로 제공하거나 객체를 묶어 새로운 기능을 제공하는 패턴

namespace StructuralPattern
{
    //클래스의 인터페이스를 다른 클라이언트의 인터페이스로 바꿈, 어뎁터는 클래스가 동시에 동작하게 만들어줌 어뎁터가 없으면 인터페이스간 호환이 되지 않아 동시에 작동하지 않음
    namespace Adapter
    {

        #region 구조
        /// <summary>
        /// Target(화합물)
        ///  - 특정 상황에 맞는 인터페이스가 정의된 것, Client에서 사용될 인터페이스
        /// Adapter(물질)
        ///  - 인터페이스를 어뎁티에 있는 인터페이스를 타겟 인터페이스에 연동시킴
        /// Adaptee(화학 데이터 베이스)
        ///  - 적용시킬 인터페이스 들고 있는 곳
        /// Client (AdapterApp)
        ///  - Target인터페이스에 맞춰 다른 오브젝트와 함께 행동이 개시되는 곳
        /// </summary>

        class AdapterEx
        {
            static void Main()
            {
                Target target = new AdapterInstance();
                target.Request();

                Console.ReadKey();
            }
        }
        class Target
        {
            public virtual void Request()
            {
                Console.WriteLine("타겟이 요청을 의뢰함");
            }
        }

        class AdapterInstance : Target
        {
            Adaptee _adaptee = new Adaptee();

            public override void Request()
            {
                base.Request();
                _adaptee.SpecificRequest();
            }
        }

        class Adaptee
        {
            public void SpecificRequest()
            {
                Console.WriteLine("특정 요구 요청");
            }
        }


        #endregion
        #region 예시
        class AdapterRealMain
        {
            static void Main()
            {
                Compound unknown = new Compound("Unknown");
                unknown.Display();

                Compound water = new RichCompound("Water");
                water.Display();

                Compound benzene = new RichCompound("Benzene");
                benzene.Display();

                Compound ethanol = new RichCompound("Ethanol");
                ethanol.Display();

                Console.ReadKey();
            }
        }

        class Compound
        {
            protected string _chemical;
            protected float _boilingPoint;
            protected float _meltingPoint;
            protected double _molecularWeight;
            protected string _molecularFormular;

            public Compound(string chemical)
            {
                this._chemical = chemical;
            }

            public virtual void Display()
            {
                Console.WriteLine($"Compound : {_chemical}");
            }
        }

        class RichCompound : Compound
        {
            ChemicalDatabank _bank;

            public RichCompound(string name) : base(name)
            {

            }

            public override void Display()
            {
                
                _bank = new ChemicalDatabank();
                _boilingPoint = _bank.GetCriticalPoint(_chemical, "B");
                _meltingPoint = _bank.GetCriticalPoint(_chemical, "M");
                _molecularWeight = _bank.GetMolecularWeight(_chemical);
                _molecularFormular = _bank.GetMolecularStructure(_chemical);
                base.Display();

                Console.WriteLine($"Formula : {_molecularFormular}");
                Console.WriteLine($"Weight : {_molecularWeight}");
                Console.WriteLine($"Melting pt : {_meltingPoint}");
                Console.WriteLine($"Boiling pt : {_boilingPoint}");
                
            }
        }

        class ChemicalDatabank
        {
            public float GetCriticalPoint(string compound, string point)
            {
                if(point == "M")
                {
                    switch (compound.ToLower())
                    {
                        case "water": return 0;
                        case "benzene": return 5.5f;
                        case "ethanol": return -114.1f;
                        default: return 0;
                    }
                }
                else
                {
                    switch (compound.ToLower())
                    {
                        case "water": return 100;
                        case "benzene": return 80.1f;
                        case "ethanol": return -78.3f;
                        default: return 0;
                    }
                }
            }

            public string GetMolecularStructure(string compound)
            {
                switch (compound.ToLower())
                {
                    case "water": return "H2O";
                    case "benzene": return "C6H6";
                    case "ethanol": return "C2H5OH";
                    default: return "";
                }
            }
            public double GetMolecularWeight(string compound)
            {
                switch (compound.ToLower())
                {
                    case "water": return 18.015;
                    case "benzene": return 78.1134;
                    case "ethanol": return 46.0688;
                    default: return 0;
                }
            }
        }
        #endregion
    }
    //abstract의 의존성을 줄여 두 객체의 의존성을 줄임
    namespace Bridge
    {
        #region 구조
        ///<summary>
        /// Abstraction (BusinessObject) : 추상 클래스의 인터페이스, 오브젝트 타입의 참조를 포함
        /// RefinedAbstraction(CustomersBusinessObject) : Abstraction에서 정의된 인터페이스 확장
        /// Implementor(DataObject) : 객체에 적용할 인터페이스가 정의된 클래스, 이 인터페이스는 Abstraction의 인터페이스와 연관이 있을 필요는 없음, 두 인터페이스가 다름, Implementor 인터페이스는 오직 primitive 동작만 정의하고, Abstraction은 primitive들 기반으로 보다 정교한 동작이 정의됨
        /// ConcreteImplementor(CustomersDataObject) : Implementor의 interface를 객체에 지정함
        ///</summary>
        class BridgeStructureMain
        {
            static void Main()
            {
                Abstraction ab = new RefinedAbstraction();

                ab.Implementor = new ConcreteImplementorA();
                ab.Operation();

                ab.Implementor = new ConcreteImplementorB();
                ab.Operation();

                Console.ReadKey();
            }
        }

        class Abstraction
        {
            protected Implementor implementor;

            public Implementor Implementor
            {
                set { implementor = value; }
            }
            public virtual void Operation()
            {
                implementor.Operation();
            }
        }

        abstract class Implementor
        {
            public abstract void Operation();
        }

        class RefinedAbstraction : Abstraction
        {
            public override void Operation()
            {
                implementor.Operation();
            }
        }

        class ConcreteImplementorA : Implementor
        {
            public override void Operation()
            {
                Console.WriteLine("ConcreteImplementorA Operation");
            }
        }

        class ConcreteImplementorB : Implementor
        {
            public override void Operation()
            {
                Console.WriteLine("ConcreteImplementorB Operation");
            }
        }
        #endregion
        #region 예시
        class BridgeRealMain
        {
            static void Main()
            {
                Customer customers = new Customer("Chicago");

                customers.Data = new CustomerData();

                customers.Show();
                customers.Next();
                customers.Show();
                customers.Next();
                customers.Show();
                customers.Add("Henry Velasquez");

                customers.ShowAll();

                Console.ReadKey();
            }
        }

        class CustomerBase
        {
            DataObject _dataObject;
            protected string group;

            public CustomerBase(string group)
            {
                this.group = group;
            }

            public DataObject Data
            {
                get { return _dataObject; }
                set { _dataObject = value; }

            }

            public virtual void Next()
            {
                _dataObject.NextRecord();
            }
            public virtual void Prior()
            {
                _dataObject.PriorRecord();
            }

            public virtual void Add(string customer)
            {
                _dataObject.AddRecord(customer);
            }

            public virtual void Show()
            {
                _dataObject.ShowRecord();
            }
            public virtual void ShowAll()
            {
                Console.WriteLine("Customer Group" + group);
                _dataObject.ShowAllRecord();
            }
        }

        class Customer : CustomerBase
        {
            public Customer(string group) : base(group)
            {

            }
            public override void ShowAll()
            {
                
                Console.WriteLine();
                Console.WriteLine("-------------------");
                base.ShowAll();
                Console.WriteLine("-------------------");
            }
        }

        abstract class DataObject
        {
            public abstract void NextRecord();
            public abstract void PriorRecord();
            public abstract void AddRecord(string name);
            public abstract void DeleteRecord(string name);
            public abstract void ShowRecord();
            public abstract void ShowAllRecord();

        }

        class CustomerData : DataObject
        {
            List<string> _customers = new List<string>();
            int _current = 0;

            public CustomerData()
            {
                _customers.Add("Jim Jones");
                _customers.Add("Samual Jackson");
                _customers.Add("Allen Good");
                _customers.Add("Ann Stills");
                _customers.Add("Lisa Giolani");
            }

            public override void NextRecord()
            {
                if(_current<= _customers.Count - 1)
                {
                    _current++;
                }
            }
            public override void PriorRecord()
            {
                if (_current > 0)
                {
                    _current--;
                }
            }
            public override void AddRecord(string name)
            {
                _customers.Add(name);
            }

            public override void DeleteRecord(string name)
            {
                _customers.Remove(name);
            }
            public override void ShowRecord()
            {
                Console.WriteLine(_customers[_current]);
            }
            public override void ShowAllRecord()
            {
                foreach(var customer in _customers)
                {
                    Console.WriteLine(" " + customer);
                }
            }

        }

        #endregion
    }
    //여러 개의 객체들로 구성된 복합 객체, 단일 객체를 클라이언트에서 구별없이 다룰 수 있게 만드는 패턴
    namespace Composite
    {
        #region 구조
        /// <summary>
        /// Component (DrawingElement) : 오브젝트의 성분에 들어갈 인터페이스 정의, 모든 클래스에 공통되는 인터페이스로 기본 성질등을 내포, 하위 성분에 접근할 수 있는 인터페이스 정의, 성분의 부모에 접근할 수 있는 인터페이스가 포함될 수 있음
        /// Leaf(PrimitiveElement) : Leaf 객체의 컴포넌트 성분, 하단 성분 없음, 기본 객체 행동이 포함됨
        /// Composite(CompositeElement) : 하위 성분이 포함된 작동이 정의됨, 하위 성분 가지고 있음, 하위와 연관된 작동함수가 포함됨
        /// Client(CompositeApp) : Component의 interface들을 조합하여 객체를 만듦
        /// </summary>
        class CompositStructureMain
        {
            static void Main()
            {
                Composite root = new Composite("root");
                root.Add(new Leaf("Leaf A"));
                root.Add(new Leaf("Leaf B"));

                Composite comp = new Composite("Composite X");
                comp.Add(new Leaf("Leaf XA"));
                comp.Add(new Leaf("Leaf XB"));

                root.Add(comp);
                root.Add(new Leaf("Leaf C"));

                Leaf leaf = new Leaf("Leaf D");
                root.Add(leaf);
                root.Remove(leaf);

                root.Display(1);

                Console.ReadKey();
            }
        }

        abstract class Component
        {
            protected string name;

            public Component(string name)
            {
                this.name = name;
            }

            public abstract void Add(Component c);
            public abstract void Remove(Component c);
            public abstract void Display(int depth);
        }

        class Composite : Component
        {
            List<Component> _children = new List<Component>();

            public Composite(string name) : base(name)
            {

            }

            public override void Add(Component c)
            {
                _children.Add(c);
            }
            public override void Remove(Component c)
            {
                _children.Remove(c);
            }
            public override void Display(int depth)
            {
                Console.WriteLine(new string('-', depth) + name);

                foreach(var component in _children)
                {
                    component.Display(depth + 2);
                }
            }

        }

        class Leaf : Component
        {
            public Leaf(string name): base(name)
            {

            }
            public override void Add(Component c)
            {
                Console.WriteLine("잎사귀에 더할 수 없음");
            }
            public override void Remove(Component c)
            {
                Console.WriteLine("잎사귀에서 제거할 수 없음");
            }
            public override void Display(int depth)
            {
                Console.WriteLine(new string('-', depth) + name);
            }
        }

        #endregion
        #region 예시

        class CompositeRealMain
        {
            static void Main()
            {
                CompositeElement root = new CompositeElement("Picture");
                root.Add(new PrimitiveElement("Red Line"));
                root.Add(new PrimitiveElement("Blue Circle"));
                root.Add(new PrimitiveElement("Green Box"));

                CompositeElement comp = new CompositeElement("Two Circle");
                comp.Add(new PrimitiveElement("Black Circle"));
                comp.Add(new PrimitiveElement("White Circle"));
                root.Add(comp);

                PrimitiveElement pe = new PrimitiveElement("Yellow Line");
                root.Add(pe);
                root.Remove(pe);

                root.Display(1);

                Console.ReadKey();
            }
        }


        abstract class DrawingElement
        {
            protected string name;
            public DrawingElement(string name)
            {
                this.name = name;
            }

            public abstract void Add(DrawingElement d);
            public abstract void Remove(DrawingElement d);
            public abstract void Display(int indent);
        }

        class PrimitiveElement : DrawingElement
        {
            public PrimitiveElement(string name): base(name)
            {

            }

            public override void Add(DrawingElement d)
            {
                Console.WriteLine("PrimitiveElement에는 성분을 더할 수 없음");
            }
            public override void Remove(DrawingElement d)
            {
                Console.WriteLine("PrimitiveElement에서 성분을 없앨 수 없음");
            }
            public override void Display(int indent)
            {
                Console.WriteLine(new string('-', indent) + name);
            }
        }


        class CompositeElement: DrawingElement
        {
            List<DrawingElement> elements = new List<DrawingElement>();

            public CompositeElement(string name) : base(name)
            {

            }
            public override void Add(DrawingElement d)
            {
                elements.Add(d);
            }
            public override void Remove(DrawingElement d)
            {
                elements.Remove(d);
            }
            public override void Display(int indent)
            {
                Console.WriteLine(new string('-', indent)+ "+ " + name);

                foreach(var d in elements)
                {
                    d.Display(indent + 2);
                }
            }
        }


        #endregion
    }
    //객체의 결합으로 기능을 동적으로 유연하게 확장할 수 있게 하는 패턴

    namespace Decorator
    {
        
        #region 구조
        /// <summary>
        /// Component (LibraryItem) : 객체에 동적으로 더해질 수 있는 인터페이스 정의
        /// ConcreteComponent(Book, Video) : 반응 할 수 있는 추가 행동 방식이 정의된 객체
        /// Decorator (Decorator) : Component 객체의 참조가 있고 Component의 인터페이스에서 따온 것..
        /// ConcreteDecorator (Borrowable) : 반응양식 성분
        /// </summary>
        
        class DecoratorStructuralMain
        {
            static void Main()
            {
                ConcreteComponent c = new ConcreteComponent();
                ConcreteDecoratorA d1 = new ConcreteDecoratorA();
                ConcreteDecoratorB d2 = new ConcreteDecoratorB();

                //데코레이터 연결
                d1.SetComponent(c);
                d2.SetComponent(d1);

                d2.Operation();

                Console.ReadKey();
            }
        }

        abstract class Component
        {
            public abstract void Operation();
        }

        class ConcreteComponent : Component
        {
            public override void Operation()
            {
                Console.WriteLine("ConcreteComponent.Operation()");
            }
        }

        abstract class Decorator : Component
        {
            protected Component component;

            public void SetComponent(Component component)
            {
                this.component = component;
            }

            public override void Operation()
            {
                component?.Operation();
            }
        }

        class ConcreteDecoratorA : Decorator
        {
            public override void Operation()
            {
                base.Operation();
                Console.WriteLine("ConcreteDecoratorA.Operation()");
            }
        }

        class ConcreteDecoratorB : Decorator
        {
            public override void Operation()
            {
                base.Operation();
                AddedBehavior();
                Console.WriteLine("ConcreteDecoratorB.Operation()");
            }
            void AddedBehavior()
            {

            }
        }
        #endregion

        #region 예시

        class DecoratorRealMain
        {
            static void Main()
            {
                Book book = new Book("Worley", "Inside ASP.NET", 10);
                book.Display();

                Video video = new Video("Spielberg", "Jaws", 23, 92);
                video.Display();

                Console.WriteLine("\n비디오를 대여할 수 있습니다");

                Borrowable borrowvideo = new Borrowable(video);
                borrowvideo.BorrowItem("고객 1");
                borrowvideo.BorrowItem("고객 2");

                borrowvideo.Display();

                Console.ReadKey();
            }
        }

        abstract class LibraryItem
        {
            int _numCopies;

            public int NumCopies
            {
                get { return _numCopies; }
                set { _numCopies = value; }
            }
            public abstract void Display();
        }

        class Book : LibraryItem
        {
            string author;
            string title;

            public Book(string author, string title, int numCopies)
            {
                this.author = author;
                this.title = title;
                this.NumCopies = numCopies;
            }

            public override void Display()
            {
                Console.WriteLine("\nBook----------");
                Console.WriteLine($"Author : {author}");
                Console.WriteLine($"Title : {title}");
                Console.WriteLine($"Copies : {NumCopies}");
            }
        }

        class Video : LibraryItem
        {
            string director;
            string title;
            int playtime;

            public Video(string director, string title, int numCopies, int playtime)
            {
                this.director = director;
                this.title = title;
                this.NumCopies = numCopies;
                this.playtime = playtime;
            }

            public override void Display()
            {
                Console.WriteLine("\nVideo---------");
                Console.WriteLine($"Director : {director}");
                Console.WriteLine($"Title : {title}");
                Console.WriteLine($"Copies : {NumCopies}");
                Console.WriteLine($"PlayTime : {playtime}");
            }
        }

        class DecoratorB : LibraryItem
        {
            protected LibraryItem libraryItem;

            public DecoratorB(LibraryItem libraryItem)
            {
                this.libraryItem = libraryItem;
            }

            public override void Display()
            {
                libraryItem.Display();
            }
        }

        class Borrowable : DecoratorB
        {
            List<string> borrowers = new List<string>();

            public Borrowable(LibraryItem libraryItem) : base(libraryItem)
            {

            }
            public void BorrowItem(string name)
            {
                borrowers.Add(name);
                libraryItem.NumCopies--;
            }
            public void ReturnItem(string name)
            {
                borrowers.Remove(name);
                libraryItem.NumCopies++;
            }

            public override void Display()
            {
                base.Display();
                foreach(string borrower in borrowers)
                {
                    Console.WriteLine($"빌려간 사람: {borrower}");
                }
            }
        }

        #endregion
    }
    //하단에서 사용될 공통 인터페이스를 제공, Facade는 상위단계의 인터페이스를 제공하여 하위단계에서 쓰기 쉽게 함
    namespace Facade
    {
        #region 구조
        /// <summary>
        /// 
        /// Facade (MortgageApplication) : 요청에 응답하는 하위 시스템 정보를 들고 있음, 클라이언트의 요청에 응답하는 대리자가 있음
        /// Subsystem classes (Bank, Credit, Loan) : 하위단계 기능을 부여함, Facade객체로부터 행동을 전달받음, facade의 정보가 없고 해당참조도 없음
        /// </summary>
        class FacadeStructuralMain
        {
            static void Main()
            {
                Facade facade = new Facade();

                facade.MethodA();
                facade.MethodB();

                Console.ReadKey();
            }
        }

        class SubSystemOne
        {
            public void MethodOne()
            {
                Console.WriteLine("서브 시스템 1 함수");
            }
        }
        class SubSystemTwo
        {
            public void MethodTwo()
            {
                Console.WriteLine("서브 시스템 2 함수");
            }
        }
        class SubSystemThree
        {
            public void MethodThree()
            {
                Console.WriteLine("서브 시스템 3 함수");
            }
        }

        class SubSystemFour
        {
            public void MethodFour()
            {
                Console.WriteLine("서브 시스템 4 함수");
            }
        }

        class Facade
        {
            SubSystemOne one;
            SubSystemTwo two;
            SubSystemThree three;
            SubSystemFour four;

            public Facade()
            {
                one = new SubSystemOne();
                two = new SubSystemTwo();
                three = new SubSystemThree();
                four = new SubSystemFour();

            }
            public void MethodA()
            {
                Console.WriteLine("\nMethodA()---------");
                one.MethodOne();
                two.MethodTwo();
                four.MethodFour();
            }
            public void MethodB()
            {
                Console.WriteLine("\nMethodB()---------");
                two.MethodTwo();
                three.MethodThree();
            }
        }
        #endregion
        #region 예시
        class FacadeRealMain
        {
            static void Main()
            {
                Mortgage mortgage = new Mortgage();

                Customer customer = new Customer("Ann McKinsey");

                bool eligible = mortgage.IsEligible(customer, 125000);

                Console.WriteLine(("\n" + customer.Name + "has been" + (eligible ? "Approved" : "Rejected")));

                Console.ReadKey();
            }
        }



        class Bank
        {
            public bool HasSufficientSavings(Customer c, int amount)
            {
                Console.WriteLine("Check bank for " + c.Name);
                return true;
            }
        }

        class Credit
        {
            public bool HasGoodCredit(Customer c)
            {
                Console.WriteLine("Check credit for " + c.Name);
                return true;
            }
        }

        class Loan
        {
            public bool HasNoBadLoans(Customer c)
            {
                Console.WriteLine("Check loans for " + c.Name);
                return true;
            }
        }

        class Customer
        {
            string name;
            
            public Customer(string name)
            {
                this.name = name;
            }
            public string Name
            {
                get { return name; }
            }
        }

        class Mortgage
        {
            Bank bank = new Bank();
            Loan loan = new Loan();
            Credit credit = new Credit();

            public bool IsEligible(Customer cust, int amount)
            {
                Console.WriteLine($"{cust.Name} applies for {amount:C} loan\n");

                bool eligibile = true;
                if(!bank.HasSufficientSavings(cust, amount))
                {
                    eligibile = false;
                }
                else if (!loan.HasNoBadLoans(cust))
                {
                    eligibile = false;
                }
                else if (!credit.HasGoodCredit(cust))
                {
                    eligibile = false;
                }

                return eligibile;
            }
        }
        #endregion
    }
    //많은 숫자의 잘 정리된 오브젝트를 효율적으로 공유하는 방법
    namespace Flyweight
    {
        #region 구조
        /// <summary>
        /// Flyweight (Character) : flyweight들을 통해 전달받고 행동하는 상태들을 결정하는 인터페이스 내장
        /// ConcreteFlyweight (CharacterA, CharacterB,...CharacterZ) : Flyweight의 인터페이스와 기본적인 상태를 포함, 해당객체는 공유 가능하고 어느 상태라도 ConcreteFlyweight의 객체의 상태와 무관해야 함
        /// UnsharedConcreteFlyweight (not used) : Flyweight의 모든 하위 클래스가 꼭 공유가능한 상태는 아니어도 됨, Flyweight의 인터페이스는 공유 가능하나 반드시 공유가능한 상태일 필요는 없음
        ///  공유하지 않는 오브젝트는 ConcreteFlyweight를 하위 클래스에 두고 있음
        /// FlyweightFactory (CharacterFactory) : flyweight 객체 생성, 관리 담당, flyweight를 공유, client가 flyweight를 요청하면 FlyweightFactory객체를 지정 혹은 없으면 생성함
        /// Client (FlyweightApp) : flyweight들의 참조를 유지, flyweight의 기본 상태를 계산, 가지고 있음
        /// </summary>
        
        class FlyweightStructuralMain
        {
            static void Main()
            {
                int extrinsicstate = 22;

                FlyweightFactory factory = new FlyweightFactory();

                Flyweight fx = factory.GetFlyweight("X");
                fx.Operation(--extrinsicstate);

                Flyweight fy = factory.GetFlyweight("Y");
                fy.Operation(--extrinsicstate);

                Flyweight fz = factory.GetFlyweight("Z");
                fz.Operation(--extrinsicstate);

                UnsharedConcreteFlyweight fu = new UnsharedConcreteFlyweight();

                fu.Operation(--extrinsicstate);

                Console.ReadKey();
            }
        }

        class FlyweightFactory
        {
            Dictionary<string, ConcreteFlyweight> flyweight = new Dictionary<string, ConcreteFlyweight>();

            public FlyweightFactory()
            {
                flyweight.Add("X", new ConcreteFlyweight());
                flyweight.Add("Y", new ConcreteFlyweight());
                flyweight.Add("Z", new ConcreteFlyweight());
            }

            public Flyweight GetFlyweight(string key)
            {
                return (Flyweight)flyweight[key];
            }
        }

        abstract class Flyweight
        {
            public abstract void Operation(int extrinsicstate);
        }

        class ConcreteFlyweight : Flyweight
        {
            public override void Operation(int extrinsicstate)
            {
                Console.WriteLine("ConcreteFlyweight: " + extrinsicstate);
            }
        }

        class UnsharedConcreteFlyweight : Flyweight
        {
            public override void Operation(int extrinsicstate)
            {
                Console.WriteLine("UnsharedConcreteFlyweight: " + extrinsicstate);
            }
        }

        #endregion
        #region 예시
        class FlyweightRealMain
        {
            static void Main()
            {
                string document = "AAZZBBZB";
                char[] chars = document.ToCharArray();

                CharacterFactory factory = new CharacterFactory();

                int pointSize = 10;

                foreach(var c in chars)
                {
                    pointSize++;
                    Character character = factory.GetCharacter(c);
                    character.Display(pointSize);
                }

                Console.ReadKey();
            }
        }

        class CharacterFactory
        {
            Dictionary<char, Character> characters = new Dictionary<char, Character>();

            public Character GetCharacter(char key)
            {
                Character character = null;
                if (characters.ContainsKey(key))
                {
                    character = characters[key];
                }
                else
                {
                    switch (key)
                    {
                        case 'A': character = new CharacterA();
                            break;
                        case 'B': character = new CharacterB();
                            break;
                        case 'Z': character = new CharacterZ();
                            break;

                    }
                    characters.Add(key, character);

                }
                return character;
            }
        }


        abstract class Character
        {
            protected char symbol;
            protected int width;
            protected int height;
            protected int ascent;
            protected int descent;
            protected int pointSize;

            public abstract void Display(int pointSize);
        }

        class CharacterA : Character
        {
            public CharacterA()
            {
                symbol = 'A';
                height = 100;
                width = 120;
                ascent = 70;
                descent = 0;
            }

            public override void Display(int pointSize)
            {
                this.pointSize = pointSize;
                Console.WriteLine(symbol + "(pointsize " + this.pointSize + ")");
            }
        }

        class CharacterB : Character
        {
            public CharacterB()
            {
                symbol = 'B';
                height = 100;
                width = 140;
                ascent = 72;
                descent = 0;
            }
            public override void Display(int pointSize)
            {
                this.pointSize = pointSize;
                Console.WriteLine(symbol + "(pointsize " + this.pointSize + ")");
            }
        }

        class CharacterZ : Character
        {
            public CharacterZ()
            {
                symbol = 'Z';
                height = 100;
                width = 100;
                ascent = 68;
                descent = 0;
            }

            public override void Display(int pointSize)
            {
                this.pointSize = pointSize;
                Console.WriteLine(symbol + "(pointsize " + this.pointSize + ")");
            }
        }
        #endregion
    }
    //다른 오브젝트에 접근, 제어할 수 있는 방법을 제공하는 형태
    namespace Proxy
    {
        #region 구조
        /// <summary>
        /// Proxy (MathProxy) : 
        /// </summary>
        class ProxyStructuralMain
        {

        }
        #endregion
        #region 예시

        #endregion
    }
}
