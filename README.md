# Context Matrix Methods for Ontology Completion in Wikidata

This is the code repository that accompanies the paper "Context Matrix Methods for Property and Structure Ontology Completion in Wikidata".

## Requirements

* You will need to install Jupyter (https://jupyter.org/)
* You will need to install .NET Core 3.1  (https://dotnet.microsoft.com/download/dotnet/3.1)
* You will need to install .NET Interactive (https://devblogs.microsoft.com/dotnet/net-interactive-preview-3-vs-code-insiders-and-polyglot-notebooks/)

## Steps

Here are the steps.

### Compile tools

Compile the tools.csproj project in tools/.  You may use any tool that can compile .NET 3.1 projects, including Visual Studio Code, Visual Studio, or the command line build tools.

The result should be a file in tools/bin/Debug/netcoreapp3.1 called tools.dll.


### Download

Download an RDF truthy dump from https://www.wikidata.org/wiki/Wikidata:Database_download/en

### Preprocess

Pre-process it into a simpler format:

|Entity ID number | Property ID number | Entity ID number |
| -------------- | --------------- | -------------- |
|31|1344|1088364|
|31|1151|3247091|
|31|1546|1308013|
|31|5125|7112200|
|31|38|4916     |

We refer to this output in the code as "WD_entity_prop_entity.csv".

See "100 Extract entities from truthy data.ipynb" for a starting point.  You may find it easier to use an RDF parser.  Alternatively you can use the JSON download provided by Wikidata if you prefer to process the JSON (e.g., with https://pypi.org/project/qwikidata/).

### Compute matrices

Open "200 Create frequency and related matrices.ipynb", modify it to point to the correct paths (throughout the file), select the property of interest, and point it to the test IDs file.  Then execute it to generate the frequency and related matrices.


### Compute similarity scores

Open "300 Calculate similarity function scores.ipynb", modify the paths, and run it to compute the similarity scores.


### Using the results

#### Prediction

You can take the top 10 scores for entities for which you wish to make predictions (e.g., when they lack the property of interest) and use these as the top predictions.

#### Evaluating the result

To compute the precision over these results, open and adjust "400 Compute precision.ipynb" to analyze the data.
