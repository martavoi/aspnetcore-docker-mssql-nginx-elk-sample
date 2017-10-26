using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oxagile.Internal.Api.Entities
{
    public class Company : Entity
    {
        public string Name { get; set; }
        [ForeignKey("CompanyId")]
        public ICollection<User> Users { get; set; }
    }
}