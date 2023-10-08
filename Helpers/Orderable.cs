namespace WebApiDB.Helpers
{
    public class Orderable
    {
        public string Property { get; init; } = "Id";

        public string? Sort { get; set; }
    }
}
