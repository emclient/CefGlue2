# CefGlue2

CefGlue2 is a rewrite of the CefGlue ([1](https://gitlab.com/xiliumhq/chromiumembedded/cefglue), [2](https://github.com/OutSystems/CefGlue)) project. It provides a lightweight API wrapper for the [Chromium Embedded Framework](https://github.com/chromiumembedded/cef/) on .NET platform.

The original CefGlue project is no longer actively maintained and forks only update the codebase very sparesely to handle the new CEF versions. The project parsed the CEF headers using Python scripts derived from the [C-to-CPP/CPP-to-CPP translator](https://github.com/chromiumembedded/cef/blob/master/tools/translator.README.txt). It then generated part of the scaffolding in C# while relying on manually writing the rest of the bindings.

This project aims to automate the bulk of the maintainance by using a C# source generator. It generates the C# objects for both proxy and handler classes of the CEF API including the parameter marshalling code. This reduces the surface of manually written bindings to pre-existing initialization code, enums and structures. It makes the upgrades to newer CEF API versions significantly easier.

The public API is very similar to the original CefGlue but not exactly identical. The main difference is in some parameter types (eg. `nint` instead of `long`, `ref` instead of `out`, and so on) where the automatic API translation produces different signatures that the original API.

High-level bindings, such as Avalonia or System.Windows.Forms wrappers for the CEF control, are not provided in this repository. They can be trivially adapted from other CefGlue forks if needed.
