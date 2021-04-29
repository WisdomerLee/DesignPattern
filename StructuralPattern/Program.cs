﻿using System;
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

        }
        #endregion
        #region 예시

        #endregion
    }
    namespace Flyweight
    {

    }
    namespace Proxy
    {

    }
}
