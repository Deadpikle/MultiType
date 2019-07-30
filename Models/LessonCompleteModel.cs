using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiType.Models
{
    public class LessonCompleteModel
    {
        public string WPM { get; set; }
        public int UserErrors { get; set; }
        public string LessonName { get; set; }

        public bool DidSucceed
        {
            get { return UserErrors < 2; }
        }

        public LessonCompleteModel(string WPM, int userErrors = 0, string lessonName = "")
        {
            this.WPM = WPM;
            this.UserErrors = userErrors;
            this.LessonName = lessonName;
        }
    }
}
