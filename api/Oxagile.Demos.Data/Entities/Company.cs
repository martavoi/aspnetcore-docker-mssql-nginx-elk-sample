using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oxagile.Demos.Data.Entities
{
    public class Company : Entity
    {
        public string Name { get; set; }
        [InverseProperty("Company")]
        public ICollection<User> Users { get; set; }
    }
}