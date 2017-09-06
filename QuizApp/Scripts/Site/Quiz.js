$("#startTest").click(function () {
    $("#startTest").hide();
    $(".testBlock").show();
    //08/29/2017 15:48:29

    $("input[name='TestingStartDateTime']").attr('value', getTime());
    InitQuestions();
});

var _allQuestions;
var _currentQuestionIndex = 0;
var _prevQuestionBlock;
var _questionParent = $("#questions");
var _curQuestionName = 'Questions[' + _currentQuestionIndex + ']';

function getTime() {
    var currentdate = new Date();
    var datetime = currentdate.getDate() + "/"
        + (currentdate.getMonth() + 1) + "/"
        + currentdate.getFullYear() + " "
        + currentdate.getHours() + ":"
        + currentdate.getMinutes() + ":"
        + currentdate.getSeconds();

    return datetime;
}

function InitQuestions() {
    $.ajax({
        url: '/Quiz/GetInfoAndStartTest',
        type: 'GET',
        data: {
            'testingUrlGuid': _questionParent.attr('guid')
        },
        success: function (data) {
            _allQuestions = data;
            _questionParent.append(GetNextQuestion());
            _currentQuestionIndex++;
        }
    });
}

$('#nextQuestion').click(function () {
    _prevQuestionBlock.hide();
    if (_currentQuestionIndex < _allQuestions.Questions.length) {
        _questionParent.append(GetNextQuestion());
        _currentQuestionIndex++;
    } else {
        $('#finishQuiz').show();
        $('#nextQuestion').hide();
    }
});

var questionToAnswerIndexDict = {};

$(document).on('submit', 'form', function (e) {
    $("input:checked").each(function () {
        var answerGuid = $(this).attr('answerGuid');
        var questionGuid = $(this).attr('value');
        var questionIndex = $(this).attr('questionIndex');

        _curQuestionName = 'Questions[' + questionIndex + ']';

        if (questionToAnswerIndexDict[questionGuid] === undefined) {
            questionToAnswerIndexDict[questionGuid] = 0;
        } else {
            questionToAnswerIndexDict[questionGuid]++;
        }
        var hiddenInfo = $('<input type="hidden" name="' + _curQuestionName + '.AnswersSelected[' + questionToAnswerIndexDict[questionGuid] + ']" value = "' + answerGuid + '"/>');
        $('#questions').after(hiddenInfo);

    });
    $("input[name='TestingEndDateTime']").attr('value', getTime());
    return true;
});

$("#questions").on("click", ".answerInfo", function (e) {
    var target = $(event.target);
    
    if (target.is("input"))
        return;
    var elem = $(this).find('input');
    if (elem.attr('type') === 'radio') {
        elem.prop('checked', true);
    } else {
        elem.prop('checked', !elem.is(":checked"));
    }
});

function GetNextQuestion() {
    var nextQuestion = _allQuestions.Questions[_currentQuestionIndex];

    var answers = $('<div class="answers"></div>');
    _allQuestions.Questions[_currentQuestionIndex].Answers.forEach(function (item, i) {

        var curQuestionGuid = _allQuestions.Questions[_currentQuestionIndex].Guid;
        _curQuestionName = 'Questions[' + _currentQuestionIndex + ']';

        var singleAnswer = $('<div class="singleAnswer"></div>');
        answers.append(singleAnswer);
        var answerInfo = $('<div class="answerInfo list-group-item"></div>');
        singleAnswer.append(answerInfo);

        var selectingType = "radio";
        var bMultiple = _allQuestions.Questions[_currentQuestionIndex].Multiple;
        if (bMultiple) {
            selectingType = "checkbox";
        }

        var selectAnswerElem =
            $('<input type="' + selectingType + '"    name="' + _curQuestionName + '.QuestionGuid" value="' + curQuestionGuid + '"/>');
        selectAnswerElem.attr('answerGuid', item.Guid);
        selectAnswerElem.attr('answerId', i);
        selectAnswerElem.attr('bMultiple', bMultiple);
        selectAnswerElem.attr('questionIndex', _currentQuestionIndex);
        answerInfo.append(selectAnswerElem);

        answerInfo.append('<span>' + item.Instance + '</span>');
    });
    var singleQuestion = $('<div class="singleQuestion"></div>');
    var questionInfo = $('<div class="questionInfo list-group-item"></div>');
    singleQuestion.append(questionInfo);
    _prevQuestionBlock = singleQuestion;
    questionInfo.append('<span>' + nextQuestion.Instance + '</span>');

    return singleQuestion.append(answers);
}