using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.Domain
{
    public class DomainObject
    {
        private List<BusinessRule> _brokenRules = new List<BusinessRule>();

        public DomainObject() { }

        protected void Validate() { }

        public void AddBrokenRule(BusinessRule rule)
        {
            _brokenRules.Add(rule);
        }

        public List<BusinessRule> GetBrokenRules()
        {
            _brokenRules.Clear();
            Validate();
            return _brokenRules;
        }

        public void ThrowExceptionIfInvalid()
        {
            GetBrokenRules();
            if (_brokenRules.Count > 0)
            {
                var issues = new StringBuilder();
                foreach (var rule in _brokenRules)
                {
                    issues.AppendLine(rule.ToString());
                }
            }
        }
    }
}
