﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ModelClasses.Entities.Testing;
using ModelClasses.Entities.TestParts;
using MoreLinq;
using Newtonsoft.Json;
using QuizApp.ViewModel;
using QuizApp.ViewModel.Managing;
using QuizApp.ViewModel.Mapping;
using Services;

namespace QuizApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IGetInfoService _getInfoService;
        private readonly IAdvancedMapper _advancedMapper;
        private readonly IAdvancedLogicService _advancedLogicService;
        private readonly IMapper _mapper;

        public AdminController(IGetInfoService getInfoService, IAdvancedMapper advancedMapper,
            IAdvancedLogicService advancedLogicService, IMapper mapper)
        {
            _getInfoService = getInfoService;
            _advancedMapper = advancedMapper;
            _advancedLogicService = advancedLogicService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View("TestManagement");
        }

        public ActionResult CreateTest()
        {
            return View();
        }

        public ActionResult CreateURL(string testGuid)
        {
            ViewBag.testGuid = testGuid;
            return View();
        }

        public ActionResult CreateQuestion(string testGuid)
        {
            ViewBag.testGuid = testGuid;
            return View();
        }

        public ActionResult AddAnswer(string questionGuid)
        {
            ViewBag.questionGuid = questionGuid;
            return View();
        }

        public ActionResult EditAnswer(string questionGuid, string answerGuid)
        {
            ViewBag.answerGuid = answerGuid;
            ViewBag.questionGuid = questionGuid;
            return View(GetAnswer(questionGuid, answerGuid));
        }

        public ActionResult EditQuestion(string questionGuid)
        {
            ViewBag.questionGuid = questionGuid;
            return View(GetQuestion(questionGuid));
        }

        public ActionResult TestManagement()
        {
            return View(GetAllTests());
        }

        public ActionResult TestingUrlManagement(string testGuid)
        {
            ViewBag.testGuid = testGuid;
            return View(GetAllTestingUrls());
        }

        public ActionResult ResultManagement()
        {
            return View(GetAllTestingResults());
        }

        [HttpGet]
        public List<TestViewModel> GetAllTests()
        {
            return _getInfoService.GetAllTests().Select(t => _advancedMapper.MapTest(t)).ToList();
        }

        [HttpGet]
        public QuestionViewModel GetQuestion(string testGuid)
        {
            return _advancedMapper.MapTestQuestion(_getInfoService.GetQuestionByGuid(testGuid));
        }

        [HttpGet]
        public AnswerViewModel GetAnswer(string questionGuid, string answerGuid)
        {
            return GetQuestion(questionGuid).Answers.FirstOrDefault(a => a.Guid == answerGuid);
        }

        [HttpGet]
        public List<TestingUrlViewModel> GetAllTestingUrls()
        {
            var testingsList = _getInfoService.GetAllTestingUrls();

            return testingsList.Select(t => _advancedMapper.MapTestingUrl(t)).ToList();
        }

        [HttpGet]
        public List<TestingResultViewModel> GetAllTestingResults()
        {
           var allResults =
                _getInfoService.GetAllTestingResults()
                    .Select(r => _mapper.Map<TestingResultViewModel>(r))
                    .ToList();
            return allResults;
        }

        public void GetResultsForTestCsv(string testGuid)
        {
            StringWriter oStringWriter = new StringWriter();
            oStringWriter.WriteLine("LoL line");
            Response.ContentType = "text/plain";

            Response.AddHeader("content-disposition", "attachment;filename=" +
                                                      $"test_results_for_{testGuid}.csv");
            Response.Clear();

            using (StreamWriter writer = new StreamWriter(Response.OutputStream, Encoding.UTF8))
            {
                _advancedLogicService.GetCsvResults(testGuid, writer);
            }
            Response.End();
        }
    }
}