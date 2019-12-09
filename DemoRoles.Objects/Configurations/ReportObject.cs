using System.Collections.Generic;

namespace DemoRoles.Objects.Configurations
{
    public class ReportObject
    {

        public ReportObject(
            int id,
            string name)
        {
            Id = id;
            Name = name;
        }

        public ReportObject(
            int id,
            string name,
            string role) : this(id, name)
        {
            Roles = new List<string>{role};
        }

        public ReportObject(
            int id,
            string name,
            List<string> roles) : this(id, name)
        {
            Roles = roles;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
    }
}
