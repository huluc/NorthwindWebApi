using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NorthwindWebApi.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        [MaxLength(350)]
        public string LogCaption { get; set; }
        public string LogDetail { get; set; }
        public bool IsBefore { get; set; }
    }
}