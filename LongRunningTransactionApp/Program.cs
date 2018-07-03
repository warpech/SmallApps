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
                    
                    var personJson = Session.Current.Store["PersonJson"] as PersonJson;
                    if (personJson == null)
                    {
                        personJson = new PersonJson {Data = person};
                        Session.Current.Store["PersonJson"] = personJson;
                        return personJson;
                    }

                    return personJson;
                });
            });

            Handle.GET("/page2", () =>
            {
                return Db.Scope(() =>
                {
                    Session.Ensure();
                    
                    var person = Db.SQL<Person>("SELECT p FROM Person p")
                        .FirstOrDefault();
                    
                    var page2 = Session.Current.Store["Page2"] as Page2;
                    if (page2 == null)
                    {
                        page2 = new Page2 { Data = person };
                        Session.Current.Store["Page2"] = page2;
                        return page2;
                    }

                    return page2; ;
                });
            });
        }
    }
}