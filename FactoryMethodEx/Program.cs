using System;
using System.Collections.Generic;
//생성 패턴: 오브젝트 생성 인터페이스 정의, 하단 클래스는 어느 클래스를 생성할지 결정... 

namespace CreationPattern.FactoryMethodEx
{
    #region 구조
    /// <summary>
    /// Product(Page): FactoryMethod로 만들어질 객체의 인터페이스 정의
    /// ConcreteProduct(SkillPage, EducationPage, ExperiencePage): 생성 객체 인터페이스 적용
    /// Creator(Document): 어느 타입의 생성 객체를 반환하는지를 결정하는 FactoryMethod정의, 기본 FactoryMethod를 정의하여 기본 객체 생성 방식을 지정할 수 있음
    /// ConcreteCreator(Report, Resume) : FactoryMethod를 덮어쓰고 실제 객체를 생성
    /// </summary>

    class FactoryMethodStructuralMain
    {
        static void Main()
        {

            Creator[] creators = new Creator[2];
            creators[0] = new ConcreteCreatorA();
            creators[1] = new ConcreteCreatorB();

            foreach (var creator in creators)
            {
                Product product = creator.FactoryMethod();
                Console.WriteLine($"{product.GetType().Name}생성됨 ");
            }

            Console.ReadKey();
        }
    }

    abstract class Product
    {

    }

    class ConcreteProductA : Product
    {

    }
    class ConcreteProductB : Product
    {

    }

    abstract class Creator
    {
        public abstract Product FactoryMethod();
    }

    class ConcreteCreatorA : Creator
    {
        public override Product FactoryMethod()
        {
            return new ConcreteProductA();
        }
    }

    class ConcreteCreatorB : Creator
    {
        public override Product FactoryMethod()
        {
            return new ConcreteProductB();
        }
    }


    #endregion
    #region 실제 예시
    class FactoryMethodRealMain
    {
        static void Main()
        {
            Document[] documents = new Document[2];

            documents[0] = new Resume();
            documents[1] = new Report();

            foreach (var document in documents)
            {
                Console.WriteLine("\n" + document.GetType().Name + "---");
                foreach (var page in document.Pages)
                {
                    Console.WriteLine(" " + page.GetType().Name);
                }
            }

            Console.ReadKey();
        }
    }

    abstract class Page
    {

    }

    class SkillsPage : Page
    {

    }

    class EducationPage : Page
    {

    }

    class ExperiencePage : Page
    {

    }
    class IntroductionPage : Page
    {

    }

    class ResultsPage : Page
    {

    }

    class ConclusionPage : Page
    {

    }
    class SummaryPage : Page
    {

    }

    class BibliographyPage : Page
    {

    }

    abstract class Document
    {
        List<Page> _pages = new List<Page>();

        public Document()
        {
            this.CreatePages();
        }

        public List<Page> Pages
        {
            get { return _pages; }
        }

        public abstract void CreatePages();
    }

    class Resume : Document
    {
        public override void CreatePages()
        {
            Pages.Add(new SkillsPage());
            Pages.Add(new EducationPage());
            Pages.Add(new ExperiencePage());
        }
    }

    class Report : Document
    {
        public override void CreatePages()
        {
            Pages.Add(new IntroductionPage());
            Pages.Add(new ResultsPage());
            Pages.Add(new ConclusionPage());
            Pages.Add(new SummaryPage());
            Pages.Add(new BibliographyPage());
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

