namespace BeautySalon.Contracts
{
    public class CatalogEditVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public byte[]? Photo { get; set; }
    }
}
