using System;
//행위 패턴 모음
//객체, 클래스 사이의 알고리즘, 책임 분배에 관련된 패턴
//한 객체가 혼자 수행할 수 없는 작업을 여러 개의 객체로 어떻게 분배하는지, 또 그렇게 하면서 객체 사이의 결합도를 최소화하는 것에 중점

namespace BehavioralPattern
{
    //어떤 요청에 따라 정보를 넘겨주는 객체와 받는 객체 사이의 직접적인 연관성을 줄이고 하나 이상의 객체가 요청에 반응할 수 있게 함
    //특정 객체가 요청을 수행중이면 요청이 체인을 통해 해당 객체로 전달됨
    namespace ChainofResponsibility
    {
        #region 구조
        /// <summary>
        /// Handler (Approver) : 요청사항을 처리할 인터페이스 정의 (선택사항) 하단 링크 포함
        /// 
        /// ConcreteHandler (Director, VicePresident, President) : 요청사항을 각자의 상태에 따라 처리할 객체, 각각  Handler의 하단 링크에 접근 가능
        /// ConcreteHandler가 요청을 직접 처리할 수 있으면 그렇게 할 수 있음, 그렇지 않으면 해당 요구사항을 Handler의 하단링크에 전달함
        /// 
        /// Client (ChainApp) : ConcreteHandler의 객체에 전달할 요청사항을 처음 시작
        /// </summary>
        class CorStructuralMain
        {
            static void Main()
            {
                Handler h1 = new ConcreteHandler1();
                Handler h2 = new ConcreteHandler2();
                Handler h3 = new ConcreteHandler3();

                h1.SetSuccessor(h2);
                h2.SetSuccessor(h3);

                int[] requests = { 2, 5, 14, 22, 18, 3, 27, 20 };

                foreach(var request in requests)
                {
                    h1.HandleRequest(request);
                }

                Console.ReadKey();

            }
        }

        abstract class Handler
        {
            protected Handler successor;

            public void SetSuccessor(Handler successor)
            {
                this.successor = successor;
            }

            public abstract void HandleRequest(int request);
        }

        class ConcreteHandler1 : Handler
        {
            public override void HandleRequest(int request)
            {
                if (request >= 0 && request < 10)
                {
                    Console.WriteLine($"{this.GetType().Name} handled request {request}");
                }
                else if(successor != null)
                {
                    successor.HandleRequest(request);
                }
            }
        }

        class ConcreteHandler2 : Handler
        {
            public override void HandleRequest(int request)
            {
                if(request>=10 && request < 20)
                {
                    Console.WriteLine($"{this.GetType().Name} handled request {request}");
                }
                else if(successor != null)
                {
                    successor.HandleRequest(request);
                }
            }
        }

        class ConcreteHandler3 : Handler
        {
            public override void HandleRequest(int request)
            {
                if (request >= 20 && request < 30)
                {
                    Console.WriteLine($"{this.GetType().Name} handled request {request}");
                }
                else if (successor != null)
                {
                    successor.HandleRequest(request);
                }
            }
        }
        #endregion
        #region 실제 예시
        class CorRealMain
        {
            static void Main()
            {
                Approver larry = new Director();
                Approver sam = new VicePresident();
                Approver tammy = new President();

                larry.SetSuccessor(sam);
                sam.SetSuccessor(tammy);

                Purchase p = new Purchase(2034, 350.00, "Assets");
                larry.ProcessRequest(p);

                p = new Purchase(2035, 32590.10, "Project X");
                larry.ProcessRequest(p);

                p = new Purchase(2036, 122100.00, "Project Y");
                larry.ProcessRequest(p);

                Console.ReadKey();
            }
        }

        abstract class Approver
        {
            protected Approver successor;

            public void SetSuccessor(Approver successor)
            {
                this.successor = successor;
            }
            public abstract void ProcessRequest(Purchase purchase);
        }

        class Director : Approver
        {
            public override void ProcessRequest(Purchase purchase)
            {
                if(purchase.Amount< 10000.0)
                {
                    Console.WriteLine($"{this.GetType().Name} approved request# {purchase.Number}");
                }
                else
                {
                    successor?.ProcessRequest(purchase);
                }
            }
        }

        class VicePresident : Approver
        {
            public override void ProcessRequest(Purchase purchase)
            {
                if (purchase.Amount < 25000.0)
                {
                    Console.WriteLine($"{this.GetType().Name} approved request# {purchase.Number}");
                }
                else
                {
                    successor?.ProcessRequest(purchase);
                }
            }
        }

        class President : Approver
        {
            public override void ProcessRequest(Purchase purchase)
            {
                if (purchase.Amount < 100000.0)
                {
                    Console.WriteLine($"{this.GetType().Name} approved request# {purchase.Number}");
                }
                else
                {
                    Console.WriteLine($"Request# {purchase.Number} requires an executive meeting");
                }
            }
        }

        class Purchase
        {
            int number;
            double amount;
            string purpose;

            public Purchase(int number, double amount, string purpose)
            {
                this.number = number;
                this.amount = amount;
                this.purpose = purpose;
            }

            public int Number
            {
                get { return number; }
                set { number = value; }
            }

            public double Amount
            {
                get { return amount; }
                set { amount = value; }
            }

            public string Purpose
            {
                get { return purpose; }
                set { purpose = value; }
            }
        }

        #endregion
    }
    //실행될 기능을 캡슐화 하여 주어진 여러 기능을 실행할 수 있게 재사용성이 높은 클래스를 설계하는 패턴
    namespace Command
    {

    }

    namespace Interpreter
    {

    }

    namespace Iterator
    {

    }

    namespace Memento
    {

    }
    //한 객체의 상태 편화에 따라 다른 객체의 상태도 연동되도록 일대 다 객체 의존관계를 구성
    namespace Observer
    {

    }
    //객체의 상태에 따라 객체의 행위 내용을 바꿔주는 패턴
    namespace State
    {

    }
    //행위를 클래스로 캡슐화 하여 동적으로 행위를 자유로이 바꿀 수 있도록 해주는 패턴
    namespace Strategy
    {

    }
    //작업을 처리하는 일부분을 서브 클래스로 캡슐화 하여 전체 일 수행 구조는 바꾸지 않고 특정 단계 수행내용을 바꾸는 패턴
    namespace TemplateMethod
    {

    }
    namespace Visitor
    {

    }
}
