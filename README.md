# pdn-that-other-app-plus

A [Paint.NET](http://www.getpaint.net) Effect plugin that exports the current layer to other image editors.

## Installation

1. Close Paint.NET.
2. Place ThatOtherAppPlus.dll in the Paint.NET Effects folder which is usually located in one the following locations depending on the Paint.NET version you have installed.

  Paint.NET Version |  Effects Folder Location
  --------|----------
  Classic | C:\Program Files\Paint.NET\Effects    
  Microsoft Store | Documents\paint.net App Files\Effects

3. Restart Paint.NET.
4. The plug-in will now be available in the following menu location: Effects > Tools > That other app+.

## License

This project is licensed under the terms of the MIT License.   
See [License.txt](License.txt) for more information.

# Source code

## Prerequisites

* Visual Studio 2019
* Paint.NET 4.2.10 or later

## Building the plugin

* Open the solution
* Change the PaintDotNet references in the ThatOtherAppPlus project to match your Paint.NET install location
* Update the post build events to copy the build output to the Paint.NET Effects folder
* Build the solution
