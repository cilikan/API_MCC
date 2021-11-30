using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_m_university")]

    public class University
    {
        [Key]
        //nama universityid hrs sama dengan universityid di tabel education
        public int UniversityId { get; set; }
        public string Name { get; set; }
        //penghubung one to many dengan tabel education
        public virtual ICollection<Education> Educations { get; set; }
    }
}
