using System;
using System.Drawing;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;
namespace GymAndFitnessManagementSystem.Forms
{
 public class LoginForm:Form
 {
  TextBox id=new TextBox(),pw=new TextBox(); Label msg=new Label();
  public LoginForm(){Theme.Apply(this,"Login");Size=new Size(760,560);Controls.Add(Theme.Header("Gym and Fitness Management System"));var box=new Panel{Width=440,Height=360,BackColor=Color.White,Left=150,Top=115,Padding=new Padding(45)};Controls.Add(box);int y=15;box.Controls.Add(new Label{Text="Sign in",Font=new Font("Segoe UI Semibold",20,FontStyle.Bold),Left=45,Top=y,Width=300,Height=40,ForeColor=Theme.Primary});y+=65;box.Controls.Add(new Label{Text="User ID or Email",Left=45,Top=y,Width=300});y+=25;id.SetBounds(45,y,330,32);box.Controls.Add(id);y+=52;box.Controls.Add(new Label{Text="Password",Left=45,Top=y,Width=300});y+=25;pw.SetBounds(45,y,330,32);pw.UseSystemPasswordChar=true;box.Controls.Add(pw);y+=48;msg.SetBounds(45,y,330,24);msg.ForeColor=Theme.Danger;box.Controls.Add(msg);y+=35;var b=Theme.Button("Login",Login);b.SetBounds(45,y,155,42);box.Controls.Add(b);var s=Theme.Button("Create Account",(o,e)=>{new SignUpForm().ShowDialog();});s.SetBounds(220,y,155,42);box.Controls.Add(s);var demo=new Label{Text="Demo: admin@gym.com / admin123  |  owner1@gym.com / owner123  |  customer1@gym.com / cust123",Left=20,Top=485,Width=710,Height=35,TextAlign=ContentAlignment.MiddleCenter,ForeColor=Color.DimGray};Controls.Add(demo);}
  void Login(object o,EventArgs e){try{if(string.IsNullOrWhiteSpace(id.Text)||string.IsNullOrWhiteSpace(pw.Text)){msg.Text="User ID/email and password are required.";return;}var r=Repositories.Login(id.Text.Trim(),pw.Text);if(r==null){msg.Text="Invalid credentials or inactive account.";return;}Session.UserId=Convert.ToInt32(r["UserId"]);Session.FullName=r["FullName"].ToString();Session.UserType=r["UserType"].ToString();Session.GymId=r["GymId"]==DBNull.Value?(int?)null:Convert.ToInt32(r["GymId"]);Hide();Form f=Session.UserType=="SuperAdmin"?(Form)new SuperAdminDashboardForm():
                    
                    
                    Session.UserType=="Admin"?(Form)new AdminDashboardForm():(Form)new CustomerDashboardForm();f.FormClosed+=(a,b)=>{Session.Clear();Show();};f.Show();}catch(Exception ex){msg.Text=ex.Message;}}
 }
}
