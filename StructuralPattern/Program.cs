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

    }
    //객체의 결합으로 기능을 동적으로 유연하게 확장할 수 있게 하는 패턴

    namespace Decorator
    {

    }
    namespace Facade
    {

    }
    namespace Flyweight
    {

    }
    namespace Proxy
    {

    }
}
