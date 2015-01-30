using System.Collections.Generic;
using System.Text;

namespace Wki.DDD.Domain
{
    public class DomainObject
    {
        private List<BusinessRule> brokenRules = new List<BusinessRule>();

        public DomainObject() { }

        protected virtual void Validate() { }

        public void AddBrokenRule(BusinessRule rule)
        {
            brokenRules.Add(rule);
        }

        public List<BusinessRule> GetBrokenRules()
        {
            brokenRules.Clear();
            Validate();
            return brokenRules;
        }

        public void ThrowExceptionIfInvalid()
        {
            GetBrokenRules();
            if (brokenRules.Count > 0)
            {
                var issues = new StringBuilder();
                brokenRules
                    .ForEach(r => issues.AppendLine(r.ToString()));

                throw new ObjectIsInvalidException(issues.ToString());
            }
        }
    }
}
