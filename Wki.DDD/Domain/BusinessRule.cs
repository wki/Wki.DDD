namespace Wki.DDD.Domain
{
    public class BusinessRule
    {
        public string Description { get; private set; }

        public BusinessRule(string description)
        {
            this.Description = description;
        }
    }
}
