using AssemblyConsoleApp;
using System.Runtime.Intrinsics.Arm;
using System.Xml;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;

namespace AssemblyConsoleApp;
public partial class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        //список результатов
        var listResult = new List<List<List<string>>>();

        using (var openFileDialog = new OpenFileDialog())
        {
            var filePath = string.Empty;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                filePath = openFileDialog.FileName;
            else return;

            using (StreamReader sr = new StreamReader(filePath))
            {
                //массив собираемых проектов
                Task[] tasks;

                int countTasks;

                //считываем количество собираемых проектов
                countTasks = int.Parse(sr.ReadLine());

                tasks = new Task[countTasks];

                //перебор заданий
                for (int i = 0; i < countTasks; i++)
                {
                    sr.ReadLine();

                    listResult.Add(new List<List<string>>());

                    tasks[i] = new Task()
                    {
                        Modules = new List<Module>(),
                    };

                    //считываем количество модулей в проекте
                    tasks[i].CountModules = int.Parse(sr.ReadLine());

                    //проходим по модулям и обозначаем как несобранные
                    for (int mod = 0; mod < tasks[i].CountModules; mod++)
                    {
                        Module module;

                        string[] modAndDep = sr.ReadLine().Split(' ');

                        modAndDep[0] = modAndDep[0].Trim(':');

                        //записываем модуль и его составные части в модули задания
                        if (modAndDep.Length > 1)
                        {
                            var deLlist = new List<Module>();

                            foreach (var item in modAndDep.Skip(1).ToList())
                                deLlist.Add(new Module(false, item, null));

                            module = new Module(false, modAndDep[0], deLlist);
                        }
                        else
                            module = new Module(false, modAndDep[0], null);

                        //заполняем список модулей заданий
                        tasks[i].Modules.Add(module);
                    }

                    //считываем количество заданий на сборку
                    tasks[i].CountBuilds = int.Parse(sr.ReadLine());

                    //проходим по заданиям на сборку
                    for (int b = 0; b < tasks[i].CountBuilds; b++)
                    {
                        var buildName = sr.ReadLine();

                        listResult[i].Add(new List<string>());

                        //текущее задание на сборку из всех модулей задания
                        var build = tasks[i].GetModule(buildName);

                        //если модуль составной
                        if (build.Children != null)
                        {
                            if (build.Builded == false)
                            {
                                //проверяем его части рекурсивно и собираем несобранные
                                CheckChildren(tasks[i], build, listResult[i][b]);

                                //собираем и добавляем в результат
                                build.Builded = true;

                                listResult[i][b].Add(build.Name);
                            }
                        }
                        else
                        {
                            //если модуль независимый то собираем сразу
                            if (build.Builded == false)
                            {
                                //собираем и добавляем в результат
                                build.Builded = true;

                                listResult[i][b].Add(build.Name);
                            }
                        }
                    }
                }
            }
        }

        using (var sw = new StreamWriter("output.txt", false))
        {
            foreach (var listTask in listResult)
            {
                foreach (var listBuilds in listTask)
                {
                    sw.Write(listBuilds.Count + " ");
                    foreach (var build in listBuilds)
                    {
                        sw.Write(build + " ");
                    }

                    sw.Write("\n");
                }

                sw.Write("\n");
            }
        }
    }

    public static void CheckChildren(Task project, Module build, List<string> listResult)
    {
        for (int m = project.Modules.Count - 1; m >= 0; m--)
        {
            for (int c = build.Children.Count - 1; c >= 0; c--)
            {
                if (project.Modules[m].Name == build.Children[c].Name)
                {
                    build.Children[c] = project.Modules[m];

                    if (build.Children[c].Builded == false)
                    {
                        if (build.Children[c].Children != null)
                        {
                            CheckChildren(project, build.Children[c], listResult);

                            build.Children[c].Builded = true;

                            listResult.Add(build.Children[c].Name);
                        }
                        else
                        {
                            build.Children[c].Builded = true;

                            listResult.Add(build.Children[c].Name);
                        }
                    }
                }
            }
        }
    }
}



