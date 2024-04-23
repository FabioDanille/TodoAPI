using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoModel
    {
        [Key]
        public int Id { get; set;}
        public string Title { get; set;}
        public bool Done { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now.ToLocalTime();
        public DateTime DateUpdate { get; set; } = DateTime.Now.ToLocalTime();
    }
}