using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ModelClasses.Entities.Testing;
using ModelClasses.Entities.TestParts;
using QuizApp.ViewModel;
using QuizApp.ViewModel.Managing;
using QuizApp.ViewModel.Mapping;
using Services;

namespace QuizApp.Controllers
{
    [Authorize]
    public class ApilikeController : Controller
    {
        private readonly IGetInfoService _getInfoService;
        private readonly ILowLevelTestManagementService _lowLevelTestManagementService;
        private readonly IHighLevelTestManagementService _highLevelTestManagementService;

        private readonly IMapper _mapper;
        private readonly IAdvancedMapper _advancedMapper;

        public ApilikeController(IGetInfoService getInfoService,
            ILowLevelTestManagementService lowLevelTestManagementService,
            IHighLevelTestManagementService highLevelTestManagementService, IMapper mapper,
            IAdvancedMapper advancedMapper)
        {
            _getInfoService = getInfoService;
            _lowLevelTestManagementService = lowLevelTestManagementService;
            _highLevelTestManagementService = highLevelTestManagementService;
            _mapper = mapper;
            _advancedMapper = advancedMapper;
        }

        [HttpGet]
        public JsonResult GetAnswersByQuestionGuid(string questionGuid)
        {
            var answerViewModelList = _getInfoService
                .GetQuestionByGuid(questionGuid)
                ?.TestAnswers
                .Select(a => _mapper.Map<AnswerViewModel>(a))
                .ToList();

            return Json(answerViewModelList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void CreateAnswer(string questionGuid, AnswerViewModel answer)
        {
            var testAnswer = _mapper.Map<TestAnswer>(answer);
            _lowLevelTestManagementService.CreateAnswerForQuestion(questionGuid, testAnswer);
        }
        [HttpPost]
        public void RemoveAnswer(string answerGuid)
        {
            _lowLevelTestManagementService.RemoveAnswer(answerGuid);
        }

        [HttpGet]
        public JsonResult GetQuestionsByTestGuid(string testGuid)
        {
            var questionViewModelList = _getInfoService
                .GetTestByGuid(testGuid)
                ?.TestQuestions
                .Select(q => _advancedMapper.MapTestQuestion(q))
                .ToList();

            return Json(questionViewModelList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void CreateQuestion(string testGuid, QuestionViewModel question)
        {
            var testQuestion = _mapper.Map<TestQuestion>(question);
            _lowLevelTestManagementService.CreateQuestionForTest(testGuid, testQuestion);
        }
        [HttpPost]
        public void RemoveQuestion(string testGuid, string questionGuid)
        {
            _lowLevelTestManagementService.RemoveQuestion(questionGuid);
        }
        [HttpPost]
        public void UpdateQuestion(string questionGuid, QuestionViewModel question)
        {
            var testQuestion = _mapper.Map<TestQuestion>(question);
            _lowLevelTestManagementService.UpdateQuestion(questionGuid, testQuestion);
        }


        [HttpPost]
        public void CreateTest(TestViewModel test)
        {
            var testFromDomain = _advancedMapper.MapTestViewModel(test);
            _highLevelTestManagementService.CreateTest(testFromDomain);
        }
        [HttpPost]
        public void UpdateTest(string testGuid, TestViewModel test)
        {
            var testFromDomain = _advancedMapper.MapTestViewModel(test);
            _highLevelTestManagementService.UpdateTest(testGuid, testFromDomain);
        }
        [HttpPost]
        public void RemoveTest(string testGuid)
        {
            _highLevelTestManagementService.RemoveTest(testGuid);
        }


        [HttpPost]
        public void CreateTestingUrl(TestingUrlViewModel testingUrl)
        {
            var testUrlDomain = _advancedMapper.MapTestingUrlViewModel(testingUrl);
            _highLevelTestManagementService.CreateTestingUrl(testUrlDomain);
        }
        [HttpPost]
        public void RemoveTestingUrl(string testingUrlGuid)
        {
            _highLevelTestManagementService.RemoveTestingUrl(testingUrlGuid);
        }


        [HttpPost]
        public void RemoveTestingResult(string testingResultGuid)
        {
            _highLevelTestManagementService.RemoveTestingResult(testingResultGuid);
        }
    }
}