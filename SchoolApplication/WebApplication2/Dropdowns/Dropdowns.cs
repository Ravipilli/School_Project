using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication2.DB;

namespace WebApplication2.Dropdowns
{
    public class Dropdowns1
    {
        schoolEntities3 db = new schoolEntities3();
        //public IEnumerable<SelectListItem> GetTeachers()
        //{

        //    var teachers = db.teachers.Select(x =>
        //                        new SelectListItem
        //                        {
        //                            Value = x.tid.ToString(),
        //                            Text = x.teachername
        //                        });

        //    return new SelectList(teachers, "Value", "Text");
        //}
        public IEnumerable<SelectListItem> GetClasses()
        {

            var classes = db.classes.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.classid.ToString(),
                                    Text = x.classname
                                });

            return new SelectList(classes, "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetSubjects()
        {

            var subjects = db.subjectts.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.subid.ToString(),
                                    Text = x.subname
                                });

            return new SelectList(subjects, "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetExams()
        {

            var Exams = db.exams.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.eid.ToString(),
                                    Text = x.examname
                                });

            return new SelectList(Exams, "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetRoles()
        {

            var roles = db.Roles.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.rid.ToString(),
                                    Text = x.RName
                                });

            return new SelectList(roles, "Value", "Text");
        }


    }
}