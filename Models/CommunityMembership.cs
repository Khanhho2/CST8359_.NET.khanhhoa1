using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A1.Models
{
    public class CommunityMembership
    {
        public int StudentID { get; set; }
        public string CommunityID { get; set; }

        public Student Student { get; set; }
        public Community Community { get; set; }
    }
}
