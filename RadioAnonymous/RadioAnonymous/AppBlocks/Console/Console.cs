using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.AppBlocks.Console
{
    public class Console
    {
        private IConsole console;

        private Object obj;

        private enum Error
        {
            InvalidConutOfParams,
            InvalidParams
        }

        public Console(IConsole console, string cInDescription = "user/root> ")
        {
            this.console = console;
            console.CInDescription = cInDescription;
        }

        public object Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        #region ConsoleCommand
        private void ErrorCommand(Error error, params object[] obj)
        {
            if (error == Error.InvalidConutOfParams)
            {
                console.COut("Error: invalid count of params");
            }
            else if (error == Error.InvalidParams)
            {
                if (obj.Length == 0)
                {
                    console.COut("Error: invalide params.");
                }
                else if (obj.Length == 1)
                {
                    console.COut("Error: invalide param: " + string.Join(", ", obj));
                }
                else
                {
                    console.COut("Error: invalide params: " + string.Join(", ", obj));
                }
            }
        }


        public string HelpFormat(params object[] obj)
        {
            return String.Format(" {0,-20}  -    {1,-20}", obj[0], obj[1]);
        }

        private void HelpCommand(string[] parameters)
        {
            if (parameters.Length == 2)
            {
                switch (parameters[1])
                {
                    default:
                        console.COut("Sorry there is no information about command: " + parameters[1]);
                        break;
                }
            }
            else if (parameters.Length == 1)
            {
                console.COut("List of conmands:\n");
                console.COut(HelpFormat("cls/clear", "Clear console."));
                console.COut(HelpFormat("help/h", "Show list of commands."));
                console.COut(HelpFormat("play/p", "play des"));
                console.COut(HelpFormat("q/exit", "Exit from console."));
                console.COut(HelpFormat("stop/s", "Stop streaming radio."));
                console.COut("\nTo see information about command write:\n>h [commandName]");
            }
        }

        private void PlayCommand(string[] parameters)
        {
            if (parameters.Length == 1)
            {

                // run command play 192
            }
            else if (parameters.Length == 2)
            {
                if (parameters[1] == "192")
                {

                }
                else if (parameters[1] == "64")
                {

                }
                else if (parameters[1] == "12")
                {

                }
                else
                {
                    ErrorCommand(Error.InvalidParams, parameters[1]);
                }
            }
            else if (parameters.Length == 3)
            {
                if (parameters[1] == "-t" || parameters[1] == "/t")
                {
                    if (parameters[2] == "192")
                    {

                    }
                    else if (parameters[2] == "64")
                    {

                    }
                    else if (parameters[2] == "12")
                    {

                    }
                    else
                    {
                        ErrorCommand(Error.InvalidParams, parameters[2]);
                    }
                }
                else
                {
                    ErrorCommand(Error.InvalidParams, parameters[1]);
                }
            }
            else
            {
                ErrorCommand(Error.InvalidConutOfParams);
            }
        }

        private void VolumeCommand(string[] parameters)
        {
            if (parameters.Length == 2)
            {
                // ...
            }
            else
            {
                ErrorCommand(Error.InvalidConutOfParams);
            }
        }

        #endregion // ConsoleCommand

        public bool RunComands(string str)
        {
            var parameters = str.Split(' ');
            if (parameters.Length > 0)
            {
                if (parameters[0].ToLower() == "help" || parameters[0].ToLower() == "h")
                {
                    HelpCommand(parameters);
                }
                else if (parameters[0].ToLower() == "play" || parameters[0].ToLower() == "p")
                {
                    PlayCommand(parameters);
                }
                else if (parameters[0].ToLower() == "stop" || parameters[0].ToLower() == "s")
                {
                    // Method Stop
                }
                else if (parameters[0].ToLower() == "cls" || parameters[0].ToLower() == "clear")
                {
                    console.Cls();
                }
                else if (parameters[0].ToLower() == "volume" || parameters[0].ToLower() == "v")
                {
                    VolumeCommand(parameters);
                }
                else if (parameters[0].ToLower() == "q" || parameters[0].ToLower() == "exit")
                {
                    return false;
                }
            }
            

            return true;
        }

        public void Start()
        {
            console.C();

            while (RunComands(console.CIn()))
            {
                console.C();
            }
        }
    }
}
