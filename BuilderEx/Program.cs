using System;
using System.Collections.Generic;
//생성 패턴: 객체 재현과 객체 생성을 분리하는 방식, 객체 생성 방식은 동일하나 서로 다른 형태를 가진 객체를 만들어낼 수 있음
namespace CreationPattern.BuilderEx
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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
