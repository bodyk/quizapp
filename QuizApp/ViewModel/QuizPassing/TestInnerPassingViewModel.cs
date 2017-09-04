using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuizApp.ViewModel.PassingQuiz;

namespace QuizApp.ViewModel.QuizPassing
{
    public class TestInnerPassingViewModel
    {
        public TimeSpan? TestTimeLimit { set; get; }
        public TimeSpan? QuestionTimeLimit { set; get; }
        public List<QuestionPassingViewModel> Questions { set; get; }
        public string AttepmtGuid { set; get; }
    }
}