using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oxagile.Demos.Data.Entities
{
    public enum MediaRelationType
    {
        UserPic,
        Unspecified
    }

    public class UserMedia : Entity
    {
        public string BlobPath { get; set; }
        public string Extension { get; set; }
        public MediaRelationType Rel { get; set; }
        public DateTime Uploaded { get; set; }
        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }   
}