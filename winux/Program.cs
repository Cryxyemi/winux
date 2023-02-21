using System;


namespace Program
{
    class App
    {
        public static void Main(string[] CmdArgs)
        {
            Console.Title = "Winux - Terminal";

            string ExeLocation = System.Reflection.Assembly.GetCallingAssembly().Location;
            string ExeFolder = System.IO.Path.GetDirectoryName(ExeLocation) ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            Handler cmd_handler = new Handler(ExeFolder);

            Console.WriteLine("Winux - Windows Linux Terminal");
            Console.WriteLine();

            while (true)
            {
                Console.Write($"{cmd_handler.StartPath} $ ");
                string? user_input = Console.ReadLine();

                if (user_input == null || user_input == string.Empty) Console.Write("");
                else cmd_handler.do_command(user_input);
            }
        }
    }

    class NicerPrint
    {
        public void ErrorPrint(string error)
        {
            Console.WriteLine($"ERROR: {error}");
        }

        public void InfoPrint(string info)
        {
            Console.WriteLine($"INFO: {info}");
        }

        public void WarningPrint(string warning)
        {
            Console.WriteLine($"WARNING: {warning}");
        }

        public void SuccessPrint(string success)
        {
            Console.WriteLine($"SUCCESS: {success}");
        }

        public void SpacerPrint()
        {
            Console.WriteLine("------------------------------");
        }
    }

    class Handler
    {
        NicerPrint nicer_print = new NicerPrint();
        public string StartPath;

        public Handler(string startPath)
        {
            StartPath = startPath;
        }

        

        public void do_command(string command)
        {
            string[] command_args = command.Split(' ');

            foreach (var arg in command_args)
            {
                if (arg == string.Empty ||
                    arg == " " ||
                    arg == null)
                {
                    nicer_print.ErrorPrint("Make sure that there are no double spaces in your command.");
                    return;
                }
            } 

            run_command(command_args[0], command_args[1..]);
        }

        private void run_command(string command, string[] args)
        {
            switch (command)
            {
                case "ls":
                    foreach (var file in System.IO.Directory.GetFiles(StartPath))
                    {
                        Console.WriteLine($"FILE: {System.IO.Path.GetFileName(file)}");
                    }

                    foreach (var dir in System.IO.Directory.GetDirectories(StartPath))
                    {
                        Console.WriteLine($"DIR: {System.IO.Path.GetFileName(dir)}");
                    }

                    nicer_print.SpacerPrint();
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "cd":
                    if (args.Length == 0)
                    {
                        nicer_print.ErrorPrint("Please specify a directory.");
                        break;
                    }

                    if (System.IO.Path.IsPathRooted(args[0]))
                    {
                        if (System.IO.Directory.Exists(args[0]))
                        {
                            StartPath = args[0] ?? Environment.GetFolderPath(Environment.SpecialFolder.System);
                        }
                        else
                        {
                            nicer_print.ErrorPrint("Directory does not exist.");
                            break;
                        }
                    }

                    if (args[0] == "..")
                    {
                        DirectoryInfo d = new DirectoryInfo(StartPath);

                        if (d.Parent == null) ;
                        else StartPath = System.IO.Directory.GetParent(StartPath).FullName ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    }
                    else if (System.IO.Directory.Exists($"{StartPath}\\{args[0]}"))
                    {
                        StartPath = $"{StartPath}\\{args[0]}";
                    }
                    else
                    {
                        nicer_print.ErrorPrint("Directory does not exist.");
                        break;
                    }
                    nicer_print.SuccessPrint($"Changed directory to {StartPath}");
                    break;

                default:
                    nicer_print.ErrorPrint("Command not found.");
                    break;
            }
        }
    }
}
