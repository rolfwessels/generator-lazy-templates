<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <NuGetReference>Humanizer</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Humanizer</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

string  _location = @"..\..\src";
string  _template = @"Project";
string  _toName = @"Sample";
string[]  _fileTypes = new [] { @".cs",".js",".html",".txt",".json", ".less"};
string[]  _exclude = new [] { @"bower_components" ,".OAuth2.","RequestClientDetailsHelper","Mappers\\MapClient.cs" , "Enums\\","node_modules",".tmp"};
string _scaffoldingInjectionFile = ".scaffolding.injection.json";
string _focus = "Controller";

bool _copyScaffold = false;
void Main()
{
	_location = Path.GetFullPath(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),_location)).Dump();
	var files = Directory.GetFiles(_location, "*" + _template + "*", SearchOption.AllDirectories)
				.Where(file => _fileTypes.Contains(Path.GetExtension(file)) && !_exclude.Any(x => file.Contains(x)))
				.OrderByDescending(x=>x.Contains(_focus))
				;
	var fileReplaces = files.Select(x => new { File = x, Replace = ReplaceAll(x) , Exists = File.Exists(ReplaceAll(x))}).ToList();
	fileReplaces.Where(x=>x.Exists).Select(x=> x.Replace.Replace(_location,"")).Dump("Existing files");
	fileReplaces.Where(x=>!x.Exists).Select(x=> x.Replace.Replace(_location,"")).Dump("Missing files");
	foreach (var replace in fileReplaces)
	{
			var file = replace.File;
			var newFile = replace.Replace;
			if (!replace.Exists) {
				
				var replaceOption = Util.ReadLine("Would you like to create "+newFile+" [Y/n]").ToUpper() != "N";
				if (replaceOption) {
				    
					var fileContent = File.ReadAllText(file);
					fileContent = InjectScaffolding(fileContent);
					var path = Path.GetDirectoryName(newFile);
					if (!Directory.Exists(path)) Directory.CreateDirectory(path);
					File.WriteAllText(newFile,ReplaceAll(fileContent));
					newFile.Dump("Created");
					AddFileToProject(newFile,file);
					
				}
				else {
					newFile.Dump("Skip");
				}
			}
	}
}

public string InjectScaffolding(string fileData)
{
	var prefix = "/* scaffolding";
	var suffix = "scaffolding */";
	var start = fileData.IndexOf(prefix);
	var end = fileData.IndexOf(suffix,Math.Max(0,start));
	if (start < 0 || end < 0) return fileData;
	start.Dump();
	end.Dump();
	var data = fileData.Substring(start+prefix.Length,end-start-prefix.Length).Dump();
	
	var injections = JsonConvert.DeserializeObject<List<FileInjection>>(data).Dump();
	var allfiles = Directory.GetFiles(_location, "*", SearchOption.AllDirectories);
	
	foreach (var inject in injections)
	{
		var inFile = allfiles.Where(x => x.EndsWith("\\"+inject.FileName)).FirstOrDefault();
		if (inFile != null)
		{
			var projectFile = File.ReadAllLines(inFile).ToList();
			var found = false;
			for (int i = 0; i < projectFile.Count; i++)
			{

				if (projectFile[i].Contains(inject.Indexline))
				{
					found = true;
					var indent = Regex.Match(projectFile[i],@"(\s*)[^\s]").Groups[1].Value;
					var indexOf = projectFile[i].IndexOf(inject.Indexline)+ inject.Indexline.Length;
					foreach (var addLine in inject.Lines)
					{
						var insertLine = ReplaceAll(addLine);
						if (inject.InsertInline)
						{
							projectFile[i] = projectFile[i].Insert(indexOf,insertLine);
						}
						else
						{
							var inLine = i + (inject.InsertAbove ? -1 : 0);
							projectFile.Insert(inLine + 1, indent+insertLine);
							i++;
						}
					}
					break;
				}
			}
			if (!found) ("****** could not find "+inject.Indexline+" in the file " + inject.FileName).Dump();
			//if (inject.InsertInline)projectFile.Dump();
			TryIt(10 ,()=>File.WriteAllLines(inFile, projectFile.ToArray()));
			
		}
		else
		{
			("****** could not find " + inject.FileName).Dump();
		}

	}
	
	if (_copyScaffold) return fileData;
	return fileData.Substring(0,start).Trim();
}
public void TryIt(int retryCount, Action action)
{
	Exception last = null;
	for (int i = 0; i < retryCount; i++)
	{
		try
		{	        
			action();
			return;
		}
		catch (Exception ex)
		{
			last= ex;
			ex.Message.Dump();
			Thread.Sleep(1);
		}
	}
	throw last;
}
class Injection
{
	public List<FileInjection> Injections { get; set; }
}
public class FileInjection
{
	public string FileName { get; set; }
	public string Indexline { get; set; }
	public bool InsertAbove { get; set; }
	public bool InsertInline { get; set; }
	public string[] Lines { get; set;}

}

public string AddFileToProject(string fileName, string oldFile) {
	string projectName = null;
	var path = Path.GetDirectoryName(fileName);
	do
	{
		projectName = Directory.GetFiles(path,"*.csproj").FirstOrDefault();
		path = Path.GetDirectoryName(path);
	} while (string.IsNullOrEmpty(projectName) && !string.IsNullOrEmpty(path) );
	if (!string.IsNullOrEmpty(projectName)) {
		var projectFile = File.ReadAllLines(projectName).ToList();
		for (int i = 0; i < projectFile.Count; i++)	
		{
			
			if (projectFile[i].Contains("\\"+Path.GetFileName(oldFile))) {
				
				projectFile.Insert(i+1,ReplaceAll(projectFile[i]).Dump());
			}
		}
		File.WriteAllLines(projectName,projectFile.ToArray());
	}
	return projectName;
}


public string ReplaceAll(string text) {
if (text == null) return null;
	return text
		.Replace(_template.Pluralize(),_toName.Pluralize()) // StockCategories Samples
		.Replace(InitialLower(_template.Pluralize()),InitialLower(_toName.Pluralize()))  // stockCategories samples
		.Replace(_template,_toName) // StockCategory Sample
		.Replace(InitialLower(_template),InitialLower(_toName)) // stockCategory sample
		 
		;
}


public string InitialLower(string text) {
	return text.Substring(0,1).ToLower()+text.Substring(1);
}

// Define other methods and classes here