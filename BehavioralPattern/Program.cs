using System;
using System.Collections.Generic;
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
        #region 구조
        /// <summary>
        /// Command (Command) : 행동에 관한 인터페이스 정의
        /// ConcreteCommand (CalculatorCommand) : 행동과 특정 파라미터를 받는 객체를 연결, 특정 행동을 불러오는 함수(받는 객체에 연결되는)를 정의
        /// Client (CommandApp) : ConcreteCommand객체를 생성하고 받는 객체로 설정
        /// Invoker (User) : 행동이 실행되도록 요청
        /// Receiver (Calculator) : 요청에 따라 동작이 어떻게 진행되는지 정의되어있음
        /// </summary>
        class CommandStructuralMain
        {
            static void Main()
            {
                Receiver receiver = new Receiver();
                StructureCommand command = new ConcreteCommand(receiver);
                Invoker invoker = new Invoker();

                invoker.SetCommand(command);
                invoker.ExecuteCommand();

                Console.ReadKey();
            }
        }

        abstract class StructureCommand
        {
            protected Receiver receiver;

            public StructureCommand(Receiver receiver)
            {
                this.receiver = receiver;
            }
            public abstract void Execute();
        }

        class ConcreteCommand : StructureCommand
        {
            public ConcreteCommand(Receiver receiver) : base(receiver)
            {

            }
            public override void Execute()
            {
                receiver.Action();
            }
        }

        class Receiver
        {
            public void Action()
            {
                Console.WriteLine("Called Receiver.Action()");
            }
        }

        class Invoker
        {
            StructureCommand command;

            public void SetCommand(StructureCommand command)
            {
                this.command = command;
            }

            public void ExecuteCommand()
            {
                command.Execute();
            }
        }
        #endregion
        #region 실제 사례
        class CommandRealMain
        {
            static void Main()
            {
                User user = new User();

                user.Compute('+', 100);
                user.Compute('-', 50);
                user.Compute('*', 10);
                user.Compute('/', 2);

                user.Undo(4);

                user.Redo(3);

                Console.ReadKey();
            }
        }

        abstract class CommandReal
        {
            public abstract void Execute();
            public abstract void UnExecute();
        }

        class CalculatorCommand : CommandReal
        {
            char _operator;
            int operand;
            Calculator calculator;

            public CalculatorCommand(Calculator calculator, char @operator, int operand)
            {
                this.calculator = calculator;
                this._operator = @operator;
                this.operand = operand;
            }
            public char Operator
            {
                set { _operator = value; }
            }
            public int Operand
            {
                set { operand = value; }
            }

            public override void Execute()
            {
                calculator.Operation(_operator, operand);
            }
            public override void UnExecute()
            {
                calculator.Operation(Undo(_operator), operand);
            }

            char Undo(char @operator) =>
            
                @operator switch
                {
                    '+' => '-',
                    '-' => '+',
                    '*' => '/',
                    '/' => '*',
                    _ => throw new ArgumentException("@operator"),
                };
        }

        class Calculator
        {
            int curr = 0;

            public void Operation(char @operator, int operand)
            {
                switch (@operator)
                {
                    case '+':
                        curr += operand;
                        break;
                    case '-':
                        curr -= operand;
                        break;
                    case '*':
                        curr *= operand;
                        break;
                    case '/':
                        curr /= operand;
                        break;
                }
                Console.WriteLine($"Current value = {curr,3} (following {@operator} {operand})");
            }
        }

        class User
        {
            Calculator calculator = new Calculator();
            List<CommandReal> commands = new List<CommandReal>();
            int current = 0;

            public void Redo(int levels)
            {
                Console.WriteLine($"\n----Redo {levels} level");
                for(int i = 0; i<levels; i++)
                {
                    if (current < commands.Count - 1)
                    {
                        CommandReal command = commands[current++];
                        command.Execute();
                    }
                }
            }

            public void Undo(int levels)
            {
                Console.WriteLine($"\n----Undo {levels} level");
                for(int i = 0; i<levels; i++)
                {
                    if (current > 0)
                    {
                        CommandReal command = commands[--current] as CommandReal;
                        command.UnExecute();
                    }
                }
            }

            public void Compute(char @operator, int operand)
            {
                CommandReal command = new CalculatorCommand(calculator, @operator, operand);
                command.Execute();

                commands.Add(command);
                current++;
            }
        }


        #endregion
    }
    //단어를 주고 해당 표현의 문법을 정의하고 표현된 문법의 해석을 달아두는 것...
    namespace Interpreter
    {
        #region 구조
        /// <summary>
        /// AbstractExpression (Expression) : 실행되어야 할 함수 인터페이스 정의
        /// TerminalExpression (ThousandExpression, HundredExpression, TenExpression, OneExpression) : 전달되는 끝의 객체 문법에서 해석되는 행동들이 정의되어있음, 모든 객체는 문장에 모든 끝맺음 기호가 있어야 함
        /// NonterminalExpression (not used) : 
        /// </summary>
        class InterpreterStructuralMain
        {
            
        }
        #endregion
        #region 실제 사례

        #endregion
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
