using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApplication2.DB;

namespace WebApplication2.Models
{

    public class SchoolModel
    {
        public StudentModel Student { get; set; }
        
        public ClassModel Class { get; set; }
        public SubjectModel Sub { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage="Email Required") ]
        
        public string Email   { get; set; }

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }
    public class RoleModel
    {
        public string RoleName { get; set; }

        public List<Role>RoleList { get; set; }

    }
    public class StudentModel
    {
        public int AdmissionNumber { get; set; }
        public string StudentName { get; set; }

        public string FatherName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Dateofbirth { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public byte[] Photo { get; set; }
        public int classes { get; set; }
        public DateTime Dateofjoine { get; set; }

        public IEnumerable<SelectListItem> classlist { get; set; }
        public string AcadamicYear { get; set; }
        public List<student> StudentList { get; set; }

        public int Roles { get; set; }
        public IEnumerable<SelectListItem> Rolelist { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class TeacherModel
    {
        
        public string TeacherName { get; set; }

        public string FatherName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public byte[] Photo { get; set; }
      
        public DateTime Dateofjoine { get; set; }

        public List<student> StudentList { get; set; }

        public int Roles { get; set; }
        public IEnumerable<SelectListItem> Rolelist { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class ClassModel
    {

        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public IEnumerable<SelectListItem> Teachers { get; set; }

    }
    public class TeacherMappingModel
    {

        public string TeacherName { get; set; }
        public string Subject { get; set; }

    }
    public class FeeMappingModel
    {

        public int AdmissionNumber { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public int Class { get; set; }
        public int Schoolfee { get; set; }
        public string Busfee { get; set; }

    }

    public class PaymentModel
    {
        public int AdmissionNumber { get; set; }
        public string StundentName { get; set; }
        public int Class { get; set; }
        public string Paymenttype { get; set; }
        public string Dateofpayment { get; set; }
        public string FeeType { get; set; }
        public int Totalfee { get; set; }
        public int Pending { get; set; }
        public int paying { get; set; }
    }
    public class AttandanceModel
    {
        public List<string> StudentName { get; set; }
        public int Classes { get; set; }
        public IEnumerable<SelectListItem> classlist { get; set; }

        public DateTime Dateofattadance { get; set; }
        public List<string> Attandance { get; set; }

    }
    public class SubjectModel
    {
        public int Subid { get; set; }
        public string SubjectName { get; set; }
        public int MinMarks { get; set; }
        public int MaxMarks { get; set; }
        

        public List<subjectt> subjectlist { get; set; }
    }
    public class MappingModel
    {
        public int exam { get; set; }
        public int subject { get; set; }
        public List<int> Classes { get; set; }
        public IEnumerable<SelectListItem> classlist { get; set; }

        public IEnumerable<SelectListItem> subjectlist { get; set; }

        public IEnumerable<SelectListItem> examlist { get; set; }
        public List<subjectmapping> SMlist { get; set; }
        public List<Exammapping> EMlist { get; set; }
    }
        public class ExamsModel
    {
     
        public string ExamName { get; set; }
       

        
        public DateTime ExamDate { get; set; }
        public List<exam> Examlist { get; set; }
    }
    public class MarksModel
    {
        public int Mid { get; set; }

        public int AdmissionNum { get; set; }
        public int Examid { get; set; }
        public List<int> Subjectid { get; set; }
        public List<int> Subjectmarks { get; set; }
        public int Classes { get; set; }
        public IEnumerable<SelectListItem> classlist { get; set; }

        public IEnumerable<SelectListItem> examlist { get; set; }

        public bool Isprasent { get; set; }

        public DateTime Date { get; set; }
    }
    public class ReportsModel
    {
        public int AddmissionNum { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }
        public string Exam { get; set; }

        public int Exams { get; set; }
        public int Classes { get; set; }
        public IEnumerable<SelectListItem> classlist { get; set; }

        public IEnumerable<SelectListItem> examlist { get; set; }
        public List<Mark> Marklist { get; set; }
        public int TotalMarks { get; set; }
        public int Percentage { get; set; }
        public String status { get; set; }
    }

    public class Quiz
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAns { get; set; }
        public int TotalMarks { get; set; }
        
        public List<string> ans { get; set; }
    

        public List<Quiz> Quizzes;
    }

}

