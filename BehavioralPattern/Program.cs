using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Common;
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
        /// NonterminalExpression (not used) : 하나의 클래스는 R::= R1R2..Rn이 문법에 포함된 규칙을 가지고 있음
        /// AbstractExpression의 각각의 객체에 해당하는 기호의 변수를 유지함
        /// 재귀함수로 쓰일 수 있음..
        /// Context (Context) : 전역변수 정보를 저장
        /// Client (InterpreterApp) : 압축된 정보를 표현, 각각의 언어로 표시, NonTerminalExpression과 TerminalExpression 클래스의 객체에서 가지고 오게 됨, 해석 함수 실행을 요청
        /// </summary>
        class InterpreterStructuralMain
        {
            static void Main()
            {
                Context context = new Context();

                ArrayList list = new ArrayList();

                list.Add(new TerminalExpression());
                list.Add(new NonterminalExpression());
                list.Add(new TerminalExpression());
                list.Add(new TerminalExpression());

                foreach(AbstractExpression exp in list)
                {
                    exp.Interpret(context);
                }

                Console.ReadKey();
            }
        }

        class Context
        {

        }

        abstract class AbstractExpression
        {
            public abstract void Interpret(Context context);
        }

        class TerminalExpression : AbstractExpression
        {
            public override void Interpret(Context context)
            {
                Console.WriteLine("Called Terminal.Interpret()");
            }
        }

        class NonterminalExpression : AbstractExpression
        {
            public override void Interpret(Context context)
            {
                Console.WriteLine("Called Nonterminal.Interpret()");
            }
        }
        #endregion
        #region 실제 사례
        class RealMain
        {
            static void Main()
            {
                string roman = "MCMXXVIII";
                ContextReal context = new ContextReal(roman);

                List<Expression> tree = new List<Expression>();
                tree.Add(new ThousandExpression());
                tree.Add(new HundredExpression());
                tree.Add(new TenExpression());
                tree.Add(new OneExpression());

                foreach(var exp in tree)
                {
                    exp.Interpret(context);
                }

                Console.WriteLine($"{roman} = {context.Output}");
                Console.ReadKey();
            }
        }

        class ContextReal
        {
            string input;
            int output;
            public ContextReal(string input)
            {
                this.input = input;
            }

            public string Input
            {
                get { return input; }
                set { input = value; }
            }

            public int Output
            {
                get { return output; }
                set { output = value; }
            }
        }

        abstract class Expression
        {
            public void Interpret(ContextReal context)
            {
                if(context.Input.Length == 0)
                {
                    return;
                }
                if (context.Input.StartsWith(Nine()))
                {
                    context.Output += (9 * Multiplier());
                    context.Input = context.Input.Substring(2);
                }
                else if (context.Input.StartsWith(Four()))
                {
                    context.Output += (4 * Multiplier());
                    context.Input = context.Input.Substring(2);
                }
                else if (context.Input.StartsWith(Five()))
                {
                    context.Output += (5 * Multiplier());
                    context.Input = context.Input.Substring(1);
                }

                while (context.Input.StartsWith(One()))
                {
                    context.Output += (1 * Multiplier());
                    context.Input = context.Input.Substring(1);
                }
            }
            public abstract string One();
            public abstract string Four();
            public abstract string Five();
            public abstract string Nine();
            public abstract int Multiplier();
        }

        class ThousandExpression : Expression
        {
            public override string Five()
            {
                return " ";
            }

            public override string Four()
            {
                return " ";
            }

            public override int Multiplier()
            {
                return 1000;
            }

            public override string Nine()
            {
                return " ";
            }

            public override string One()
            {
                return "M";
            }
        }

        class HundredExpression : Expression
        {
            public override string Five()
            {
                return "D";
            }

            public override string Four()
            {
                return "CD";
            }

            public override int Multiplier()
            {
                return 100;
            }

            public override string Nine()
            {
                return "CM";
            }

            public override string One()
            {
                return "C";
            }
        }

        class TenExpression : Expression
        {
            public override string Five()
            {
                return "L";
            }

            public override string Four()
            {
                return "XL";
            }

            public override int Multiplier()
            {
                return 10;
            }

            public override string Nine()
            {
                return "XC";
            }

            public override string One()
            {
                return "X";
            }
        }

        class OneExpression : Expression
        {
            public override string Five()
            {
                return "V";
            }

            public override string Four()
            {
                return "IV";
            }

            public override int Multiplier()
            {
                return 1;
            }

            public override string Nine()
            {
                return "IX";
            }

            public override string One()
            {
                return "I";
            }
        }
        #endregion
    }
    //해당 객체의 내용등에 의존하지 않고 각 원소들에 순차적으로 접근할 수 있는 방법을 제공하는 방법
    namespace Iterator
    {
        #region 구조
        /// <summary>
        /// Iterator(AbstractIterator) : 원소에 접근, 제어 가능한 인터페이스 정의
        /// ConcreteIterator(Iterator) : Iterator의 인터페이스 구현, 압축된 정보들의 현재 상태를 저장하고 추적함
        /// Aggregate(AbstractCollection) : Iterator의 객체 생성 인터페이스 정의
        /// ConcreteAggregate(Collection) : Iterator 생성 인터페이스 구현, 
        /// </summary>
        class StructuralMain
        {
            static void Main()
            {
                ConcreteAggregate a = new ConcreteAggregate();
                a[0] = "Item A";
                a[1] = "Item B";
                a[2] = "Item C";
                a[3] = "Item D";

                Iterator i = a.CreateIterator();

                Console.WriteLine("Iterating over collection");

                object item = i.First();
                while(item != null)
                {
                    Console.WriteLine(item);
                    item = i.Next();
                }

                Console.ReadKey();
            }
        }

        abstract class Aggregate
        {
            public abstract Iterator CreateIterator();
        }

        class ConcreteAggregate : Aggregate
        {
            ArrayList items = new ArrayList();

            public override Iterator CreateIterator()
            {
                return new ConcreteIterator(this);
            }

            public int Count
            {
                get
                {
                    return items.Count;
                }
            }

            public object this[int index]
            {
                get { return items[index]; }
                set { items.Insert(index, value); }
            }
        }
        abstract class Iterator
        {
            public abstract object First();
            public abstract object Next();
            public abstract bool IsDone();
            public abstract object CurrentItem();
        }

        class ConcreteIterator : Iterator
        {
            ConcreteAggregate aggregate;
            int current = 0;

            public ConcreteIterator(ConcreteAggregate aggregate)
            {
                this.aggregate = aggregate;
            }

            public override object First()
            {
                return aggregate[0];
            }

            public override object Next()
            {
                object ret = null;
                if (current < aggregate.Count - 1)
                {
                    ret = aggregate[++current];
                }
                return ret;
            }

            public override object CurrentItem()
            {
                return aggregate[current];
            }
            public override bool IsDone()
            {
                return current >= aggregate.Count;
            }
        }

        #endregion
        #region 실제 사례
        class RealMain
        {
            static void Main()
            {
                Collection collection = new Collection();
                collection[0] = new Item("Item 0");
                collection[1] = new Item("Item 1");
                collection[2] = new Item("Item 2");
                collection[3] = new Item("Item 3");
                collection[4] = new Item("Item 4");
                collection[5] = new Item("Item 5");
                collection[6] = new Item("Item 6");
                collection[7] = new Item("Item 7");
                collection[8] = new Item("Item 8");

                IteratorReal iterator = collection.CreateIterator();

                iterator.Step = 2;

                Console.WriteLine("Iterating over collection");

                for(Item item = iterator.First(); !iterator.IsDone; item = iterator.Next())
                {
                    Console.WriteLine(item.Name);
                }

                Console.ReadKey();
            }
        }

        class Item
        {
            string name;
            public Item(string name)
            {
                this.name = name;
            }
            public string Name
            {
                get { return name; }
            }
        }

        interface IAbstractCollection
        {
            IteratorReal CreateIterator();
        }

        class Collection : IAbstractCollection
        {
            ArrayList items = new ArrayList();

            public IteratorReal CreateIterator()
            {
                return new IteratorReal(this);
            }

            public int Count
            {
                get
                {
                    return items.Count;
                }
            }

            public object this[int index]
            {
                get { return items[index]; }
                set { items.Add(value); }
            }
        }

        interface IAbstractIterator
        {
            Item First();
            Item Next();
            bool IsDone { get; }
            Item CurrentItem { get; }
        }
        class IteratorReal : IAbstractIterator
        {
            Collection collection;
            int current = 0;
            int step = 1;

            public IteratorReal(Collection collection)
            {
                this.collection = collection;
            }

            public Item First()
            {
                current = 0;
                return collection[current] as Item;
            }

            public Item Next()
            {
                current += step;
                if (!IsDone)
                {
                    return collection[current] as Item;
                }
                else
                {
                    return null;
                }
            }
            public int Step
            {
                get
                {
                    return step;
                }
                set
                {
                    step = value;
                }
            }
            public Item CurrentItem
            {
                get { return collection[current] as Item; }
            }

            public bool IsDone
            {
                get
                {
                    return current >= collection.Count;
                }
            }
        }
        #endregion
    }

    //객체의 상호작용을 묶을 수 있는 방법을 제공, Mediator는 결합도를 낮추고 객체간의 상호 참조를 최대한 줄임, 각각의 상호작용의 의존성을 줄일 수 있음
    namespace Mediator
    {
        #region 구조
        /// <summary>
        /// Mediator (IChatroom) : 상호작용하는 오브젝트의 상호작용을 정의한 인터페이스가 있음
        /// ConcreteMediator (Chatroom) : 상호작용하는 오브젝트들의 협동하는 행동 등이 포함됨
        /// Collegue classes (Participant) : 각 Collegue 클래스는 각각의 Mediator 객체 정보를 알고 있음, 다른 collegue들과 상호작용이 일어날 때  각 colleague들은 mediator와 상호작용을 함
        /// </summary>
        class StructuralMain
        {
            static void Main()
            {
                ConcreteMediator m = new ConcreteMediator();
                ConcreteColleague1 c1 = new ConcreteColleague1(m);
                ConcreteColleague2 c2 = new ConcreteColleague2(m);

                m.Colleague1 = c1;
                m.Colleague2 = c2;

                c1.Send("How are you?");
                c2.Send("Fine, thanks");

                Console.ReadKey();
            }
        }

        abstract class Mediator
        {
            public abstract void Send(string message, Colleague colleague);
        }

        class ConcreteMediator : Mediator
        {
            ConcreteColleague1 colleague1;
            ConcreteColleague2 colleague2;

            public ConcreteColleague1 Colleague1
            {
                set { colleague1 = value; }
            }
            public ConcreteColleague2 Colleague2
            {
                set { colleague2 = value; }
            }

            public override void Send(string message, Colleague colleague)
            {
                if(colleague == colleague1)
                {
                    colleague2.Notify(message);
                }
                else
                {
                    colleague1.Notify(message);
                }
            }
        }

        abstract class Colleague
        {
            protected Mediator mediator;

            public Colleague(Mediator mediator)
            {
                this.mediator = mediator;
            }
        }

        class ConcreteColleague1 : Colleague
        {
            public ConcreteColleague1(Mediator mediator) : base(mediator)
            {
                
            }
            public void Send(string message)
            {
                mediator.Send(message, this);
            }
            public void Notify(string message)
            {
                Console.WriteLine("Colleague1 gets message : " + message);
            }
        }
        class ConcreteColleague2 : Colleague
        {
            public ConcreteColleague2(Mediator mediator): base(mediator)
            {

            }
            public void Send(string message)
            {
                mediator.Send(message, this);
            }
            public void Notify(string message)
            {
                Console.WriteLine("Colleague2 gets message : " + message);
            }
        }
        #endregion
        #region 실제예시
        class RealMain
        {
            static void Main()
            {
                Chatroom chatroom = new Chatroom();

                Participant George = new Beatle("George");
                Participant Paul = new Beatle("Paul");
                Participant Ringo = new Beatle("Ringo");
                Participant John = new Beatle("John");
                Participant Yoko = new NonBeatle("Yoko");

                chatroom.Register(George);
                chatroom.Register(Paul);
                chatroom.Register(Ringo);
                chatroom.Register(John);
                chatroom.Register(Yoko);

                Yoko.Send("John", "Hi John!");
                Paul.Send("Ringo", "All you need is love");
                Ringo.Send("George", "My sweet Lord");
                Paul.Send("John", "Can't buy me love");
                John.Send("Yoko", "My sweet love");

                Console.ReadKey();
            }
        }

        abstract class AbstractChatroom
        {
            public abstract void Register(Participant participant);
            public abstract void Send(string from, string to, string message);
        }

        class Chatroom : AbstractChatroom
        {
            Dictionary<string, Participant> participants = new Dictionary<string, Participant>();

            public override void Register(Participant participant)
            {
                if (!participants.ContainsValue(participant))
                {
                    participants[participant.Name] = participant;
                }
                participant.Chatroom = this;
            }

            public override void Send(string from, string to, string message)
            {
                Participant participant = participants[to];
                if(participant != null)
                {
                    participant.Receive(from, message);
                }
            }
        }
        class Participant
        {
            Chatroom chatroom;
            string name;

            public Participant(string name)
            {
                this.name = name;
            }

            public string Name
            {
                get { return name; }
            }

            public Chatroom Chatroom
            {
                set { chatroom = value; }
                get { return chatroom; }
            }

            public void Send(string to, string message)
            {
                chatroom.Send(name, to, message);
            }

            public virtual void Receive(string from, string message)
            {
                Console.WriteLine($"{from} to {Name}: {message}");
            }
        }
        class Beatle : Participant
        {
            public Beatle(string name) : base(name)
            {

            }
            public override void Receive(string from, string message)
            {
                Console.Write("To a Beatle: ");
                base.Receive(from, message);
            }
        }
        class NonBeatle: Participant
        {
            public NonBeatle(string name): base(name)
            {

            }
            public override void Receive(string from, string message)
            {
                Console.Write("To a non-Beatle: ");
                base.Receive(from, message);
            }
        }
        #endregion
    }
    //캡슐화를 깨지 않고 객체의 내부 상태를 얻고 외부로 돌려 나중에 객체의 상태를 되돌릴 수 있게 만드는 방법
    namespace Memento
    {

        #region 구조
        /// <summary>
        /// Memento (Memento) : Originator객체의 내부 상태를 저장, Memento는 필요한 만큼의 내부 상태들을 일부만 저장할 수 있음
        /// 다른 originator객체는 접근이 안됨, Memento는 효율성 문제로 두 개의 interface를 가지고 있는 경우가 많음, Caretaker는 Memento의 매우 적은 interface를 통해 접근 가능
        /// memento에서 다른 객체로 접근이 허용된 유일한 곳, Originator는 보다 넓은 interface접근이 가능, 즉 저장하고 있는 이전의 모든 상태에 정보에 대해 접근이 가능함
        /// 이론적으로 orginator만 memento의 내부 상태에 접근 가능해야 함
        /// 
        /// Originator(SalesProspect) : memento를 생성하고 현재 내부 상태를 저장, memento를 이용하여 원래의 상태로 되돌릴 수 있음
        /// Caretaker (Caretaker) : memento의 캡슐성을 보호, memento의 상태를 외부로 내보내거나 동작하지 않음
        /// </summary>
        class StructuralMain
        {
            static void Main()
            {
                Originator o = new Originator();
                o.State = "On";

                Caretaker c = new Caretaker();
                c.Memento = o.CreateMemento();

                o.State = "Off";

                o.SetMemento(c.Memento);

                Console.ReadKey();
            }
        }

        class Originator
        {
            string state;
            public string State
            {
                get
                {
                    return state;
                }
                set
                {
                    state = value;
                    Console.WriteLine("State = " + state);
                }
            }

            public Memento CreateMemento()
            {
                return (new Memento(state));
            }

            public void SetMemento(Memento memento)
            {
                Console.WriteLine("Restoring state...");
                State = memento.State;
            }
        }

        class Memento
        {
            string state;

            public Memento(string state)
            {
                this.state = state;
            }

            public string State
            {
                get
                {
                    return state;
                }
            }
        }

        class Caretaker
        {
            Memento memento;
            public Memento Memento
            {
                get
                {
                    return memento;
                }
                set
                {
                    memento = value;
                }
            }

        }

        #endregion
        #region 실제 사례
        class RealMain
        {
            static void Main()
            {
                SalesProspect s = new SalesProspect();

                s.Name = "Noel van Halen";
                s.Phone = "(412) 256 - 0900";
                s.Budget = 25000.0;

                ProspectMemory m = new ProspectMemory();
                m.Memento = s.SaveMemento();

                s.Name = "Leo Welch";
                s.Phone = "(310) 209-7111";
                s.Budget = 1000000.0;

                s.RestoreMemento(m.Memento);

                Console.ReadKey();
            }
        }

        class SalesProspect
        {
            string name;
            string phone;
            double budget;

            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                    Console.WriteLine("Name: " + name);
                }
            }

            public string Phone
            {
                get
                {
                    return phone;
                }
                set
                {
                    phone = value;
                    Console.WriteLine("Phone: " + phone);
                }
            }

            public double Budget
            {
                get
                {
                    return budget;
                }
                set
                {
                    budget = value;
                    Console.WriteLine("Budget: " + budget);
                }
            }

            public MementoReal SaveMemento()
            {
                Console.WriteLine("\nSaving state-----\n");
                return new MementoReal(name, phone, budget);
            }

            public void RestoreMemento(MementoReal memento)
            {
                Console.WriteLine("\nRestoring state --\n");
                this.Name = memento.Name;
                this.Phone = memento.Phone;
                this.Budget = memento.Budget;
            }
        }

        class MementoReal
        {
            string name;
            string phone;
            double budget;

            public MementoReal(string name, string phone, double budget)
            {
                this.name = name;
                this.phone = phone;
                this.budget = budget;
            }

            public string Name
            {
                get => name;
                set
                {
                    name = value;
                }
            }

            public string Phone
            {
                get => phone;
                set
                {
                    phone = value;
                }
            }

            public double Budget
            {
                get => budget;
                set
                {
                    budget = value;
                }
            }
        }

        class ProspectMemory
        {
            MementoReal memento;

            public MementoReal Memento
            {
                get => memento;
                set
                {
                    memento = value;
                }
            }
        }
        #endregion
    }
    //한 객체의 상태 편화에 따라 다른 객체의 상태도 연동되도록 일대 다 객체 의존관계를 구성
    namespace Observer
    {
        #region 구조
        /// <summary>
        /// Subject (Stock) : observer를 알고 있음, Observer객체는 여럿일 수도 있음 Observer 객체에 붙고 떨어지는 인터페이스를 제공
        /// ConcreteSubject (IBM) : ConcreteObserver와 연관된 상태를 저장, 관측 객체의 상태가 바뀌면 해당 observer에 그 상태를 알림
        /// Observer (IInvestor) : 오브젝트가 해당 상태가 바뀌었을 때 다른 곳으로 알릴 인터페이스
        /// ConcreteObserver ( Investor) : ConcreteSubject의 객체 참조를 가지고 있음, 객체의 상태를 저장하고 있음, 상태가 바뀌면 옵저버에 전달하게 될 인터페이스를 실행...
        /// </summary>
        class StructuralMain
        {
            static void Main()
            {
                ConcreteSubject s = new ConcreteSubject();

                s.Attach(new ConcreteObserver(s, "X"));
                s.Attach(new ConcreteObserver(s, "Y"));
                s.Attach(new ConcreteObserver(s, "Z"));

                s.SubjectState = "ABC";
                s.Notify();

                Console.ReadKey();
            }
        }

        abstract class Subject
        {
            List<Observer> observers = new List<Observer>();

            public void Attach(Observer observer)
            {
                observers.Add(observer);
            }

            public void Detach(Observer observer)
            {
                observers.Remove(observer);
            }

            public void Notify()
            {
                foreach(var o in observers)
                {
                    o.Update();
                }
            }
        }
        class ConcreteSubject : Subject
        {
            string subjectState;

            public string SubjectState
            {
                get => subjectState;
                set
                {
                    subjectState = value;
                }
            }
        }
        abstract class Observer
        {
            public abstract void Update();
        }

        class ConcreteObserver : Observer
        {
            string name;
            string observerState;
            ConcreteSubject subject;

            public ConcreteObserver(ConcreteSubject subject, string name)
            {
                this.subject = subject;
                this.name = name;
            }

            public override void Update()
            {
                observerState = subject.SubjectState;

                Console.WriteLine($"Observer {name}'s new state is {observerState}");
            }

            public ConcreteSubject Subject
            {
                get => subject;
                set
                {
                    subject = value;
                }
            }
        }

        #endregion 구조
        #region 실제 사례
        class RealMain
        {
            static void Main()
            {
                IBM ibm = new IBM("IBM", 120.00);
                ibm.Attach(new Investor("Sorros"));
                ibm.Attach(new Investor("Berkshire"));

                ibm.Price = 120.10;
                ibm.Price = 121.00;
                ibm.Price = 120.50;
                ibm.Price = 120.75;

                Console.ReadKey();
            }
        }

        abstract class Stock
        {
            string symbol;
            double price;
            List<IInvestor> investors = new List<IInvestor>();

            public Stock(string symbol, double price)
            {
                this.symbol = symbol;
                this.price = price;
            }

            public void Attach(IInvestor investor)
            {
                investors.Add(investor);
            }
            public void Detach(IInvestor investor)
            {
                investors.Remove(investor);
            }

            public void Notify()
            {
                foreach(var investor in investors)
                {
                    investor.Update(this);
                }
                Console.WriteLine("");
            }
            public double Price
            {
                get => price;
                set
                {
                    if(price != value)
                    {
                        price = value;
                        Notify();
                    }
                }
            }

            public string Symbol
            {
                get => symbol;
            }
        }
        class IBM : Stock
        {
            public IBM(string symbol, double price): base(symbol, price)
            {

            }
        }

        interface IInvestor
        {
            void Update(Stock stock);
        }

        class Investor : IInvestor
        {
            string name;
            Stock stock;
            public Investor(string name)
            {
                this.name = name;
            }

            public void Update(Stock stock)
            {
                Console.WriteLine($"Notified {name} of {stock.Symbol}'s change to {stock.Price:2}");
            }

            public Stock Stock
            {
                get => stock;
                set
                {
                    stock = value;
                }
            }
        }
        #endregion

    }
    //객체의 상태에 따라 객체의 행위 내용을 바꿔주는 패턴
    namespace State
    {
        #region 구조
        /// <summary>
        /// Context (Account): 클라이언트에서 활용될 인터페이스 정의, ConcreteState의 파생 클래스들의 객체의 현재 상태를 보관
        /// State (State) : Context의 특정 상황에 맞물리는 인터페이스 정의
        /// Concrete State (RedState, SilverState, GoldState) : Context의 상태와 연동되는 파생클래스
        /// </summary>
        class Structure
        {
            static void Main()
            {
                Context c = new Context(new ConcreteStateA());

                c.Request();
                c.Request();
                c.Request();
                c.Request();

                Console.ReadKey();
            }
        }

        abstract class State
        {
            public abstract void Handle(Context context);
        }

        class ConcreteStateA : State
        {
            public override void Handle(Context context)
            {
                context.State = new ConcreteStateB();
            }
        }

        class ConcreteStateB : State
        {
            public override void Handle(Context context)
            {
                context.State = new ConcreteStateA();
            }
        }

        class Context
        {
            State state;
            public Context(State state)
            {
                this.State = state;
            }
            public State State
            {
                get => state;
                set
                {
                    state = value;
                    Console.WriteLine("State: " + state.GetType().Name);
                }
            }
            public void Request()
            {
                state.Handle(this);
            }
        }
        #endregion
        #region 실제 사례
        class Real
        {
            static void Main()
            {
                Account account = new Account("Jim Johnson");

                account.Deposit(500.0);
                account.Deposit(300.0);
                account.Deposit(550.0);
                account.PayInterest();
                account.Withdraw(2000.00);
                account.Withdraw(1100.00);

                Console.ReadKey();
            }
        }

        abstract class StateReal
        {
            protected Account account;
            protected double balance;

            protected double interest;
            protected double lowerLimit;
            protected double upperLimit;

            public Account Account
            {
                get => account;
                set
                {
                    account = value;
                }
            }

            public double Balance
            {
                get => balance;
                set
                {
                    balance = value;
                }
            }

            public abstract void Deposit(double amount);
            public abstract void Withdraw(double amount);
            public abstract void PayInterest();
        }

        class RedState : StateReal
        {
            double serviceFee;

            public RedState(StateReal state)
            {
                this.balance = state.Balance;
                this.account = state.Account;
                Initialize();
            }
            void Initialize()
            {
                interest = 0.0;
                lowerLimit = -100.0;
                upperLimit = 0.0;
                serviceFee = 15.00;
            }

            public override void Deposit(double amount)
            {
                balance += amount;
                StateChangeCheck();
            }
            public override void Withdraw(double amount)
            {
                amount -= serviceFee;
                Console.WriteLine("No funds available for withdrawal");
            }

            public override void PayInterest()
            {
                //이자 없음..
            }

            void StateChangeCheck()
            {
                if (balance > upperLimit)
                {
                    account.State = new SilverState(this);
                }
            }
        }

        class SilverState : StateReal
        {
            public SilverState(StateReal state) : this(state.Balance, state.Account)
            {

            }
            public SilverState(double balance, Account account)
            {
                this.balance = balance;
                this.account = account;
                Initialize();
            }
            void Initialize()
            {
                interest = 0.0;
                lowerLimit = 0.0;
                upperLimit = 1000.0;
            }

            public override void Deposit(double amount)
            {
                balance += amount;
                StateChangeCheck();
            }

            public override void Withdraw(double amount)
            {
                balance -= amount;
                StateChangeCheck();
            }
            public override void PayInterest()
            {
                balance += interest * balance;
                StateChangeCheck();
            }

            void StateChangeCheck()
            {
                if (balance < lowerLimit)
                {
                    account.State = new RedState(this);
                }
                else if (balance > upperLimit)
                {
                    account.State = new GoldState(this);
                }
            }
        }

        class GoldState : StateReal
        {
            public GoldState(StateReal state):this(state.Balance, state.Account)
            {

            }
            public GoldState(double balance, Account account)
            {
                this.balance = balance;
                this.account = account;
                Initialize();
            }
            void Initialize()
            {
                interest = 0.05;
                lowerLimit = 1000.0;
                upperLimit = 100000000.0;
            }
            public override void Deposit(double amount)
            {
                balance += amount;
                StateChangeCheck();
            }
            public override void Withdraw(double amount)
            {
                balance -= amount;
                StateChangeCheck();
            }
            public override void PayInterest()
            {
                balance += interest * balance;
                StateChangeCheck();
            }

            void StateChangeCheck()
            {
                if (balance < 0.0)
                {
                    account.State = new RedState(this);
                }
                else if (balance < lowerLimit)
                {
                    account.State = new SilverState(this);
                }
            }
        }

        class Account
        {
            StateReal state;
            string owner;

            public Account(string owner)
            {
                this.owner = owner;
                this.state = new SilverState(0.0, this);
            }

            public double Balance
            {
                get => state.Balance;
            }
            public StateReal State
            {
                get => state;
                set
                {
                    state = value;
                }
            }
            public void Deposit(double amount)
            {
                state.Deposit(amount);
                Console.WriteLine($"Deposited {amount:C}");
                Console.WriteLine($"Balance = {Balance:C}");
                Console.WriteLine($"Status {State.GetType().Name}");
                Console.WriteLine("");
            }
            public void Withdraw(double amount)
            {
                state.Withdraw(amount);
                Console.WriteLine($"Withdraw {amount:C}");
                Console.WriteLine($"Balance {Balance:C}");
                Console.WriteLine($"Status {State.GetType().Name}\n");
            }
            public void PayInterest()
            {
                state.PayInterest();
                Console.WriteLine("Interest Paid--");
                Console.WriteLine($"Balance = {Balance:C}");
                Console.WriteLine($"Status = {State.GetType().Name}\n");
            }
        }
        #endregion
    }
    //행위를 클래스로 캡슐화 하여 동적으로 행위를 자유로이 바꿀 수 있도록 해주는 패턴
    //알고리즘 단위를 정의하고 각각을 내부를 감싸 교체 가능한 형태로 만들어주는 것, 알고리즘은 각각의 객체에 따라 서로 독립적으로 작동하게 됨
    namespace Strategy
    {
        #region 구조
        /// <summary>
        /// Strategy (SortStrategy) : 모든 알고리즘에 적용 가능한 인터페이스 정의, Context는 이 인터페이스를 이용하여 ConcreteStrategy 알고리즘을 호출함
        /// ConcreteStrategy (QuickSort, ShellSort, MergeSort) : Strategy 인터페이스를 구현한 알고리즘
        /// Context (SortedList) : ConcreteStrategy 객체를 구성, Strategy객체의 참조를 담고 있음, Strategy의 정보에 접근할 수 있는 인터페이스가 정의될 수도 있음
        /// </summary>
        class Structure
        {
            static void Main()
            {
                Context context;

                context = new Context(new ConcreteStrategyA());
                context.ContextInterface();

                context = new Context(new ConcreteStrategyB());
                context.ContextInterface();

                context = new Context(new ConcreteStrategyC());
                context.ContextInterface();

                Console.ReadKey();
            }
        }

        abstract class Strategy 
        {
            public abstract void AlgorithmInterface();
        }

        class ConcreteStrategyA : Strategy 
        {
            public override void AlgorithmInterface()
            {
                Console.WriteLine("Called ConcreteStrategyA.AlgorithmInterface()");
            }
        }

        class ConcreteStrategyB : Strategy
        {
            public override void AlgorithmInterface()
            {
                Console.WriteLine("Called ConcreteStrategyB.AlgorithmInterface()");
            }
        }

        class ConcreteStrategyC : Strategy 
        {
            public override void AlgorithmInterface() 
            {
                Console.WriteLine("Called ConcreteStrategyC.AlgorithmInterface()");
            }
        }

        class Context
        {
            Strategy strategy;

            public Context(Strategy strategy)
            {
                this.strategy = strategy;
            }
            public void ContextInterface()
            {
                strategy.AlgorithmInterface();
            }
        }
        #endregion
        #region 실제 사례
        class Real
        {
            static void Main() 
            {
                SortedListS studentRecords = new SortedListS();
                studentRecords.Add("Samuel");
                studentRecords.Add("Jimmy");
                studentRecords.Add("Sandra");
                studentRecords.Add("Vivek");
                studentRecords.Add("Anna");

                studentRecords.SetSortStrategy(new QuickSort());
                studentRecords.Sort();
                studentRecords.SetSortStrategy(new ShellSort());
                studentRecords.Sort();
                studentRecords.SetSortStrategy(new MergeSort());
                studentRecords.Sort();

                Console.ReadKey();
            }
        }
        abstract class SortStrategy
        {
            public abstract void Sort(List<string> list);
        }

        class QuickSort: SortStrategy
        {
            public override void Sort(List<string> list)
            {
                list.Sort();
                Console.WriteLine("QuickSorted list");
            }
        }
        class ShellSort: SortStrategy
        {
            public override void Sort(List<string> list)
            {
                //list.ShellSort();
                Console.WriteLine("ShellSorted list");
            }
        }
        class MergeSort : SortStrategy
        {
            public override void Sort(List<string> list)
            {
                //list.MergeSort();
                Console.WriteLine("MergeSorted list");
            }
        }

        class SortedListS
        {
            List<string> list = new List<string>();
            SortStrategy sortStrategy;

            public void SetSortStrategy(SortStrategy sortStrategy)
            {
                this.sortStrategy = sortStrategy;
            }

            public void Add(string name)
            {
                list.Add(name);
            }
            public void Sort()
            {
                sortStrategy.Sort(list);

                foreach(var name in list)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
    //작업을 처리하는 일부분을 서브 클래스로 캡슐화 하여 전체 일 수행 구조는 바꾸지 않고 특정 단계 수행내용을 바꾸는 패턴
    namespace TemplateMethod
    {
        #region 구조
        /// <summary>
        /// AbstractClass (DataObject) : 알고리즘의 특정 단계를 담당하는 단위 연산자가 정의됨, 알고리즘의 구조가 정의된 기본 형태 메소드를 구현, 기본형태 메소드는 단위연산자(AbstractClass나 해당 객체)를 부름
        /// ConcreteClass(CustomerDataObject) : 알고리즘의 특정 단계를 담당하는 단위연산자 구현
        /// </summary>
        class Structure
        {
            static void Main()
            {
                AbstractClass aA = new ConcreteClassA();
                aA.TemplateMethod();

                AbstractClass aB = new ConcreteClassB();
                aB.TemplateMethod();

                Console.ReadKey();
            }
        }

        abstract class AbstractClass
        {
            public abstract void PrimitiveOperation1();
            public abstract void PrimitiveOperation2();

            public void TemplateMethod()
            {
                PrimitiveOperation1();
                PrimitiveOperation2();
                Console.WriteLine("");
            }
        }

        class ConcreteClassA : AbstractClass
        {
            public override void PrimitiveOperation1()
            {
                Console.WriteLine("ConcreteClassA.PrimitiveOperation1()");
            }
            public override void PrimitiveOperation2()
            {
                Console.WriteLine("ConcreteClassA.PrimitiveOperation2()");
            }
        }

        class ConcreteClassB : AbstractClass
        {
            public override void PrimitiveOperation1()
            {
                Console.WriteLine("ConcreteClassB.PrimitiveOperation1()");
            }

            public override void PrimitiveOperation2()
            {
                Console.WriteLine("ConcreteClassB.PrimitiveOperation2()");
            }
        }

        #endregion
        #region 실제 사례
        class Real
        {
            static void Main()
            {
                DataAccessObject daoCategories = new Categories();
                daoCategories.Run();

                DataAccessObject daoProducts = new Products();
                daoProducts.Run();

                Console.ReadKey();
            }
        }

        abstract class DataAccessObject
        {
            protected string connectionString;
            protected DataSet dataset;
            public virtual void Connect()
            {
                connectionString = "provider-Microsoft.JET.OLEDB.4.0; " + "data source=..\\..\\..\\db1.mdb";
            }
            public abstract void Select();
            public abstract void Process();

            public virtual void Disconnect()
            {
                connectionString = "";
            }

            public void Run()
            {
                Connect();
                Select();
                Process();
                Disconnect();
            }
        }

        class Categories : DataAccessObject
        {
            public override void Select()
            {
                string sql = "select CategoryName from Categories";
                DataAdapter dataAdapter = new();
                
                dataset = new DataSet();
                dataAdapter.Fill(dataset);
            }
            public override void Process()
            {
                Console.WriteLine("Categories ----");

                DataTable dataTable = dataset.Tables["Categories"];
                foreach(DataRow row in dataTable.Rows)
                {
                    Console.WriteLine(row["CategoryName"]);
                }
                Console.WriteLine();
            }
        }
        class Products : DataAccessObject
        {
            public override void Select()
            {
                string sql = "select CategoryName from Products";
                DataAdapter dataAdapter = new(sql, dataset);

                dataset = new DataSet();
                dataAdapter.Fill(dataset);
            }
            public override void Process()
            {
                Console.WriteLine("Products ----");

                DataTable dataTable = dataset.Tables["Products"];
                foreach (DataRow row in dataTable.Rows)
                {
                    Console.WriteLine(row["CategoryName"]);
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
    //행위의 재현을 오브젝트 구조를 이루는 원소에서 실행함 Visitor는 기존의 행동에 아무런 영향없이 새 행동을 정의할 수 있음
    namespace Visitor
    {
        #region 구조
        /// <summary>
        /// Visitor (Visitor) : 각 ConcreteElement클래스의 오브젝트 구조에서 Visit행동이 정의됨, visitor에서 요구하는 Visit 함수의 이름과 파라미터 등은 모두 동일함
        /// visitor는 각 객체의 성분에 인터페이스를 통해 접근 가능함
        /// ConcreteVisitor (IncomeVisitor, VacationVisitor) : Visitor에서 정의된 인터페이스 행동들 정의, 각각의 행동들은 알고리즘에서 정의된 일부분이고 해당 클래스나 객체 구조에서 결정된 것
        /// ConcreteVisitor는 알고리즘의 내용과 해당 상태등을 저장하고 있음, 해당 상태등은 구조상의 형태에서 특정 결과를 이끌어냄
        /// Element (Element) : visitor의 요청을 수락하는 행동이 지정됨
        /// ConcretElement (Employee) : visitor의 요청을 수락하는 행동의 내용이 구성됨
        /// ObjectStructure (Employees) : 각각의 요소들에 접근 가능, visitor가 각 요소들에 접근할 수 있는 인터페이스가 제공될 수 있음, Composite 패턴이나 list같은 것들로 구성될 수 있음
        /// </summary>
        class Structure
        {
            static void Main()
            {
                ObjectStructure o = new ObjectStructure();
                o.Attach(new ConcreteElementA());
                o.Attach(new ConcreteElementB());

                ConcreteVisitor1 v1 = new ConcreteVisitor1();
                ConcreteVisitor2 v2 = new ConcreteVisitor2();

                o.Accept(v1);
                o.Accept(v2);

                Console.ReadKey();
            }
        }

        interface IVisitor
        {
            void VisitConcreteElementA(ConcreteElementA concreteElementA);
            void VisitConcreteElementB(ConcreteElementB concreteElementB);
        }

        class ConcreteVisitor1 : IVisitor
        {
            public void VisitConcreteElementA(ConcreteElementA concreteElementA)
            {
                Console.WriteLine($"{concreteElementA.GetType().Name} visited by {this.GetType().Name}");
            }
            public void VisitConcreteElementB(ConcreteElementB concreteElementB)
            {
                Console.WriteLine($"{concreteElementB.GetType().Name} visited by {this.GetType().Name}");
            }
        }

        class ConcreteVisitor2 : IVisitor
        {
            public void VisitConcreteElementA(ConcreteElementA concreteElementA)
            {
                Console.WriteLine($"{concreteElementA.GetType().Name} visited by {this.GetType().Name}");
            }
            public void VisitConcreteElementB(ConcreteElementB concreteElementB)
            {
                Console.WriteLine($"{concreteElementB.GetType().Name} visited by {this.GetType().Name}");
            }
        }

        interface IElement
        {
            void Accept(IVisitor visitor);
        }

        class ConcreteElementA : IElement
        {
            public void Accept(IVisitor visitor)
            {
                visitor.VisitConcreteElementA(this);
            }
            public void OperationA()
            {

            }
        }
        class ConcreteElementB : IElement
        {
            public void Accept(IVisitor visitor)
            {
                visitor.VisitConcreteElementB(this);
            }
            public void OperationB()
            {

            }
        }

        class ObjectStructure
        {
            List<IElement> elements = new List<IElement>();

            public void Attach(IElement element)
            {
                elements.Add(element);
            }
            public void Detach(IElement element)
            {
                elements.Remove(element);
            }

            public void Accept(IVisitor visitor)
            {
                foreach(var element in elements)
                {
                    element.Accept(visitor);
                }
            }
        }

        #endregion
        #region 실제 사례
        class Real
        {
            static void Main()
            {
                Employees e = new Employees();
                e.Attach(new Clerk());
                e.Attach(new Director());
                e.Attach(new President());

                e.Accept(new IncomeVisitor());
                e.Accept(new VacationVisitor());

                Console.ReadKey();
            }
        }

        interface IVisit
        {
            void Visit(IElem element);
        }

        class IncomeVisitor : IVisit
        {
            public void Visit(IElem element)
            {
                Employee employee = element as Employee;

                employee.Income *= 1.10;
                Console.WriteLine($"{employee.GetType().Name} {employee.Name}'s new income :{employee.Income:C}");
            }
        }
        class VacationVisitor : IVisit
        {
            public void Visit(IElem element)
            {
                Employee employee = element as Employee;

                employee.VacationDays += 3;
                Console.WriteLine($"{employee.GetType().Name} {employee.Name}'s new vacation days : {employee.VacationDays}");
            }
        }

        interface IElem
        {
            void Accept(IVisit visitor);
        }
        class Employee : IElem
        {
            string name;
            double income;
            int vacationdays;

            public Employee(string name, double income, int vacationdays)
            {
                this.name = name;
                this.income = income;
                this.vacationdays = vacationdays;
            }

            public string Name
            {
                get => name;
                set
                {
                    name = value;
                }
            }
            public double Income
            {
                get => income;
                set
                {
                    income = value;
                }
            }
            public int VacationDays
            {
                get => vacationdays;
                set
                {
                    vacationdays = value;
                }
            }
            public void Accept(IVisit visitor)
            {
                visitor.Visit(this);
            }
        }

        class Employees
        {
            List<Employee> employees = new List<Employee>();

            public void Attach(Employee employee)
            {
                employees.Add(employee);
            }
            public void Detach(Employee employee)
            {
                employees.Remove(employee);
            }
            public void Accept(IVisit visitor)
            {
                foreach(Employee e in employees)
                {
                    e.Accept(visitor);
                }
                Console.WriteLine();
            }
        }

        class Clerk: Employee
        {
            public Clerk() : base ("Hank", 25000.0, 14)
            {

            }
        }
        class Director : Employee
        {
            public Director() : base("Elly", 35000.0, 16)
            {

            }
        }
        class President : Employee
        {
            public President(): base("Dick", 45000.0, 21)
            {

            }
        }
        #endregion
    }
}
