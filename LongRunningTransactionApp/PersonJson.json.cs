using Starcounter;

namespace LongRunningTransactionApp
{
    partial class PersonJson : Json
    {
        public string FullName => $"{FirstName} {LastName}";

        void Handle(Input.SaveTrigger action)
        {
            AttachedScope.Commit();
        }
    }
}
