﻿using CefParser;

var headerFiles = Directory.GetFiles("D:\\cef2\\chromium\\src\\cef\\binary_distrib\\cef_binary_138.0.27-em-7204.3237+gedd7a02+chromium-138.0.7204.35_windows32\\include\\", "*.h", SearchOption.AllDirectories);

var cp = new CefParser.CefParser(headerFiles);

var interopGen = new InteropGen(cp);

Directory.CreateDirectory("Interop");
using (var writer = new StreamWriter($"Interop\\libcef.g.cs"))
    interopGen.GenerateLibCefG(writer);
using (var writer = new StreamWriter($"Interop\\version.g.cs"))
    interopGen.GenerateVersionFile(writer);

Directory.CreateDirectory("Interop\\Classes.g");
foreach (var c in cp.Classes)
{
    using var writer = new StreamWriter($"Interop\\Classes.g\\{CefParser.CefParser.GetCApiName(c.Name, true)}.g.cs");
    interopGen.GenerateStructFile(c, writer);
}

Directory.CreateDirectory("Classes.g");
foreach (var c in cp.Classes)
{
    var csName = NameConverter.ToCSharpClassName(c.Name, CefParser.CefParser.TypeClass.Class);
    using var writer = new StreamWriter($"Classes.g\\{csName}.g.cs");
    interopGen.GenerateWrapper(c, writer);
}

Directory.CreateDirectory("Enums");
foreach (var e in cp.Enums)
{
    var csName = NameConverter.ToCSharpClassName(e.Name, CefParser.CefParser.TypeClass.Enum);
    using var writer = new StreamWriter($"Enums\\{csName}.g.cs");
    interopGen.GenerateEnum(e, writer);
}
