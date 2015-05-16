using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadioAnonymous.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RadioAnonymous.Helpers.ControlHelpers
{
    public class AnswerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Answer { get; set; }
        public DataTemplate QuestionAnswer { get; set; }

        protected override DataTemplate SelectTemplateCore(object item,
                                                           DependencyObject container)
        {
            if (item is AnswerModel)
            {
                var answerModel = item as AnswerModel; 
                if (answerModel.UserId != null && answerModel.Answer != null)
                {
                    return QuestionAnswer;
                }
                else if (answerModel.Answer != null)
                {
                    return Answer;
                }
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
