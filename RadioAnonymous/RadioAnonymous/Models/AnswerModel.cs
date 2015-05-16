using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;

namespace RadioAnonymous.Models
{
    public class AnswerModel
    {
        #region Fields

        private string timestamp; 

        private string question;

        private string answer;

        private string userId;

        private HashSet<string> listUrls; 

        #endregion // Fields

        #region Properties
        public string Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public string TimestampLable
        {
            get { return " " + this.Timestamp; }
        }
        
        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        public string QuestionLable
        {
            get { return " " + this.Question; }
        }

        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }

        public string AnswerLable
        {
            get { return " " + this.Answer; }
        }

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public HashSet<string> ListUrls
        {
            get { return listUrls; }
            set { listUrls = value; }
        }



        #endregion // Properties

        #region Constructors
        public AnswerModel(string answer)
        {
            this.answer = HtmlUtilities.ConvertToText(answer);
            listUrls = new HashSet<string>();
        }
        public AnswerModel(string answer, string question) : this(answer)
        {
            this.question = HtmlUtilities.ConvertToText(question);
        }
        #endregion // Constructors

        #region Methods
        public override int GetHashCode()
        {
            return (this.question + this.answer + this.timestamp + this.userId).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var other = obj as AnswerModel;

            return other != null && this.question == other.question && this.answer == other.answer && this.timestamp == other.timestamp && this.userId == other.userId;
        }
        #endregion // Methods
    }

}
