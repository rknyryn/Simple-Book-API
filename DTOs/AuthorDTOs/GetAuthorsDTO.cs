namespace SimpleBookAPI.DTOs.AuthorDTOs
{
    public class GetAuthorsDTO
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
    }
}
