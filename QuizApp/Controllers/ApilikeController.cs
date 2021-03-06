﻿using System;
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
        public ActionResult CreateAnswer(string questionGuid, AnswerViewModel answer)
        {
            var testAnswer = _mapper.Map<TestAnswer>(answer);
            _lowLevelTestManagementService.CreateAnswerForQuestion(questionGuid, testAnswer);

            return RedirectToAction("TestManagement", "Admin");
        }
        [HttpPost]
        public ActionResult RemoveAnswer(string answerGuid)
        {
            _lowLevelTestManagementService.RemoveAnswer(answerGuid);

            return RedirectToAction("TestManagement", "Admin");
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
        public ActionResult CreateQuestion(string testGuid, QuestionViewModel question)
        {
            var testQuestion = _mapper.Map<TestQuestion>(question);
            _lowLevelTestManagementService.CreateQuestionForTest(testGuid, testQuestion);

            return RedirectToAction("TestManagement", "Admin");
        }

        [HttpPost]
        public ActionResult RemoveQuestion(string testGuid, string questionGuid)
        {
            _lowLevelTestManagementService.RemoveQuestion(questionGuid);

            return RedirectToAction("TestManagement", "Admin");
        }
        [HttpPost]
        public ActionResult UpdateQuestion(string questionGuid, QuestionViewModel question)
        {
            var testQuestion = _mapper.Map<TestQuestion>(question);
            _lowLevelTestManagementService.UpdateQuestion(questionGuid, testQuestion);

            return RedirectToAction("TestManagement", "Admin");
        }

        [HttpPost]
        public ActionResult UpdateAnswer(string answerGuid, AnswerViewModel answer)
        {
            var testAnswer = _mapper.Map<TestAnswer>(answer);
            _lowLevelTestManagementService.UpdateAnswer(answerGuid, testAnswer);

            return RedirectToAction("TestManagement", "Admin");
        }


        [HttpPost]
        public ActionResult CreateTest(TestViewModel test)
        {
            if (test.Questions == null)
            {
                test.Questions = new List<QuestionViewModel>();
            }

            var testFromDomain = _advancedMapper.MapTestViewModel(test);
            _highLevelTestManagementService.CreateTest(testFromDomain);

            return RedirectToAction("TestManagement", "Admin");
        }
        [HttpPost]
        public void UpdateTest(string testGuid, TestViewModel test)
        {
            var testFromDomain = _advancedMapper.MapTestViewModel(test);
            _highLevelTestManagementService.UpdateTest(testGuid, testFromDomain);
        }
        [HttpPost]
        public ActionResult RemoveTest(string testGuid)
        {
            _highLevelTestManagementService.RemoveTest(testGuid);

            return RedirectToAction("TestManagement", "Admin");
        }


        [HttpPost]
        public ActionResult CreateTestingUrl(TestingUrlViewModel testingUrl)
        {
            var testUrlDomain = _advancedMapper.MapTestingUrlViewModel(testingUrl);
            _highLevelTestManagementService.CreateTestingUrl(testUrlDomain);

            return RedirectToAction("TestManagement", "Admin");
        }
        [HttpPost]
        public ActionResult RemoveTestingUrl(string testingUrlGuid)
        {
            _highLevelTestManagementService.RemoveTestingUrl(testingUrlGuid);

            return RedirectToAction("TestingUrlManagement", "Admin");
        }


        [HttpPost]
        public ActionResult RemoveTestingResult(string testingResultGuid)
        {
            _highLevelTestManagementService.RemoveTestingResult(testingResultGuid);

            return RedirectToAction("ResultManagement", "Admin");
        }
    }
}