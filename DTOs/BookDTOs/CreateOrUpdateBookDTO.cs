namespace SimpleBookAPI.DTOs.BookDTOs
{
    public class CreateOrUpdateBookDTO
    {
        public string? Title { get; set; }
        public string? Langueage { get; set; }
        public int PageCount { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public int AuthorId { get; set; }
    }
}
