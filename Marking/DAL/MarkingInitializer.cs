using Marking.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Marking.DAL
{
    public class MarkingInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MarkingContext>
    {
        protected override void Seed(MarkingContext context)
        {
            var classrooms = new List<Classroom>
            {
                new Classroom{Title="Miss Beecroft",Grade=2,Year=2014},
                new Classroom{Title="Miss Cherett",Grade=2,Year=2014},
                new Classroom{Title="Miss Ryan",Grade=4,Year=2014}
            };
            classrooms.ForEach(s => context.Classrooms.Add(s));
            context.SaveChanges();

            var assessments = new List<Assessment>
            {
                new Assessment{ClassroomID=1,Title="Purple Cow",Subtitle="Demonstrating knowledge of note value & rhythym",Description="First assignment",GroupWork=false},
            };
            assessments.ForEach(s => context.Assessments.Add(s));
            context.SaveChanges();

            var attachments = new List<Attachment>
            {
                new Attachment{ParentID=1,ParentModel="Assessment",Title="My File Name",Filename="SomeFileName.jpg",FilenameInternal="mission-bg6.jpg"}
            };
            attachments.ForEach(s => context.Attachments.Add(s));
            context.SaveChanges();

            var criteria = new List<Criterion>
            {
                new Criterion{
                    AssessmentID=1,
                    FieldType="checkbox",
                    Label="Checkbox",
                    FieldOrder=1,
                    Default=true
                },
                new Criterion{
                    AssessmentID=1,
                    FieldType="textbox",
                    Label="Textbox",
                    FieldOrder=2
                },
                new Criterion{
                    AssessmentID=1,
                    FieldType="radio",
                    Label="Radio",
                    FieldOrder=3
                },
                new Criterion{
                    AssessmentID=1,
                    FieldType="textboxmulti",
                    Label="TextboxMulti",
                    FieldOrder=4
                },
                new Criterion{
                    AssessmentID=1,
                    FieldType="dropdown",
                    Label="Dropdown",
                    FieldOrder=5
                }
            };
            criteria.ForEach(s => context.Criteria.Add(s));
            context.SaveChanges();

            var options = new List<DropdownOption>
            {
                new DropdownOption{
                    CriterionID=1,
                    OptionOrder=1,
                    Key="1",
                    Value="Checkbox Value 1"
                },
                new DropdownOption{
                    CriterionID=3,
                    OptionOrder=1,
                    Key="1",
                    Value="Radio Value 1"
                },
                new DropdownOption{
                    CriterionID=3,
                    OptionOrder=2,
                    Key="2",
                    Value="Radio Value 2"
                },
                new DropdownOption{
                    CriterionID=5,
                    OptionOrder=1,
                    Key="1",
                    Value="Dropdown Value 1"
                },
                new DropdownOption{
                    CriterionID=5,
                    OptionOrder=2,
                    Key="2",
                    Value="Dropdown Value 2"
                },
                new DropdownOption{
                    CriterionID=5,
                    OptionOrder=3,
                    Key="3",
                    Value="Dropdown Value 3"
                }
            };
            options.ForEach(s => context.DropdownOptions.Add(s));
            context.SaveChanges();

            var students = new List<Student>
            {
                new Student{FirstName="Carson",LastName="Alexander"},
                new Student{FirstName="Meredith",LastName="Alonso"},
                new Student{FirstName="Arturo",LastName="Anand"},
                new Student{FirstName="Gytis",LastName="Barzdukas"},
                new Student{FirstName="Yan",LastName="Li"},
                new Student{FirstName="Peggy",LastName="Justice"},
                new Student{FirstName="Laura",LastName="Norman"},
                new Student{FirstName="Nino",LastName="Olivetto"}
            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment{StudentID=1,ClassroomID=1},
                new Enrollment{StudentID=2,ClassroomID=1},
                new Enrollment{StudentID=3,ClassroomID=1},
                new Enrollment{StudentID=4,ClassroomID=2},
                new Enrollment{StudentID=5,ClassroomID=2},
                new Enrollment{StudentID=6,ClassroomID=2},
                new Enrollment{StudentID=7,ClassroomID=3},
                new Enrollment{StudentID=8,ClassroomID=3},
                new Enrollment{StudentID=2,ClassroomID=2},
                new Enrollment{StudentID=3,ClassroomID=3},
                new Enrollment{StudentID=5,ClassroomID=1},
                new Enrollment{StudentID=6,ClassroomID=3}
            };
            enrollments.ForEach(s => context.Enrollments.Add(s));
            context.SaveChanges();
        }
    }
}