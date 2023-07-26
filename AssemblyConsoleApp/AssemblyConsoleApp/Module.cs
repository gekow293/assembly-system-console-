using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyConsoleApp;

public class Module
{
    public bool Builded { get; set; }

    public string Name { get; set; }

    public Module(bool builded, string name, List<Module> children)
    {
        this.Builded = builded;
        this.Name = name;
        this.Children = children;
    }

    public List<Module> Children { get; set; }
}
