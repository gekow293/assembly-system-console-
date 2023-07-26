using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AssemblyConsoleApp;

public class Task
{
    public List<Module> Modules { get; set; }

    public int CountModules { get; set; }

    public int CountBuilds { get; set; }

    public Module GetModule(string name) => Modules.FirstOrDefault(x => x.Name == name); 
}
