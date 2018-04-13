# PlantUml Language Service

![PlantUml](src/Resources/Icon.png)

## Description


>_PlantUML is an open-source tool allowing users to create UML diagrams from a plain text language._
>
>**[Wikipedia](https://en.wikipedia.org/wiki/PlantUML)**

PlantUml Language Service provides editor features for the [PlantUml Language](http://plantuml.com/) to Visual Studio (2017). These features are listed and elaborated on below.

##### Credit
+ Built and adapted from [TextMate starter project](https://github.com/madskristensen/TextmateSample) by [Mads Kristensen](https://github.com/madskristensen)
+ Modelled on [Ruby Language Service](https://github.com/madskristensen/RubyLanguageService)
+ Inspired by [Visual Studio Code plugin](https://github.com/qjebbs/vscode-plantuml) by [qjebbs](https://github.com/qjebbs)
+ Grammars by [qjebbs](https://github.com/qjebbs)

#### Features

- [Syntax highlighting](#syntax-highlighting-file-recognition)
- [Auto complete](#auto-complete)
- [Preview diagrams](#preview-diagrams)
- [Language Reference](#language-reference)
- [Preprocessor Friendly](#preprocessor)
- [Embedded Macros](#embedded-macros)
- [Embedded Skins](#embedded-skins)

<sub>More to be added over time ...</sub>

---
&nbsp;

## Syntax Highlighting & File Recognition

Syntax elements recognised in the [PlantUml grammar definition](src/grammars/plantuml.json) are highlighted accordingly for any of the following file types:

- .plantuml
- .plant
- .uml
- .iuml
- .puml
- .pu

![syntax](art/syntax.png)

These files are indicated in the Solution Explorer with the following icon: ![UmlModel](http://glyphlist.azurewebsites.net/img/images/UMLModel.png)

&nbsp;

## Auto Complete

![autocomplete](art/auto_complete.png)

Rudimentary auto completion against symbols in the same file is provided. Because no cross-file symbols can be loaded, the language service assumes some user knowledge of the basic PlantUml syntax.

---
&nbsp;

## Preview Diagrams

A context menu option has been added to the code window and solution explorer allowing for diagram generation of the selected/active PlantUml file.

#### Feedback

The preview window will render the diagram and provide feedback.

The Preview window is composed of 3 areas:

- **Top Panel** ~ collapsible holds the generated diagram url
- **Canvas** ~ the rendered diagram
- **Badge** ~ the error state indicator

![preview](art/preview.png)

###### Badges
 
|![OK](art/ok.png)|![warn](art/warning.png)|![error](art/error.png)|![unknown](art/unknown.png)|
|:----:|:----:|:----:|:----:|
| OK | Warning | Error | Unknown |

###### Urls

If valid, the generated diagram url will be printed to the Output window. For ease of re-use in your documentation it will be printed in both standard html and markdown formats.

To ensure high quality, scalable diagrams, the default url route will resolve to svg format. To use png instead, modify the generated url at this section "http://www.plantuml.com/plantuml/<**svg | png**>/"

&nbsp;

## Language Reference

A dockable Tool Window available under _"View > Other Windows > PlantUml Language Reference"_ links to the latest PlantUml Reference Guide.

&nbsp;

## Preprocessor

The PlantUml Language Service respects all common preprocessor commands, and can use them even if they are nested.

###### !include

While `!inlcude` is usable within the language service, there are caveats:

- can only make use of files located inside the active solution directory
- file to include should be specified [NAME].[EXTENSION]:

`!include somefile.extension`

If the file is not in the solution directory, or can otherwsie not be found or loaded, the diagram will render with warnings.

_To avoid warnings, or to use files located elsewhere on your computer, a custom flag has been created: **-P**  (provide the full path)._

`!include -p full/path/to/file`

&nbsp;

## Embedded Macros

Several macros have been embedded for simplicity. They can be imported into your diagram using the `!import` command.

<sub> -- Happy to receive/include more -- </sub>

- SysML (`!import SysML`)

Provides short hand commands to create and associate requirements, tests, and expectations:

| Function | Purpose |
| -- | -- |
|`Requirement(name,definition)` | creates a requirement object with detail |
|`BusinessNeed(name,definition)` | creates a business requirement with detail |
|`Generic(name,definition)`| creates a generic entry with detail |
|`TestWithScenario(name,testable)`| (where testable = existing requirement) creates a linked test |
|`VerifyWithTest(testable)`| generates a linked test without scenario against a requirement |
|`Option(requirement,name)`| creates an Options entry and associates with named requirement |

*all functions above accept a 3rd parameter for arrow direction and placement*

| Function | Purpose |
|--|--|
| `Test(name)` | create an unlinked test entry |
| `Scenario(test,testable)` | associate a test with a requirement via scenario |
|`Verify(requirement,testable)` | define acceptance as test verified for requirement |
|`Associate(firstrequirement,secondrequirement)` | create associateion between 2 requirements |
|`Derive(firstrequirement,secondrequirement)`| indicates requirement is derived from existing need |
| `Expectation(requirement, detail)` | outlines the expectation of a requirement |
|` Criticality(requirement, detail)`| describes the criticality os a requirement |
|`Describe(requirement, detail)` | provides a simple description of a requirement |

&nbsp;

## Embedded Skins

Several skins/themes have been embedded for simplicity. They can be imported into your diagram using the `!theme` command.

<sub> -- Happy to receive/include more -- please see [resource](src/resources/base.skin) for a base to work with</sub>

- blue (`!theme blue`)
- napkin (`!theme napkin`)
- trans [transparent] (`!theme trans`)

---
&nbsp;

## Future

- Reverse Dll to PlantUml Class diagrams
- Document Generator
- Code Generator
- Simple Intellisense / Auto Complete for core keywords

**To help realize these or to contribute please contact me.**