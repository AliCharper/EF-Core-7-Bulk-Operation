namespace EFBulkActivities_V2.Entities
{
    public class UniversityClass
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime LastActivitytime { get; set; }
        public List<Student> Students { get; set; }
    }
}
