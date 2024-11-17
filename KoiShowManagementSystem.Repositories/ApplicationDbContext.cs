namespace KoiShowManagementSystem.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Scores> Scores { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<Koi> Kois { get; set; }
        public DbSet<JudgeAssignments> JudgeAssignments { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Event_Koi_Participation> Event_Koi_Participations { get; set; }
    }
}
