using System;
//구조 패턴들 모음
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
    namespace Bridge
    {

    }
    namespace Composite
    {

    }
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
