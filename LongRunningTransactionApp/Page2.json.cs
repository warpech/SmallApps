using Starcounter;

namespace LongRunningTransactionApp
{
    partial class Page2 : Json
    {
        public string FullName => $"{FirstName} {LastName}";

        void Handle(Input.SaveTrigger action)
        {
            AttachedScope.Commit();
        }
    }
}
