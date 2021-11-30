using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_m_education")]
    public class Education
    {
        [Key]
        //nama educationid hrs sama dengan educationid di tabel profiling
        public int EducationId { get; set; }
        public string Degree { get; set; }
        public string GPA { get; set; }
        //nama universityid hrs sama dengan universityid di tabel university
        public int UniversityId { get; set; }
        //penghubung one to many dengan tabel profiling
        public virtual ICollection<Profiling> Profilings { get; set; }
        //penghubung many to one dengan tabel university
        public virtual University University { get; set; }
    }
}
