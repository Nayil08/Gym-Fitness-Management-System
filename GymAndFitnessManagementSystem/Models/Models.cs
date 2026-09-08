namespace GymAndFitnessManagementSystem.Models
{
    public abstract class User
    {
        public int UserId { get; set; } public string FullName { get; set; } public string Email { get; set; } public string UserType { get; protected set; }
        public abstract string DashboardTitle();
    }
    public sealed class SuperAdmin : User { public SuperAdmin(){UserType="SuperAdmin";} public override string DashboardTitle()=>"Platform Control Center"; }
    public sealed class Admin : User { public int GymId { get; set; } public Admin(){UserType="Admin";} public override string DashboardTitle()=>"Gym Owner Dashboard"; }
    public sealed class Customer : User { public Customer(){UserType="Customer";} public override string DashboardTitle()=>"Customer Dashboard"; }
    public class Gym { public int GymId {get;set;} public int OwnerUserId{get;set;} public string GymName{get;set;} public string Status{get;set;} }
    public class Trainer { public int TrainerId{get;set;} public int GymId{get;set;} public string TrainerName{get;set;} public string Specialization{get;set;} }
    public class Category { public int CategoryId{get;set;} public string CategoryName{get;set;} }
    public class FitnessItem { public int ItemId{get;set;} public int GymId{get;set;} public string ItemName{get;set;} public string ItemType{get;set;} public decimal Price{get;set;} public int StockQty{get;set;} }
    public class Order { public int OrderId{get;set;} public int CustomerUserId{get;set;} public decimal TotalAmount{get;set;} public string Status{get;set;} }
    public class Review { public int ReviewId{get;set;} public int Rating{get;set;} public string Comment{get;set;} }
}
