using System;
using System.Collections.Generic;
//dofactory.com/net/prototye-design-pattern
//특정 종류의 오브젝트를 프로토타입 계통의 객체를 통해 만들고 이 프로토타입을 복사하는 형태로 새 객체를 만드는 방식


namespace CreationPattern.ProtoTypeEx
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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
