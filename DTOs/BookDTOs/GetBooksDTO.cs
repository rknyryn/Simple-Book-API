namespace SimpleBookAPI.DTOs.BookDTOs
{
    public class GetBooksDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
    }
}
