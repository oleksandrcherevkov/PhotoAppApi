using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.EF.Models.Base
{
    public class PhotoBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public byte[] Data { get; set; }
        public byte[] CompressedData { get; set; }
    }
}
