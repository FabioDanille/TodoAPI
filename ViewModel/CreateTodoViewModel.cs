using System.ComponentModel.DataAnnotations;

namespace TodoAPI.ViewModel
{
    public class CreateTodoViewModel
    {
        [Required]
        public string Title { get; set;}
        public bool Done { get; set; }
    }
}