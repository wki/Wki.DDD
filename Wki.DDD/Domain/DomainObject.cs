using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.Domain
{
    public class DomainObject
    {
        private List<BusinessRule> brokenRules = new List<BusinessRule>();

        public DomainObject() { }

        protected void Validate() { }

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
                foreach (var rule in brokenRules)
                {
                    issues.AppendLine(rule.ToString());
                }
            }
        }
    }
}
