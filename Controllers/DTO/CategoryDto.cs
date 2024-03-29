namespace InterviewPuzzle.Controllers.DTO
{
    public class CategoryDto
    {
        public CategoryDto()
        {
            
        }
        public CategoryDto(string Name, int Count)
        {
            this.Name = Name;
            this.Count = Count;
        }
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
