
namespace SimpleBookAPI.DTOs.BookDTOs
{
    public class GetBookDetailDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Language { get; set; }
        public int PageCount { get; set; }
        public string? AuthorName { get; set; }
        public string? CategoryName { get; set; }
        public string? PublisherName { get; set; }
    }
}
