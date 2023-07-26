# assembly-system-console-

The source file has:
first line - number of projects to build - integer space
number of modules in the project - an integer followed by a description of the modules and their dependencies
~~~~
[module name 1]: [dependency 1] ... [dependency N]
~~~~
further - tasks for assembling certain modules
What is the output: for each task, a line, in the format:
~~~~
[number of modules assembled]: [module assembled 1] .... [module assembled N]
~~~~
modules must go in build order from the most independent job linked if the module
already built, then you do not need to rebuild it duplication of the assembly of the same module
disabled a module can have from 0 to N dependencies (modules)
