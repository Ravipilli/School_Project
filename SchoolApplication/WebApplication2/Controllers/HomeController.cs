
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using WebApplication2.DB;
using WebApplication2.Models;



namespace WebApplication2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        schoolEntities3 db = new schoolEntities3();
        [AllowAnonymous]
        public ActionResult AddRoles()
        {
            RoleModel RM = new RoleModel();
            RM.RoleList = db.Roles.ToList();

            return View(RM);
        }


        [HttpPost]
        public ActionResult AddRoles(RoleModel RM)
        {

            Role R = new Role();

            R.RName = RM.RoleName;
            db.Roles.Add(R);
            db.SaveChanges();
            TempData["RoleCreated"] = true;
            return RedirectToAction("AddRoles");

        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
       [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel L)
        {
            if (ModelState.IsValid)
            {

                var f = db.credintials.Where(x => x.Email == L.Email && x.Password == L.Password).SingleOrDefault();


                if (f != null)
                {
                    FormsAuthentication.SetAuthCookie(f.Email, false);

                    Session["Role"] = f.Role.RName.ToString();


                    return RedirectToAction("StudentList");

                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(L);
                

            }
            return View(L);
           
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
        [Authorize]
        public ActionResult studentcreate()
        {
            StudentModel e = new StudentModel();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            e.classlist = d.GetClasses();
            e.Rolelist = d.GetRoles();
            return View(e);
        }
        
        [HttpPost]
        public ActionResult studentcreate(StudentModel s, HttpPostedFileBase file,int? id)
        {

         
            Dropdowns.Dropdowns1 w = new Dropdowns.Dropdowns1();
            s.classlist = w.GetClasses();
            if (file != null)
            {

                s.Photo = new byte[file.ContentLength];
                file.InputStream.Read(s.Photo, 0, file.ContentLength);

               }


            student d = new student();

            d.studentname = s.StudentName;

            d.phonenumber = s.PhoneNumber;
            d.gender = s.Gender;
            d.adress = s.Address;
            d.dateofbirth = s.Dateofbirth;
            d.photo = s.Photo;
            d.fathername = s.FatherName;
            d.classid = s.classes;
            d.dateofjoining = s.Dateofjoine;
            d.acadamicyear = s.AcadamicYear;
            d.isactive = true;
            d.Email = s.Email;
            d.Password = s.Password;
            d.rid = s.Roles;
            db.students.Add(d);
            db.SaveChanges();
            var y = db.students.OrderByDescending(x => x.admissionnum).FirstOrDefault();
            TempData["AdmissionNumber"] = y.admissionnum;
            return RedirectToAction("studentcreate");
        }
        [Authorize(Roles = "Teacher")]
        public ActionResult Details(int?id)
        {

            if (id != null)
            {
                var feemap = db.feemappings.Where(x => x.admissionnum == id).SingleOrDefault();
                List<payment>payment = db.payments.Where(x => x.admissionnum == id).ToList();
                List<Mark> mark = db.Marks.Where(x => x.Admissionum == id).ToList();
                var delete = db.students.Where(x=>x.admissionnum==id).SingleOrDefault();

                db.feemappings.Remove(feemap);
                foreach(var del in payment)
                {
                    db.payments.Remove(del);
                }
                foreach (var del in mark)
                {
                    db.Marks.Remove(del);
                }
                
                db.students.Remove(delete);
                db.SaveChanges();
                TempData["Delete"] = "Student Deleted Sucessfully";
                
            }
            return RedirectToAction("StudentList");
        }


        [Authorize(Roles = "Student")]
        public ActionResult StudentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var h = db.students.Where(x => x.admissionnum == id).SingleOrDefault();
            if (h == null)

                if (h == null)
            {
                return HttpNotFound();
            }
            return View(h);
            
        }
       
        [Authorize(Roles = "Teacher,Admin")]
        public ActionResult StudentEdit(int? id)
        
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentModel edit = new StudentModel();
           var d  = db.students.Where(x=>x.admissionnum == id).SingleOrDefault();
            if (d == null)
            {
                return HttpNotFound();
            }
            edit.AdmissionNumber = d.admissionnum;
            edit.StudentName = d.studentname;

            edit.PhoneNumber = d.phonenumber;
            edit.Gender = d.gender;
            edit.Address = d.adress;
            edit.Dateofbirth = d.dateofbirth;
            edit.Photo = d.photo;
            edit.FatherName = d.fathername;
            edit.classes = d.classid;
            edit.Dateofjoine = d.dateofjoining;
            d.rid = edit.Roles;
            edit.AcadamicYear = d.acadamicyear;
            edit.Email = d.Email;
            edit.Password = d.Password;

            return View(edit);

        }
        [HttpPost]
        public ActionResult StudentEdit(StudentModel edit)
        {
      
            var d = db.students.Find(edit.AdmissionNumber);
            d.studentname = edit.StudentName;

            d.phonenumber = edit.PhoneNumber;
            d.gender = edit.Gender;
            d.adress = edit.Address;
            d.dateofbirth = edit.Dateofbirth;
            d.photo = edit.Photo;
            d.fathername = edit.FatherName;
            d.classid = edit.classes;
            d.dateofjoining = edit.Dateofjoine;
            d.acadamicyear = edit.AcadamicYear;
            d.rid = edit.Roles;
            d.isactive = true;
         
            db.SaveChanges();
            TempData["Edit"] = "Student Details Saved Sucessfully";
            return RedirectToAction("StudentList");

        }

        
        [Authorize]
        public ActionResult StudentList()
        {

            var h = db.students.ToList();

            return View(h);

        }
        public ActionResult practice()
        {

            return View();
        }
        public ActionResult AddClass()
        {
            ClassModel e = new ClassModel();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            //e.Teachers = d.GetTeachers();
            return View();
        }

        [HttpPost]
        public ActionResult AddClass(ClassModel c)
        {
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            //c.Teachers = d.GetTeachers();
            db.createclass(c.ClassName);
            //@class b = new @class();
            //b.classname = c.ClassName;
            


            //db.classes.Add(b);
            //db.SaveChanges();
            return RedirectToAction("AddClass");
        }
        public ActionResult Todo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Todo(string x)
        {


            var array = x.ToLower().ToCharArray();
            Array.Reverse(array);
            if (x.ToLower().Equals(new string(array)))
            {
                TempData["result"]=x + "is palindrome";
            }
            else
            {
                TempData["result"] = x + " is not palindrome";
            }

           
            return View();

            
           
        }
        public ActionResult Payment()
        {



            return View();
        }
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Payment(PaymentModel p, string GridHtml)
        {
            if (p.StundentName == null)
            {
                var pay = db.feemappings.Where(x => x.admissionnum == p.AdmissionNumber).FirstOrDefault();
                if (pay == null)
                {
                    TempData["FeeMapping"] = "Record not exist";
                    return View();
                }
                p.AdmissionNumber = pay.admissionnum;
                p.StundentName = pay.studentname;
                p.Class = pay.classid;
                p.Totalfee = pay.schoolfee;
                p.Dateofpayment = DateTime.Now.ToString();
                var a = db.payments.Where(x => x.admissionnum == p.AdmissionNumber && x.islastpay == true).FirstOrDefault();
                if (a == null)
                {
                    p.Pending = pay.schoolfee;
                }
                else
                {
                    p.Pending = a.feepending;
                }
                return View(p);
            }
            else
            {
                var h = db.payments.Where(x => x.admissionnum == p.AdmissionNumber && x.islastpay == true).FirstOrDefault();
                if (h == null)
                {
                    Console.WriteLine("it is null");
                }
                else
                {
                    h.islastpay = false;
                }

                payment t = new payment();
                t.admissionnum = p.AdmissionNumber;
                t.studnetname = p.StundentName;
                t.totalfee = p.Totalfee;
                t.paymentname = p.Paymenttype;
                t.feepaid = p.paying;
                if (p.Pending == t.totalfee)
                {
                    t.feepending = t.totalfee - p.paying;
                }
                else
                {
                    t.feepending = h.feepending - p.paying;
                }
                t.lastpaiddate = DateTime.Now;
                t.classid = p.Class;
                t.islastpay = true;
                t.isactive = true;

                db.payments.Add(t);
                db.SaveChanges();
                TempData["Success"] = "Payment Successfully!";
                if (GridHtml.IsEmpty())
                {
                    Console.WriteLine("No Print");

                }
                else
                {
                    using (MemoryStream stream = new System.IO.MemoryStream())
                    {
                        StringReader sr = new StringReader(GridHtml);
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        pdfDoc.Close();
                        return File(stream.ToArray(), "application/pdf", "Grid.pdf");

                    }
                }

            }

            return RedirectToAction("Payment");
        }



        public ActionResult Paymentlist()
        {

            var c = db.payments.ToList();
            return View(c);
        }
        public ActionResult Attandance()
        {

            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            AttandanceModel a = new AttandanceModel();
            a.classlist = d.GetClasses();

            return View(a);
        }


        [HttpPost]
        public ActionResult Attandance(AttandanceModel a)

        {

            attendance attend = new attendance();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            a.classlist = d.GetClasses();

            if (db.attendances.Where(x => x.dateofattandance == a.Dateofattadance && x.classid == a.Classes).Any())
            {
                TempData["Attendance"] = "Records already exist!";
                return View(a);
            }

            if (a.Attandance != null)
            {


                for (int i = 0; i < a.Attandance.Count; i++)
                {

                    attend.attandences = a.Attandance[i];
                    attend.studentname = a.StudentName[i];
                    attend.classid = a.Classes;
                    attend.dateofattandance = a.Dateofattadance;
                    db.attendances.Add(attend);
                    db.SaveChanges();
                }

                return View(a);

            }
            else
            {
                ViewBag.students = db.students.Where(x => x.classid == a.Classes).ToList();


                return View(a);
            }
        }
        public ActionResult FeeMapping()
        {

            return View();
        }
        [HttpPost]
        public ActionResult FeeMapping(FeeMappingModel f)
        {

            if (f.Class == 0)
            {
                if (db.feemappings.Where(x => x.admissionnum == f.AdmissionNumber).Any())
                {
                    TempData["FeeMapping"] = "Records already exist!";
                    return View();
                }
                var d = db.students.Where(x => x.admissionnum == f.AdmissionNumber).FirstOrDefault();
                if (d == null)
                {
                    TempData["FeeMapping"] = "Record not exist";
                    return View();
                }
                f.AdmissionNumber = d.admissionnum;
                f.Class = d.classid;
                f.FatherName = d.fathername;
                f.StudentName = d.studentname;
                return View(f);
            }
            else {
                feemapping g = new feemapping();

                g.admissionnum = f.AdmissionNumber;
                g.classid = f.Class;
                g.fathername = f.FatherName;
                g.studentname = f.StudentName;
                g.schoolfee = f.Schoolfee;
                g.busfee = f.Busfee;
                db.feemappings.Add(g);
                db.SaveChanges();
                TempData["FeeMapping"] = "FeeMapping sucessfully!";
            }
            return RedirectToAction("FeeMapping");
        }
        public ActionResult AddSubject()

        {
            SubjectModel s = new SubjectModel();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
         
            s.subjectlist = db.subjectts.ToList();
            return View(s);

        }

        [HttpPost]
        public ActionResult AddSubject(SubjectModel s)
        {


            subjectt d = new subjectt();
            
                d.subname = s.SubjectName;
                d.minmarks = s.MinMarks;
                d.maxmarks = s.MaxMarks;
                db.subjectts.Add(d);
                db.SaveChanges();
            
            TempData["Addsubject"] = "Subject Added sucessfully!";
            return RedirectToAction("AddSubject");
        }
        public ActionResult SubjectMapping()
         {
            MappingModel m = new MappingModel();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            m.classlist = d.GetClasses();
            m.subjectlist = d.GetSubjects();
            m.SMlist = db.subjectmappings.ToList();
            return View(m);
           
        }
        [HttpPost]
        public ActionResult SubjectMapping(MappingModel m)
        {

            subjectmapping sm = new subjectmapping();

            for (int i = 0; i < m.Classes.Count; i++)
            {

                sm.classid = m.Classes[i];
                sm.subid = m.subject;

                db.subjectmappings.Add(sm);

                db.SaveChanges();
            }
            TempData["SubjectMapping"] = "Subject Mapping sucessfully!";
            return RedirectToAction("SubjectMapping");
        }
        public ActionResult ExamMapping()
        {
            MappingModel m = new MappingModel();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            m.classlist = d.GetClasses();
            m.examlist = d.GetExams();
            m.EMlist = db.Exammappings.ToList();
            return View(m);

        }
        [HttpPost]
        public ActionResult ExamMapping(MappingModel m)
        {
            Exammapping sm = new Exammapping();
            for (int i = 0; i < m.Classes.Count; i++)
            {
                sm.classid = m.Classes[i];
                sm.examid = m.exam;

                db.Exammappings.Add(sm);
                db.SaveChanges();
            }
            TempData["ExamMapping"] = "Exam Mapping sucessfully!";
            return RedirectToAction("ExamMapping");
            
        }
        [HttpPost]
        public ActionResult GetExams(string id)
        {
            int statId;
            List<SelectListItem> ex = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(id))
            {
                statId = Convert.ToInt32(id);
                List<exam> e = db.exams.Where(x => x.eid == statId).ToList();
                e.ForEach(x =>
                {
                    ex.Add(new SelectListItem { Text = x.examname, Value = x.eid.ToString() });
                });

            }
            return Json( ex , JsonRequestBehavior.AllowGet );
        }
        public ActionResult Exams()
        {
            ExamsModel E = new ExamsModel();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
           
            E.Examlist = db.exams.ToList();
            

            return View(E);


            
        }
        [HttpPost]  
        public ActionResult Exams(ExamsModel E)
        {
            exam e = new exam();
          
                e.examname = E.ExamName;
               
                e.examdate = E.ExamDate;
                db.exams.Add(e);
                db.SaveChanges();
            
            TempData["CreateExam"] = "Exam Created sucessfully!";
            return RedirectToAction("Exams");

            
        }
        public ActionResult Marks()
        {
            MarksModel M = new MarksModel();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            M.classlist = d.GetClasses();
            M.examlist = d.GetExams();
            return View(M);

        }
        [HttpPost]
        public ActionResult Marks(MarksModel M)
        {
           
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            M.classlist = d.GetClasses();
            M.examlist = d.GetExams();
            
           
            if(M.Subjectid ==null)
            {

                ViewBag.students = db.students.Where(x => x.classid == M.Classes).ToList();
                ViewBag.subject = db.subjectmappings.Where(x => x.classid == M.Classes).ToList();
                return View(M);
            }
           
            Mark m = new Mark();
            for (int i = 0; i < M.Subjectid.Count; i++)
            {
                m.Admissionum = M.AdmissionNum;
                m.classid = M.Classes;
                m.examid = M.Examid;
                m.subid = M.Subjectid[i];
                m.submarks = M.Subjectmarks[i];
                if (M.Subjectmarks[i]==0)
                {
                    m.isprasent = true;
                }
                
                var sub = db.subjectts.Where(x => x.subid == m.subid).FirstOrDefault();
                if (M.Subjectmarks[i]<sub.minmarks)
                {
                    m.Mstatus = "Fail";
                }
                else
                {
                    m.Mstatus = "Pass";
                }
                db.Marks.Add(m);
                db.SaveChanges();
            }
            TempData["Marks"] = "Marks Added sucessfully!";
            return RedirectToAction("Marks");
            

        }
        public ActionResult Reports()
        {
            ReportsModel R = new ReportsModel();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            R.classlist = d.GetClasses();
            R.examlist = d.GetExams();
            return View(R);

        }
        [HttpPost]
        public ActionResult Reports(ReportsModel R)
        {
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            R.classlist = d.GetClasses();
            R.examlist = d.GetExams();
            if (R.AddmissionNum==0)
            {
                ViewBag.students = db.students.Where(x => x.classid == R.Classes).ToList();

                return View(R);
            }
           
                R.Marklist = db.Marks.Where(x => x.Admissionum == R.AddmissionNum && x.examid==R.Exams).ToList();
            var t = 0;
                foreach (var n in R.Marklist)
                {
                    R.TotalMarks += Convert.ToInt16(n.submarks);
                     t += n.subjectt.maxmarks;
               
                    R.Percentage = (R.TotalMarks*100) / t;
                    if (n.Mstatus == "Fail")
                    {
                        R.status = "Fail";
                    }
                    else
                    {
                        R.status = "Pass";
                      }
                R.StudentName = n.student.studentname;
                R.Class = n.@class.classname;
                R.Exam = n.exam.examname;
                   
                }
            
            return View("StudentReportCard",R);
        }
        public ActionResult AddTeacher()
            {
            TeacherModel T = new TeacherModel();
            Dropdowns.Dropdowns1 d = new Dropdowns.Dropdowns1();
            T.Rolelist = d.GetRoles();
            return View(T);
        }
        [HttpPost]
        public ActionResult AddTeacher(TeacherModel s, HttpPostedFileBase file, int? id)
        {


            Dropdowns.Dropdowns1 w = new Dropdowns.Dropdowns1();
            s.Rolelist = w.GetRoles();
            if (file != null)
            {

                s.Photo = new byte[file.ContentLength];
                file.InputStream.Read(s.Photo, 0, file.ContentLength);

            }


            Teacher d = new Teacher();
            
            d.Tname = s.TeacherName;

            d.phonenumber = s.PhoneNumber;
            d.gender = s.Gender;
            d.adress = s.Address;
            d.dateofbirth = s.Dateofbirth;
            d.photo = s.Photo;
            d.fathername = s.FatherName;
            d.rid = s.Roles;
            d.dateofjoing = s.Dateofjoine;
            d.Email = s.Email;
            d.Password = s.Password;
            d.isactive = true;
            db.Teachers.Add(d);
            db.SaveChanges();
           
            TempData["Teacher"] = true;
            return RedirectToAction("AddTeacher");
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {


            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }

        }
        public void Quizquestions()
        {

            Q.Add(new Quiz
            {
                ID = 1,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            Q.Add(new Quiz
            {
                ID = 1,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            Q.Add(new Quiz
            {
                ID = 2,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            Q.Add(new Quiz
            {
                ID = 3,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            Q.Add(new Quiz
            {
                ID = 4,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
        }
        List<Quiz> Q = new List<Quiz>();
        public ActionResult QuizTest() 
        {

            Q.Add(new Quiz
            {
                ID = 4,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            Q.Add(new Quiz
            {
                ID = 5,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            Q.Add(new Quiz
            {
                ID = 6,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            Quiz a = new Quiz();
            a.Quizzes = Q.ToList();

           
            return View(a);
        }
        [HttpPost]
        public ActionResult QuizTest(Quiz a,List<int>num)
        {
            Q.Add(new Quiz
            {
                ID = 4,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            Q.Add(new Quiz
            {
                ID = 5,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            Q.Add(new Quiz
            {
                ID = 6,
                Question = "Automobiles produced by Telsa Motors operate on which form of energy?",
                OptionA = "Honda",
                OptionB = "Toyota",
                OptionC = "Kawasaki",
                OptionD = "Yamaha",
                CorrectAns = "Toyota",
                TotalMarks = 0,
            });
            try
            {
                for (int i = 0; i < a.ans.Count; i++)
                {
                    var b = Q.ToList().Where(x => x.ID == num[i]).FirstOrDefault();
                    if (b.CorrectAns == a.ans[i])
                    {
                        a.TotalMarks = a.TotalMarks+1;
                    }
                }
            }
            catch (Exception e)
            {

                ViewData["Total"] = e.Message;
            }
            
            ViewData["Total"] = a.TotalMarks;
            return View(a);
        }
    }
}