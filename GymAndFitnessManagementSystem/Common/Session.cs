namespace GymAndFitnessManagementSystem.Common
{
    public static class Session
    {
        public static int UserId { get; set; }
        public static string FullName { get; set; }
        public static string UserType { get; set; }
        public static int? GymId { get; set; }
        public static void Clear() { UserId = 0; FullName = null; UserType = null; GymId = null; }
    }
}
