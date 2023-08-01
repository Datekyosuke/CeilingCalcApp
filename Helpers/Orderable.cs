namespace WebApiDB.Helpers
{
    public class Orderable
    {
        public string Property { get; init; } = "Id";

        public Sort Sort { get; init; } = Sort.Asc;
    }
}
