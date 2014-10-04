using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NicePictureStudio.Models
{
    public class Question
    {
        public int ID { set; get; }
        public string QuestionText { set; get; }
        public List<Answer> Answers { set; get; }
        public string SelectedAnswer { set; get; }
        public Question()
        {
            Answers = new List<Answer>();
        }
    }
    public class Answer
    {
        public int ID { set; get; }
        public string AnswerText { set; get; }
    }
    public class CRMModel
    {
        public int Id { get; set; }
        public List<Question> Questions { set; get; }
        public string CustomerName { get; set; }
        public CRMModel()
        {
            Questions = new List<Question>();
        }
    }

    public class CustomerRelation
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceEmail { get; set; }
        public string ReferencePhoneNumber { get; set; }
    }
}