using System;
using System.Linq;
using Starcounter;

namespace LongRunningTransactionApp
{
    [Database]
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }   

    class Program
    {
        static void Main()
        {
            Db.Transact(() =>
            {
                var person = Db.SQL<Person>("SELECT p FROM Person p").FirstOrDefault();
                if (person == null)
                {
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };
                }
            });

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());
            
            Handle.GET("/LongRunningTransactionApp", () =>
            {
                return Db.Scope(() =>
                {
                    Session.Ensure();
                    var person = Db.SQL<Person>("SELECT p FROM Person p")
                        .FirstOrDefault();
                    return new PersonJson { Data = person };
                });
            });

            Handle.GET("/page2", () =>
            {
                return Db.Scope(() =>
                {
                    Session.Ensure();
                    var person = Db.SQL<Person>("SELECT p FROM Person p")
                        .FirstOrDefault();
                    return new Page2() { Data = person };
                });
            });
        }
    }
}